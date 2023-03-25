using Core;
using Core.Extractor;
using Microsoft.Extensions.Hosting;

namespace BusinessLogin;

    public class ExtractorManager : BackgroundService
    {
        private readonly IExtractorQueue _queue;
        private readonly IExtractorWorkerFactory _workerFactory;

        /// <summary>
        /// Количество доступных воркеров
        /// </summary>
        private int _countWorkers;
    
        /// <summary>
        /// список воркеров
        /// </summary>
        private List<IExtractorWorker> _workers;
        
        public ExtractorManager(IExtractorQueue queue, IExtractorWorkerFactory workerFactory)
        {
            _queue = queue;
            _workerFactory = workerFactory;
        }
        
        /// <summary>
        /// Инициализация воркеров
        /// </summary>
        private void InitializeWorkers()
        {
            _workers = new List<IExtractorWorker>();
            for (int i = 0; i < _countWorkers; i++)
            {
                _workers.Add(_workerFactory.CreateWorker());
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    await HandleWorkerListAsync(cancellationToken);
                }
                catch
                {
                    // продолжаем выполнение задач
                }
            }
        }
        
        /// <summary>
        /// Метод работы с воркерами
        /// </summary>
        /// <param name="cancellationToken"></param>
        private async Task HandleWorkerListAsync(CancellationToken cancellationToken)
        {
            // потоки воркеров
            var tasks = new List<Task>(_workers.Count + 1) { Task.Delay(3000, cancellationToken) };
            foreach (var worker in _workers)
            {
                if (_queue.IsEnd())  // пустая очередь
                    break;
            
                if (!worker.CanGetTask)
                    continue;

                var triggerForTask = _queue.Dequeue();
                if(triggerForTask.IsFailed) 
                    continue;
            
                var task = worker.StartTaskAsync(triggerForTask.Value);
                tasks.Add(task);

            }

            // ждем завершения какой-нибудь из задач, 
            // но продолжаем исполнение кода не раньше чем через IterationDelay
            await Task.WhenAll(
                Task.WhenAny(tasks),
                Task.Delay(IterationDelay, cancellationToken)
            );
        }
    }

using BusinessLogin.ExtTask;
using BusinessLogin.ExtTask.Queue;
using BusinessLogin.Services;
using Core.Extractor;
using Core.Models;
using Microsoft.Extensions.Hosting;

namespace BusinessLogin.Worker;

public class ExtractorWorker : BackgroundService
{
    private const int IterationDelay = 5000;
    private readonly ExtractorFactory _extractorFactory;
    private readonly ITaskQueue _taskQueueService;
    private readonly BookService _service;
    private readonly int _threadCount;

    public ExtractorWorker(ExtractorFactory extractorFactory,
                            ITaskQueue queue,
                            BookService service,
                            int threadCount = 2)
    {
        _extractorFactory = extractorFactory;
        _taskQueueService = queue;
        _service = service;
        _threadCount = threadCount;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Worker started");
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await HandleQueue(stoppingToken);
                await Task.Delay(IterationDelay);
            }
            catch
            {
                // продолжаем выполнение задач
            }
        }
        
        Console.WriteLine("Worker stopped");
    }

    /// <summary>
    /// Взятие в работу задач из очереди
    /// </summary>
    private async Task HandleQueue(CancellationToken cancellationToken)
    {
        if(_taskQueueService.IsEnd())
            return;
        var taskInfo = _taskQueueService.Dequeue();
        var books = await HandleTaskInfoAsync(taskInfo);
        _service.AddRangeAsync(books);
    }

    /// <summary>
    /// Работа над задачей
    /// </summary>
    private async Task<IEnumerable<Book>> HandleTaskInfoAsync(ExtractorTask extractorTask)
    {
        var provider = _extractorFactory.GetResourceInfoProvider(extractorTask.ProviderSettings, extractorTask.ResourceProviderType);
        var extractor = _extractorFactory.GetBookExtractor(extractorTask.ExtractorType);

        var resources = provider.GetResources();
        List<Book> bookResults = new List<Book>();
        Parallel     // распараллеливание работы над задачей на заданное количество потоков
            .ForEach(provider.GetResources(),
                new ParallelOptions() {MaxDegreeOfParallelism = _threadCount}, async info =>
                {           
                    Console.WriteLine($"Started thread with {info.URLResource}");
                    var rawInfo = await extractor.GetRawDataAsync(info);
                    var newBook = await extractor.HandleAsync(rawInfo);
                    lock (bookResults)
                    {
                        bookResults.Add(newBook);
                    }

                    // Console.WriteLine($"Parsed {newBook.Name}");
                });
            //
        return bookResults;
    }                // всё на уровне обработчика задачи, странно работает bookService, 
}
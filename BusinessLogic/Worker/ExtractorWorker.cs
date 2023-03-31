using BusinessLogin.ExtractorTask;
using Core.Models;
using Microsoft.Extensions.Hosting;

namespace BusinessLogin.Worker;

public class ExtractorWorker : BackgroundService
{
    
    
    private const int IterationDelay = 500;
    private readonly ExtractorFactory _extractorFactory;
    private readonly ITaskQueue _taskQueueService;
    private readonly int _threadCount;

    public ExtractorWorker(ExtractorFactory extractorFactory, ITaskQueue queue, int threadCount = 1)
    {
        _extractorFactory = extractorFactory;
        _taskQueueService = queue;
        _threadCount = threadCount;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await HandleQueue(stoppingToken);
            }
            catch
            {
                // продолжаем выполнение задач
            }
        }
    }

    private async Task HandleQueue(CancellationToken cancellationToken)
    {
        var taskInfo = _taskQueueService.Dequeue();
        var provider = _extractorFactory.GetResourceInfoProvider(taskInfo.ProviderSettings, taskInfo.ResourceProviderType);
        var extractor = _extractorFactory.GetBookExtractor(taskInfo.ExtractorType);

        List<Book> bookResults = new List<Book>();
        Parallel
            .ForEach(provider.GetResources(),
            new ParallelOptions() {MaxDegreeOfParallelism = _threadCount}, 
        info =>
            {
                var rawInfo = extractor.GetRawData(info);
                var newBook = extractor.Handle(rawInfo);
                lock (bookResults)
                {
                    bookResults.Add(newBook);
                }
            });
        
    }
}
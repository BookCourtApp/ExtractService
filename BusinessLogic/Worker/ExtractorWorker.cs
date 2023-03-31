using BusinessLogin.ExtractorTask;
using Core.Models;
using Microsoft.Extensions.Hosting;

namespace BusinessLogin.Worker;

public class ExtractorWorker : BackgroundService
{
    private const int IterationDelay = 5000;
    private readonly ExtractorFactory _extractorFactory;
    private readonly ITaskQueue _taskQueueService;
    private readonly int _threadCount;

    public ExtractorWorker(ExtractorFactory extractorFactory, ITaskQueue queue, int threadCount = 2)
    {
        _extractorFactory = extractorFactory;
        _taskQueueService = queue;
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

    private async Task HandleQueue(CancellationToken cancellationToken)
    {
        if(_taskQueueService.IsEnd())
            return;
        var taskInfo = _taskQueueService.Dequeue();
        var provider = _extractorFactory.GetResourceInfoProvider(taskInfo.ProviderSettings, taskInfo.ResourceProviderType);
        var extractor = _extractorFactory.GetBookExtractor(taskInfo.ExtractorType);

        List<Book> bookResults = new List<Book>();
        Parallel
            .ForEach(provider.GetResources(),
            new ParallelOptions() {MaxDegreeOfParallelism = _threadCount}, 
        info =>
            {
                Console.WriteLine($"Started thread with {info.URLResource}");
                var rawInfo = extractor.GetRawData(info);
                var newBook = extractor.Handle(rawInfo);
                lock (bookResults)
                {
                    bookResults.Add(newBook);
                }

               // Console.WriteLine($"Parsed {newBook.Name}");
            });
    }
}
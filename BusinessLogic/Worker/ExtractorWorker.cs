using System.Diagnostics;
using AngleSharp.Dom;
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
                            int threadCount = 5)
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
        if (_taskQueueService.IsEnd())
            return;
        var taskInfo = _taskQueueService.Dequeue();
        await HandleTaskInfoAsync(taskInfo);
        //_service.AddRangeAsync(books);
    }

    /// <summary>
    /// Работа над задачей
    /// </summary>
    private async Task HandleTaskInfoAsync(ExtractorTask extractorTask)
    {
        var provider = _extractorFactory.GetResourceInfoProvider( extractorTask.ResourceProviderType);
        var extractor = _extractorFactory.GetBookExtractor(extractorTask.ExtractorType);

        int counter = 0;
        var timer = Stopwatch.StartNew();
        //var resources = provider.GetResources();
        List<Book> bookResults = new List<Book>();
        Parallel // распараллеливание работы над задачей на заданное количество потоков
            .ForEach(provider.GetResources(),
              new ParallelOptions() { MaxDegreeOfParallelism = _threadCount },
                info =>  HandleInfo(info, ref counter, extractor, timer));

        // foreach (var resourceInfo in provider.GetResources())
        // {
        //     HandleInfo(resourceInfo, ref counter, extractor, timer);
        // }
        return;
    }

    private Task HandleInfo(ResourceInfo info, ref int counter, IExtractor<IDocument, Book> extractor, Stopwatch timer)
    {
        var rawInfo =  extractor.GetRawDataAsync(info).Result;
        var newBook =  extractor.HandleAsync(rawInfo).Result;
        lock (_service)
        {
            newBook.SourceName = info.URLResource;
            var res = _service.AddBookAsync(newBook).Result;
            // switch (res)
            // {
            //     case "error":
            //         Console.WriteLine($"Error on {info.URLResource}");
            //         break;
            //     case "exist":
            //         Console.WriteLine($"Exist on {info.URLResource}");
            //         break;
            // }

            counter++;
            if (counter % 100 == 0)
            {
                
                Console.WriteLine($"counter={counter}; timer= {timer.ElapsedMilliseconds/1000}s");
            }
        }
        return Task.CompletedTask;
    }
}
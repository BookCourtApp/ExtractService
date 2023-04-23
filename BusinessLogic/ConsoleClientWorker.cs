using BusinessLogin.ExtTask;
using BusinessLogin.ExtTask.Queue;
using Microsoft.Extensions.Hosting;

namespace BusinessLogin;

/// <summary>
/// класс который взаимодействует с пользователем через консоль для команды на парсинг
/// </summary>
public class ConsoleClientWorker : BackgroundService
{
    private readonly ExtractorTaskFactory _taskFactory;
    private readonly ITaskQueue _taskQueue;

    public ConsoleClientWorker(ExtractorTaskFactory taskFactory, ITaskQueue taskQueue)
    {
        _taskFactory = taskFactory;
        _taskQueue = taskQueue;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            Console.WriteLine("Введите номер экстрактора из списка, который вы хотите использовать");
            var extractors = _taskFactory.GetExtractors().ToList();
            PrintList(extractors);
            string extractorName = extractors[GetNumber()];
            
            Console.WriteLine("Введите номер провайдера из списка, который вы хотите использовать");
            var providers = _taskFactory.GetProviders().ToList();
            PrintList(providers);
            string providerName = providers[GetNumber()];

            Console.WriteLine("Введите номер настроек из списка, который вы хотите использовать");
            var provodersSettings = _taskFactory.GetSettings().ToList();
            PrintList(provodersSettings);
            string settingsName = provodersSettings[GetNumber()];

            var task = _taskFactory.CreateExtractorTask(providerName, extractorName, settingsName);
            _taskQueue.Enqueue(task);
            Console.WriteLine("Началась работа над задачей.");
            Console.ReadLine();
            await Task.Delay(3000);
           // Console.Clear();
        }
    }

    private void PrintList(List<string> list)
    {
        for(int i = 0; i < list.Count; i++)
            Console.WriteLine($"[{i}] : {list[i]}");
    }

    private int GetNumber()
    {
        Console.Write("Номер: ");
        var numStr = Console.ReadLine();
        return Int32.Parse(numStr);
    }


}
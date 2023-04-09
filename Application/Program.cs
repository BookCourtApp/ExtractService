// See https://aka.ms/new-console-template for more information

using Application;
using BusinessLogin;
using BusinessLogin.ExtTask;
using BusinessLogin.ExtTask.Queue;
using BusinessLogin.Services;
using BusinessLogin.Worker;
using Core.Repository;
using InfrastructureProject;
using InfrastructureProject.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// создание приложения с DI контейнером 
using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services)  =>   // настройка сервисов
    {
        // Добавил dbContext для работы с БД
        services.AddDbContextFactory<ApplicationContext>(options =>
        {
            var config = context.Configuration;
            // sqlite это in-memory db
            var connectionString = config.GetValue<string>("SqliteConnectionString");
            options.UseSqlite(connectionString);      
        });
        services.AddSingleton<IBookRepository, BookRepository>();
        services.AddSingleton<BookService>();
        services.AddSingleton<ExtractorFactory>();
        services.AddSingleton<ExtractorTaskFactory>();
        
        services.AddSingleton<ITaskQueue,TaskQueueLocalMemory>();
        services.AddHostedService<ExtractorWorker>();
        services.AddHostedService<ConsoleClientWorker>();
        
        services.AddExtractors();

    }).Build();

#region taskFactoryUseExample

// ExtractorTaskFactory factory = new ExtractorTaskFactory(host.Services.GetService<IConfiguration>());
// factory.CreateExtractorTask("LabirintResourceInfoProvider", "ExtractorLabirint", "LabirintProviderSettings");

#endregion


#region TasksAddExample

// var queueService = host.Services.GetService<ITaskQueue>();
// for (int i = 0; i < 5; i++)
// {
//     var taskInfo = new ExtractorTask()
//     {
//         ExtractorType = typeof(ExtractorLabirint),
//         ResourceProviderType = typeof(LabirintResourceInfoProvider),
//         ProviderSettings = new LabirintProviderSettings()
//         {
//             MinId = i*5,
//             MaxId = i*5+5,
//             CatalogUrl = "https://www.labirint.ru/books/"
//         }
//     };
//     queueService.Enqueue(taskInfo);
// }

#endregion

#region DbDepednencyExample

//получаем из зависимостей контекст
//var context = host.Services.GetService<ApplicationContext>();

// context.Books.Add(new Book()
// {
//     Name = "",
//     Author = "aaa",
//     Description = "",
//     Genre = "",
//     Image = "",
//     ISBN = "",
//     SourceName = ""
// });

// Тест связей
// var extractorId = Guid.NewGuid();
// context.ExtractorResults.Add(
//     new ExtractorResult()
//     {
//         Id = extractorId,
//         AverageBookProcessing = DateTime.Now,
//         ExtractorDataCount = 10,
//         Errors = new List<Error>()
//         {
//             new Error()
//             {
//                 Id = Guid.NewGuid(),
//                 ExtractorResultId = extractorId,
//                 Reason = ""
//             },
//             new Error()
//             {
//             Id = Guid.NewGuid(),
//             ExtractorResultId = extractorId,
//             Reason = "aa"
//         }
//         }
//     });
// context.SaveChanges();


#endregion
await host.RunAsync();
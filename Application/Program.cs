// See https://aka.ms/new-console-template for more information

using Application;
using BusinessLogin;
using BusinessLogin.ExtTask;
using BusinessLogin.ExtTask.Queue;
using BusinessLogin.Services;
using BusinessLogin.Worker;
using Core.Models;
using Core.Repository;
using ExtractorProject.Extractors;
using InfrastructureProject;
using InfrastructureProject.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

//создание приложения с DI контейнером 
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
        services.AddSingleton<UserRepository>();
        services.AddSingleton < UserPreferenceRepository>();
        
        services.AddSingleton<BookService>();
        services.AddSingleton<UserService>();
        services.AddSingleton < UserPreferenceService>();
        services.AddSingleton<ExtractorFactory>();
        services.AddSingleton<ExtractorTaskFactory>();
        
        services.AddSingleton<ITaskQueue,TaskQueueLocalMemory>();
        services.AddHostedService<ExtractorWorker>();
        services.AddHostedService<ConsoleClientWorker>();
        
        services.AddExtractors();

    }).Build();
var service = host.Services.GetService<UserService>();

var serviceEx = host.Services.GetService<UserPreferenceService>();
LiveLibUserExtractor userExtractor = new LiveLibUserExtractor();
LiveLibUserInfoExtractor ex = new LiveLibUserInfoExtractor();
for (int i = 100000; i < 100001; i++)
{
    var res = userExtractor.GetRawDataAsync(new ResourceInfo() { URLResource = "https://www.livelib.ru/readers/listview/smalllist~" +i}).Result;
    if(res.Title.Contains("LiveLib"))
        Console.WriteLine($"DANGER!!!!!!! {res.Title} : {i}");
    
    var users = await userExtractor.HandleAsync(res);
    if(users.Count() > 0)
        await service.AddRangeAsync(users);
    foreach (var user in users)
    {
        int numPage = 1;
        List<UserPreference> preferences = new List<UserPreference>();
        int countparsed = -1;
        while (countparsed == -1 || countparsed > 0)
        {
            countparsed = 0;
            var resource = new ResourceInfo()
                { URLResource = "https://www.livelib.ru/reader/" + user.UserLogin + "/read~" + numPage };
            var dataex = await ex.GetRawDataAsync(resource);
            var resex = await ex.HandleAsync(dataex);
            countparsed = resex.Count();
            preferences.AddRange(resex);
        }

        serviceEx.AddRangeAsync(preferences);

    }
    Console.WriteLine(res.Title);            // https://www.livelib.ru/service/ratelimitcaptcha - url with captcha
}
var data =await ex.GetRawDataAsync(new ResourceInfo() { URLResource = "https://www.livelib.ru/reader/Shakespeare/read~1" });
var userpref =await ex.HandleAsync(data);
Console.WriteLine("aa");
// await host.RunAsync();

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

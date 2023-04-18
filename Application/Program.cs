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
using ExtractorProject.Settings;
using InfrastructureProject;
using InfrastructureProject.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;


//создание приложения с DI контейнером 
using IHost host = Host.CreateDefaultBuilder(args).ConfigureLogging(loggin =>
    {
        loggin.SetMinimumLevel(LogLevel.Warning);
    })
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
        

        services.AddExtractors(context.Configuration);
        
    }).UseSerilog((context, services, config) =>
    {
        config.WriteTo.Console(LogEventLevel.Warning)
            .WriteTo.File("logs.txt", LogEventLevel.Warning);
    }).Build();

var service = host.Services.GetService<UserService>();
var logger = host.Services.GetService<ILogger<Program>>();


var serviceEx = host.Services.GetService<UserPreferenceService>();
var usSet = new LiveLibUserExtractorSettingsInfo()
{
    Cookie =
        "LiveLibId=b8b79824f39aee14fdc09e0ece2fae19; __ll_tum=3495147849; __ll_ab_mp=1; __ll_unreg_session=b8b79824f39aee14fdc09e0ece2fae19; __ll_unreg_sessions_count=1; tmr_lvid=fe30080f574fb4fbf3871dc58b83be4f; tmr_lvidTS=1681620833896; _ga=GA1.2.685237644.1681620834; _gid=GA1.2.1667689490.1681620834; _ym_uid=1681620835986693727; _ym_d=1681620835; _ym_isad=2; _ym_visorc=b; iwatchyou=5df35936713e92b51205077989eee0cc; __llutmz=-600; __ll_fv=1681620901; __ll_dvs=5; __ll_cp=1; ll_asid=1246767843; __ll_popup_count_pviews=regc1_; __ll_google_oauth=; __ll_google_code=; llsid=98440d279f26ffcd38e8b2d00d0ec0fe; __utnx=12000205153; __llutmf=1; __utnt=g0_y0_a15721_u12000205153_c0; __ll_dv=1681620997; tmr_detect=0%7C1681621244655 ;_GRECAPTCHA=09AMqPRJwCrCJqa1Dz9seWcvLVRxRuIWHft81RkeBiChzgi5USgczb22xEMPuihJr_zHPHA9InPKUypdM6h9g3Gvs; 1P_JAR=2023-04-16-12; NID=511=Ma22WXo7e_5MrejpmvCQQTO-e-6bPzeNVotx2fI9Comd4URzmswyuos8EnC8rCVjndO7F1v58f1OVxmz1VGQsL-FB095NQvwRclrIMrwLf7HeMnZtw4w6j_ONTust--veO1v8_GrQFj5XByZ8Vm8EVoKbc_TlZQRkY8JrmH4vi4"
};
var usprefset = new LiveLibUserPreferenceExtractorSettingsInfo()
{
    Cookie =
        "LiveLibId=b8b79824f39aee14fdc09e0ece2fae19; __ll_tum=3495147849; __ll_ab_mp=1; __ll_unreg_session=b8b79824f39aee14fdc09e0ece2fae19; __ll_unreg_sessions_count=1; tmr_lvid=fe30080f574fb4fbf3871dc58b83be4f; tmr_lvidTS=1681620833896; _ga=GA1.2.685237644.1681620834; _gid=GA1.2.1667689490.1681620834; _ym_uid=1681620835986693727; _ym_d=1681620835; _ym_isad=2; _ym_visorc=b; iwatchyou=5df35936713e92b51205077989eee0cc; __llutmz=-600; __ll_fv=1681620901; __ll_dvs=5; __ll_cp=1; ll_asid=1246767843; __ll_popup_count_pviews=regc1_; __ll_google_oauth=; __ll_google_code=; llsid=98440d279f26ffcd38e8b2d00d0ec0fe; __utnx=12000205153; __llutmf=1; __utnt=g0_y0_a15721_u12000205153_c0; __ll_dv=1681620997; tmr_detect=0%7C1681621244655 ;_GRECAPTCHA=09AMqPRJwCrCJqa1Dz9seWcvLVRxRuIWHft81RkeBiChzgi5USgczb22xEMPuihJr_zHPHA9InPKUypdM6h9g3Gvs; 1P_JAR=2023-04-16-12; NID=511=Ma22WXo7e_5MrejpmvCQQTO-e-6bPzeNVotx2fI9Comd4URzmswyuos8EnC8rCVjndO7F1v58f1OVxmz1VGQsL-FB095NQvwRclrIMrwLf7HeMnZtw4w6j_ONTust--veO1v8_GrQFj5XByZ8Vm8EVoKbc_TlZQRkY8JrmH4vi4"
};
var userExtractor = host.Services.GetService<LiveLibUserExtractor>();
var ex = host.Services.GetService<LiveLibUserPreferenceExtractor>();
// var res2 = await ex.GetRawDataAsync(new ResourceInfo() { URLResource = "https://www.livelib.ru/reader/YanaDarmograj/read" });
// var data = await ex.HandleAsync(res2);

for (int i = 305; i < 100000; i++)
{
    var res = userExtractor.GetRawDataAsync(new ResourceInfo() { URLResource = "https://www.livelib.ru/readers/listview/smalllist~" +i}).Result;
    if (res.Title.Contains("LiveLib"))
    {
        Console.WriteLine($"DANGER!!!!!!! {res.Title} : {i}");
        
        Console.WriteLine($"DANGER!!!!!!! {res.Title} : {i}");
        
        Console.WriteLine($"DANGER!!!!!!! {res.Title} : {i}");
        logger.LogError($"DANGER!!!!! {res.Title} : {i}");
        Thread.Sleep(10000);
    }
    
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
            numPage++;
        }
        countparsed = -1;
        
        numPage = 1;
        
        while (countparsed == -1 || countparsed > 0)
        {
            countparsed = 0;
            var resource = new ResourceInfo()
                { URLResource = "https://www.livelib.ru/reader/" + user.UserLogin + "/wish~" + numPage };
            var dataex = await ex.GetRawDataAsync(resource);
            var resex = await ex.HandleAsync(dataex);
            countparsed = resex.Count();
            preferences.AddRange(resex);
            numPage++;
        }

        numPage = 1;
        countparsed = -1;
        while (countparsed == -1 || countparsed > 0)
        {
            countparsed = 0;
            var resource = new ResourceInfo()
                { URLResource = "https://www.livelib.ru/reader/" + user.UserLogin + "/reading~" + numPage };
            var dataex = await ex.GetRawDataAsync(resource);
            var resex = await ex.HandleAsync(dataex);
            countparsed = resex.Count();
            preferences.AddRange(resex);
            numPage++;
        }

        serviceEx.AddRangeAsync(preferences);
        Console.WriteLine($"[{i}]: User {user.UserLink} handled, {preferences.Count()} preferences");
        logger.LogWarning($"[{i}]: User {user.UserLink} handled, {preferences.Count()} preferences");
    }
    Console.WriteLine(res.Title);            // https://www.livelib.ru/service/ratelimitcaptcha - url with captcha
}
// var data =await ex.GetRawDataAsync(new ResourceInfo() { URLResource = "https://www.livelib.ru/reader/Shakespeare/read~1" });
// var userpref =await ex.HandleAsync(data);
// Console.WriteLine("aa");
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

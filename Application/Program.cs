// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
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
var context = host.Services.GetRequiredService<ApplicationContext>();
context.Database.EnsureCreated();

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
var ex = host.Services.GetService<LiveLibUserPreferenceExtractorJson>();
// var res2 = await ex.GetRawDataAsync(new ResourceInfo() { URLResource = "https://www.livelib.ru/reader/YanaDarmograj/read" });
// var data = await ex.HandleAsync(res2);

while (true)
{
    Console.Write("Введи по очереди:\n startPage=");
    int startPage = Int32.Parse(Console.ReadLine());
    Console.Write("countOneBatch=");
    int countOneBatch = Int32.Parse(Console.ReadLine());
    Console.Write("countIterations=");
    int countIterations = Int32.Parse(Console.ReadLine());
    
    Console.Write("countMaxThreads=");
    int countMaxThreads = Int32.Parse(Console.ReadLine());
    DoParse(startPage, countOneBatch, countIterations, countMaxThreads);
}

void DoParse(int startPage, int countOneBatch, int countIterations, int countMaxThreads = 8)
{
    List<int> pages = new List<int>();
    for (int j = 0; j < countIterations; j++)
    {
        pages.Clear();
        for (int i = startPage + j * countOneBatch; i < startPage + (j + 1) * countOneBatch; i++)
        {
            pages.Add(i);
        }

        int startPageCol = pages.First(), lastPageCol = pages.Last();
        var res = Parallel.ForEach(pages, new ParallelOptions() { MaxDegreeOfParallelism = 5 }, async (i) =>
        {
            try
            {
                var res = userExtractor.GetRawDataAsync(new ResourceInfo()
                    { URLResource = "https://www.livelib.ru/readers/listview/smalllist~" + i }).Result;
                if (res.Title.Contains("LiveLib"))
                {
                    // Console.WriteLine($"DANGER!!!!!!! {res.Title} : {i}");
                    //
                    // Console.WriteLine($"DANGER!!!!!!! {res.Title} : {i}");
                    //
                    // Console.WriteLine($"DANGER!!!!!!! {res.Title} : {i}");
                    logger.LogError($"DANGER!!!!! {res.Title} : {i}");
                    Thread.Sleep(10000);
                }

                var users = userExtractor.HandleAsync(res).Result;

                // if (users.Count() > 0)
                // {
                //     lock (service)
                //         service.AddRangeAsync(users);
                // }

                foreach (var user in users)
                {
                    lock (service)
                        service.AddAsync(user);
                    var timer = Stopwatch.StartNew();
                    List<UserPreference> preferences = new List<UserPreference>();
                    preferences.AddRange(ExtractPreferences(ex, user, "read").Result);
                    preferences.AddRange(ExtractPreferences(ex, user, "reading").Result);

                    preferences.AddRange(ExtractPreferences(ex, user, "wish").Result);

                    if (preferences.Count == 0)
                        logger.LogWarning("Preferences are empty, userlink: {0}", user.UserLink);
                    lock (serviceEx)
                        serviceEx.AddRangeAsync(preferences);
                    timer.Stop();
                    //Console.WriteLine($"[{i}]: User {user.UserLink} handled, {preferences.Count()} preferences");
                    logger.LogWarning("[{0}]: User {1} handled, {2} preferences; {3}s", i, user.UserLink,
                        preferences.Count,
                        timer.ElapsedMilliseconds / 1000);
                }
            }
            catch (Exception ex)
            {
                logger.LogCritical("Ошибка при потоке {0}; message:{1},\n stack:{2}", i, ex.Message, ex.StackTrace);
            }
        });
        while (!res.IsCompleted)
        {
            Thread.Sleep(5000);
        }
        logger.LogCritical("HANDLED USERS SINCE {0} to {1}", startPageCol, lastPageCol);

    }
}
// for (int i = 300; i < 100000; i++)
// {
//     var res = userExtractor.GetRawDataAsync(new ResourceInfo() { URLResource = "https://www.livelib.ru/readers/listview/smalllist~" +i}).Result;
//     if (res.Title.Contains("LiveLib"))
//     {
//         Console.WriteLine($"DANGER!!!!!!! {res.Title} : {i}");
//         
//         Console.WriteLine($"DANGER!!!!!!! {res.Title} : {i}");
//         
//         Console.WriteLine($"DANGER!!!!!!! {res.Title} : {i}");
//         logger.LogError($"DANGER!!!!! {res.Title} : {i}");
//         Thread.Sleep(10000);
//     }
//     
//     var users = await userExtractor.HandleAsync(res);
//     if(users.Count() > 0)
//         await service.AddRangeAsync(users);
//     foreach (var user in users)
//     {
//         var timer = Stopwatch.StartNew();
//         List<UserPreference> preferences = new List<UserPreference>();
//         preferences.AddRange(await ExtractPreferences(ex, user, "read"));
//         preferences.AddRange(await ExtractPreferences(ex, user, "reading"));
//
//         preferences.AddRange(await ExtractPreferences(ex, user, "wish"));
//         
//         if(preferences.Count == 0)
//             logger.LogWarning("Preferences are empty, userlink: {0}", user.UserLink);
//         serviceEx.AddRangeAsync(preferences);
//         timer.Stop();
//         Console.WriteLine($"[{i}]: User {user.UserLink} handled, {preferences.Count()} preferences");
//         logger.LogWarning("[{0}]: User {1} handled, {2} preferences; {3}s", i, user.UserLink, preferences.Count, timer.ElapsedMilliseconds/1000);
//     }
//     Console.WriteLine(res.Title);            // https://www.livelib.ru/service/ratelimitcaptcha - url with captcha
// }
//
// async Task HandlePage(int page)
// {
//     
// }
// var data =await ex.GetRawDataAsync(new ResourceInfo() { URLResource = "https://www.livelib.ru/reader/Shakespeare/read~1" });
// var userpref =await ex.HandleAsync(data);
// Console.WriteLine("aa");
// await host.RunAsync();

async Task<IEnumerable<UserPreference>> ExtractPreferences(LiveLibUserPreferenceExtractorJson extractor, User user, string type)
{
    var preferences = new List<UserPreference>();
    var numPage = 1;
    var countparsed = -1;
    
    while (countparsed == -1 || countparsed > 0)
    {
        countparsed = 0;
        var url = $"https://www.livelib.ru/reader/{user.UserLogin}/{type}/listview/smalllist?page_no={numPage}&per_page=2000&is_new_design=ll2015b";

        var resource = new ResourceInfo()
            { URLResource = url};          
        var dataex = await extractor.GetRawDataAsync(resource);
        var resex = await extractor.HandleAsync(dataex);
        countparsed = resex.Count();
        preferences.AddRange(resex);
        numPage++;
    }
    HandlePreferences(preferences, user, type);
    
    return preferences;
}
#region taskFactoryUseExample

void HandlePreferences(IEnumerable<UserPreference> preferences, User user, string type)
{
    string realType = "";
    switch (type)
    {
        case "read":
            realType = "прочитал";
            break;
        case "reading":
            realType = "не дочитал";
            break;
        case "wish":
            realType = "хочет прочитать";
            break;
    }
    foreach (var preference in preferences)
    {
        preference.UserLink = user.UserLink;
        preference.UserLogin = user.UserLogin;
        preference.PreferenceType = type;
    }
}
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

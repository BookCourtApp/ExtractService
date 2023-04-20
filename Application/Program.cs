// See https://aka.ms/new-console-template for more information

using InfrastructureProject;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

//создание приложения с DI контейнером 
using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services)  =>   // настройка сервисов
    {
        // Добавил dbContext для работы с БД
        services.AddDbContext<ApplicationContext>(options =>
        {
            var config = context.Configuration;
            // sqlite это in-memory db
            var connectionString = config.GetValue<string>("SqliteConnectionString");
            options.UseSqlite(connectionString);      
        });
    }).Build();

#region ExtractorCreationExample
// ResourceProviderSettings settings = new ResourceProviderSettings()
// {
//     Site = "https://primbook.ru",
//     Info = new PrimbookProviderSettings
//     {
//         CatalogUrls = new[] { "https://primbook.ru/catalog/detskaya/?PAGEN_1=" }
//     }
// };
// PrimbookResourceInfoProvider provider = new PrimbookResourceInfoProvider(settings);
// ExtractorPrimbook extractor = new ExtractorPrimbook();
// foreach (var resourceInfo in provider.GetResources())
// {
//     var raw = extractor.GetRawData(resourceInfo);
//     var book = extractor.Handle(raw);
//     Console.WriteLine(book.Name);
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
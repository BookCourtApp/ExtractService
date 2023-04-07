// See https://aka.ms/new-console-template for more information

using Core.Models;
using Core.Settings;
using ExtractorProject.Extractors;
using ExtractorProject.ResourceProvider;
using ExtractorProject.Settings;
using InfrastructureProject;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// создание приложения с DI контейнером 
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

#region DbDepednencyExample

//получаем из зависимостей контекст
var context = host.Services.GetService<ApplicationContext>();

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

Console.WriteLine(context.Errors.Count());

Vault14ProviderSettings vault14ProviderSettings = new Vault14ProviderSettings();
ResourceProviderSettings resourceProviderSettings = new ResourceProviderSettings();
resourceProviderSettings.Info = vault14ProviderSettings;
Vault14ResourceInfoProvider vault14ResourceInfoProvider = new Vault14ResourceInfoProvider(resourceProviderSettings);
IEnumerable<ResourceInfo> urls = vault14ResourceInfoProvider.GetResources();
foreach (var url in urls)
{
    Console.WriteLine(url.URLResource);
    ExtractorVault14 extractorVault14 = new ExtractorVault14();
    Book book = extractorVault14.Handle(extractorVault14.GetRawData(url));
    Console.WriteLine(book.Name + "Имя");
    Console.WriteLine(book.Description + "Описание");
    Console.WriteLine(book.SiteBookId);
    Console.WriteLine(book.Author + "Автор"); 
    Console.WriteLine(book.ISBN + "ISBN");
    Console.WriteLine(book.Genre + "Жанр");
    Console.WriteLine(book.Image + "Изобр");
    Console.WriteLine(book.NumberOfPages + "str");
}
#endregion

await host.RunAsync();
// See https://aka.ms/new-console-template for more information

using ExtractorService.Models;
using InfrastructureProject;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// создание приложения с DI контейнером 
using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>   // настройка сервисов
    {
        // Добавил dbContext для работы с БД
        services.AddDbContext<ApplicationContext>(options =>
        {
            // захардкодил путь до БД, перед запуском нужно поставить свой путь.В существуюзей папке будет создаваться/использоваться .db файл как БД sqlite
            // sqlite это in-memory db
            options.UseSqlite(@"Data Source=C:\Users\ted70\Work\BookCourt\db\myDb.db");      
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


#endregion

await host.RunAsync();
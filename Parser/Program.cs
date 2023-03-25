// See https://aka.ms/new-console-template for more information

using Core.Database;
using InfrastructureProject;
using LabirintExtractor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
optionsBuilder.UseSqlite("Data Source=labirint.db");
ApplicationContext context = new ApplicationContext(optionsBuilder.Options);
BookService service = new BookService(context);
ExtractorBooks parser = new ExtractorBooks(context);

List<Task> tasks = new List<Task>();
tasks.Add(Task.Run(() => parser.Parse("https://www.labirint.ru/books/", 1, 1_0)));
tasks.Add(Task.Run(() => parser.Parse("https://www.labirint.ru/books/", 1_0, 2_0)));
//
// parser.Parse("https://www.labirint.ru/books/", 848248, 848800);
Task.WhenAll(tasks).Wait();

Console.WriteLine("FinishWork");
Console.ReadLine();
// See https://aka.ms/new-console-template for more information

using InfrastructureProject;
using LabirintExtractor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var options = new DbContextOptions<ApplicationContext>();
var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
optionsBuilder.UseSqlite("Data Source=labirint.db");
ApplicationContext context = new ApplicationContext(options);
ExtractorBooks parser = new ExtractorBooks(context);

List<Task> tasks = new List<Task>();

parser.Parse("https://www.labirint.ru/books/", 848248, 848800);

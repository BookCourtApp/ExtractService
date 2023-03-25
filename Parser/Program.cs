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
ExtractorBooks parser = new ExtractorBooks(service);

int start = 1;
int end = 500_000;
int batch = 500;
int countIterations = end-start+1;
int countBatches = countIterations % batch == 0 ? countIterations / batch : countIterations / batch + 1;
List<(string, int, int)> periods = new List<(string, int, int)>();
for (int i = 0; i < countBatches; i++)
{
    int startBatch = start + batch * i;
    int endBatch = startBatch + batch;
    if (i == countBatches - 1)
        endBatch = end;
    periods.Add(("https://www.labirint.ru/books/",startBatch, endBatch));
}

Parallel.ForEach(periods, tuple =>parser.Parse(tuple));

start = 500_001;
end = 1_000_000;
batch = 500;
countIterations = end-start+1;
countBatches = countIterations % batch == 0 ? countIterations / batch : countIterations / batch + 1;

periods = new List<(string, int, int)>();
for (int i = 0; i < countBatches; i++)
{
    int startBatch = start + batch * i;
    int endBatch = startBatch + batch;
    if (i == countBatches - 1)
        endBatch = end;
    periods.Add(("https://www.labirint.ru/books/",startBatch, endBatch));
}

Parallel.ForEach(periods, tuple =>parser.Parse(tuple));

context.Dispose();
Console.WriteLine("FinishWork");
Console.ReadLine();
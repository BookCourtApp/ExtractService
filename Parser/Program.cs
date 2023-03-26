// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using Core.Database;
using InfrastructureProject;
using LabirintExtractor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

//var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
//optionsBuilder.UseSqlite("Data Source=labirint.db");
//ApplicationContext context = new ApplicationContext(optionsBuilder.Options);
BookService service = new BookService(new DbContextFactory(),"labirint.db");
ExtractorBooks parser = new ExtractorBooks(service);

bool isEnd = false;
while (!isEnd)
{
    Stopwatch timer = Stopwatch.StartNew();
    try
    {
        timer.Restart();
        Console.WriteLine("Введи первую границу id-шников для парсинга");
        int start = Console.Read();
        Console.WriteLine("Введи вторую границу id-шников для парсинга");
        int end = Console.Read();
        ; //100_000;
        
        Console.WriteLine("Введи количество id-шников для одного батча");
        int batch = 1;
        ; //1000;
        int countIterations = end - start + 1;
        int countBatches = countIterations % batch == 0 ? countIterations / batch : countIterations / batch + 1;
        
        List<(string, int, int)> periods = new List<(string, int, int)>();
        for (int i = 0; i < countBatches; i++)
        {
            int startBatch = start + batch * i;
            int endBatch = startBatch + batch;
            if (i == countBatches - 1)
                endBatch = end;
            periods.Add(("https://www.labirint.ru/books/", startBatch, endBatch));
        }

        Console.WriteLine($"Начался экстрактинг с {start} по {end} id.");
        Parallel.ForEach(periods, tuple => parser.Parse(tuple));
        
        timer.Stop();
        Console.WriteLine($"[{DateTime.Now}]: {start} по {end} id спаршены.\n" +
                          $"Парсинг занял {timer.ElapsedMilliseconds/1000} s");
        

        Console.WriteLine("Если хочешь закончить, введи \"0\"; Если продолжить - любую другую строку");
        var isEndStr = Console.ReadLine();
        if ((isEndStr == "0"))
            isEnd = true;
        Console.WriteLine("-------------------------------------");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Ты чет поломал, давай заного");
    }
}


//
// start = 100_001;
// end = 200_000;
// batch = 1000;
// countIterations = end-start+1;
// countBatches = countIterations % batch == 0 ? countIterations / batch : countIterations / batch + 1;
//
// periods = new List<(string, int, int)>();
// for (int i = 0; i < countBatches; i++)
// {
//     int startBatch = start + batch * i;
//     int endBatch = startBatch + batch;
//     if (i == countBatches - 1)
//         endBatch = end;
//     periods.Add(("https://www.labirint.ru/books/",startBatch, endBatch));
// }
//
// Parallel.ForEach(periods, tuple =>parser.Parse(tuple));

//context.Dispose();
Console.WriteLine("FinishWork");
Console.ReadLine();
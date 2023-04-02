using System.Diagnostics;
using Core.Database;
using ExtractorService.Parser;
using InfrastructureProject;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
BookService service = new BookService(new DbContextFactory(), "book24.db");
ExtractorBook24 Parser = new ExtractorBook24(service);

bool isEnd = false;
while (!isEnd)
{
    Stopwatch timer = Stopwatch.StartNew();
    try
    {
        timer.Restart();
        Console.WriteLine("Введи первую границу страниц для парсинга");
        int start = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Введи вторую границу страниц для парсинга");
        int end = Convert.ToInt32(Console.ReadLine());
        //100_000;

        Console.WriteLine("Сколько поток берет страниц(Меньше число - больше потоков):");
        int batch = Convert.ToInt32(Console.ReadLine());
        ; //1000;
        int countIterations = end - start + 1;
        int countBatches = countIterations % batch == 0 ? countIterations / batch : countIterations / batch + 1;

        List<(int, int)> periods = new List<(int, int)>();
        for (int i = 0; i < countBatches; i++)
        {
            int startBatch = start + batch * i;
            int endBatch = startBatch + batch;
            if (i == countBatches - 1)
                endBatch = end;
            periods.Add((startBatch, endBatch));
        }

        Console.WriteLine($"Начался экстрактинг с {start} по {end} id.");
        Parallel.ForEach(periods,
                        new ParallelOptions(){MaxDegreeOfParallelism = 4},  // MaxDegreeOfParallelism - количество потоков которые будут использоваться
                        tuple => Parser.InitParsing(tuple));

        timer.Stop();
        Console.WriteLine($"[{DateTime.Now}]: {start} по {end} страницы спаршены.\n" +
                          $"Парсинг занял {timer.ElapsedMilliseconds / 1000} s");


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
//namespace ExtractorService.Parser
//{
//    class Program
//    {
//        public static void Main(){
//            try{
//                BookService service = new BookService(new DbContextFactory(), "book24.db"); 
//                ExtractorBook24 Parser= new ExtractorBook24(service);
                
//                Console.WriteLine("Введите страницу для старта парсинга:");
//                int start = Convert.ToInt32(Console.ReadLine());
//                Console.WriteLine("Введите страницу для конца парсинга:");
//                int end = Convert.ToInt32(Console.ReadLine()); 
//                Console.WriteLine("Сколько страниц в потоке: ");
//                int threads = Convert.ToInt32(Console.ReadLine()); 
                
//                List<(int, int)> ConfigToStart = new List<(int, int)>();
//                for(int i = 0; i < end/threads; i++){
//                    var StartFrom = start + i * threads;
//                    var EndOn = (i + 1) * threads;
//                    ConfigToStart.Add((StartFrom, EndOn));

//                    if(i + 1 == end / threads && EndOn < end){
//                        StartFrom = EndOn + 1;
//                        ConfigToStart.Add((StartFrom, end));
//                    }
//                }
//                Parallel.ForEach(ConfigToStart, OneConfig =>  Parser.InitParsing(OneConfig)); 
//                Console.WriteLine("end");
//            }
//            catch (Exception ex){
//                Console.WriteLine($"Ошибка во время парсинга: {ex.Message}");
//            }
//        }
//    }
//}

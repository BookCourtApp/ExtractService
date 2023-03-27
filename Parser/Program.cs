using System.Diagnostics;
using Core.Database;
using InfrastructureProject;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ExtractorService.Parser
{
    class Program
    {
        public static async Task Main(){
            try{
                BookService service = new BookService(new DbContextFactory(), "book24.db"); 
                ExtractorBook24 Parser= new ExtractorBook24(service);
                
                Console.WriteLine("Введите страницу для старта парсинга:");
                int start = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Введите страницу для конца парсинга:");
                int end = Convert.ToInt32(Console.ReadLine()); 
                Console.WriteLine("Сколько страниц в потоке: ");
                int threads = Convert.ToInt32(Console.ReadLine()); 
                
                List<(int, int)> ConfigToStart = new List<(int, int)>();
                for(int i = 0; i < end/threads; i++){
                    var StartFrom = start + i * threads;
                    var EndOn = (i + 1) * threads;
                    ConfigToStart.Add((StartFrom, EndOn));

                    if(i + 1 == end / threads && EndOn < end){
                        StartFrom = EndOn + 1;
                        ConfigToStart.Add((StartFrom, end));
                    }
                }
                Parallel.ForEach(ConfigToStart, OneConfig => Parser.InitParsing(OneConfig)); 
                Console.WriteLine("end");
            }
            catch (Exception ex){
                Console.WriteLine($"Ошибка во время парсинга: {ex.Message}");
            }
        }
    }
}

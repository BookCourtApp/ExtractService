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
                BookService service = new BookServices(new DbContextFactory(), "book24.db"); 
                 
                
                Console.WriteLine("Введи первую границу id-шников для парсинга");
                int start = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Введи вторую границу id-шников для парсинга");
                int end = Convert.ToInt32(Console.ReadLine()); 
            }
            catch (Exception ex){
                Console.WriteLine($"Ошибка во время парсинга: {ex.Message}");
            }
        }
    }
}

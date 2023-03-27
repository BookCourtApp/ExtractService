using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Newtonsoft.Json;
using System;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Net;
using System.Text;
using AngleSharp;
using AngleSharp.Dom;


using Core.Database;
using ExtractorProject.Extractors.Models;
using InfrastructureProject;

namespace ExtractorService.Parser
{
    class ExtractorBook24
    {
        private readonly BookService _service;

        public ExtractorBook24(BookService service){
            _service = service; 
        }
        public async Task InitParsing((int StartPage, int LastPage) ConfigToStart){
            await Parse(ConfigToStart);
        }
        private /*static*/ async Task Parse((int StartPage, int LastPage) ConfigToStart){

            //Для логирования и контроля старта/конца парсинга
            Logger Logging = new Logger($"Log_book24_{DateTime.Now.ToString("MMMM d, yyyy").Replace(" ", "_").Replace(",","")}");
            int NumberOfStartingPage = ConfigToStart.StartPage;
            int NumberOfLastPage = ConfigToStart.LastPage;
            int PageCounter = NumberOfStartingPage;
            int BookCounter = 0;
            string CurrentPageUrl; 
            Logging.Log($"Запуск: Стартовая страница:{NumberOfStartingPage}, Конечная страница:{NumberOfLastPage}");

            //Для накопления книг в batch
            List<Book> Batch;

            var address = "https://book24.ru/catalog/page-";
            while(PageCounter <= NumberOfLastPage){
                if (!await IsPageFound(CurrentPageUrl = $"{address}{PageCounter++}")){
                    Logging.Error($"Страница не найдена: PageURL:{CurrentPageUrl}");
                    Console.WriteLine($"Страница не найдена: Page:{PageCounter}, PageURL:{CurrentPageUrl}");
                    continue;
                }

                Console.WriteLine($"Parsing page: {CurrentPageUrl}");
                var document = GetDocument(CurrentPageUrl);
                var BookPages = document
                    .GetElementsByClassName("product-list__item");

                if ((BookPages.Length == 0))
                {
                    Console.WriteLine($"Нет книг на странице");
                    Logging.Error($"Нет книг на странице: PageURL:{CurrentPageUrl}");
                    continue;
                }

                BookCounter = 0; //зануляем счетчик спарешнных книг со странички
                Batch = new List<Book>(); 
                foreach(var BookPage in BookPages){
                    //Логирование
                    Console.Clear();
                    Console.WriteLine($"Парсинг от {NumberOfStartingPage} страницы до {NumberOfLastPage}");
                    Console.WriteLine($"Page#:{PageCounter-1}");
                    Console.WriteLine($"Book#:{++BookCounter}");

                    var BookPageUrl = "https://book24.ru" + BookPage
                                .GetElementsByClassName("product-card__image-holder")[0]
                                .Children[0]
                                .Attributes["href"]
                                .Value;

                    document = GetDocument(BookPageUrl);
                    if(document == null){
                        Console.WriteLine("Книга не найдена");
                        Logging.Error($"Не найдена книга: Page:{PageCounter-1}, Product:{BookCounter}, URL:{BookPageUrl}");
                        continue;
                    }

                    if(!IsProductValid(document)){
                        Console.WriteLine("Товар не является книгой");
                        Logging.Error($"Товар не является книгой: Page:{PageCounter-1}, Product:{BookCounter}, URL:{BookPageUrl}");
                        continue;
                    }

                    //Парсинг книги
                    try{
                        Book BookInfo = new Book{
                            Name            = getBookName(document),
                            Author          = getAuthor(document),
                            Genre           = getGenre(document),
                            Description     = getDescription(document),
                            NumberOfPages   = getPages(document),
                            Price           = getPrice(document),
                            SourceName      = BookPageUrl,
                            Image           = getImage(document),
                            ParsingDate     = DateTime.UtcNow.ToUniversalTime(),
                            ISBN            = getISBN(document),
                            PublisherYear   = getYear(document),
                            Breadcrqmbs     = getBreadcrumbs(document),
                            SiteBookId      = getVendor(document),
                            SourceUrl       = "book24"
                        };
                        Batch.Add(BookInfo);
                    }
                    catch (Exception ex){
                        Console.WriteLine("Ошибка при парсинге книги: " + ex.Message);
                        Logging.Error($"Ошибка при парсинге книги: Page:{PageCounter-1}, Product:{BookCounter}, URL:{BookPageUrl}");
                    }
                }
                //WriteBatchToJson(Batch);
                await AddToDatabase(Batch);
            }
        }

        public static bool IsProductValid(IDocument document){
             var category = new List<string>() { " Книги с автографом ", " Художественная литература ", " Детские книги ", " Книги для подростков ", " Бизнес-литература ", " Самообразование и развитие ", " Хобби и досуг ", " Учебная литература ", " Педагогика и воспитание ", " Научно-популярная литература ", " Публицистика ", " Религия ", " Эксклюзивная продукция ", " Книги в кожаном переплете ", " Книжный развал ", " Букинистика и антикварные издания ", "" };
            if(category.Contains(document.GetElementsByClassName("breadcrumbs__link smartLink")[1].Children[0].TextContent))
                return true;
            return false; 
        }

        public static IDocument GetDocument(string url)
        {
            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);
            return context.OpenAsync(url).Result;
        }

        public static async Task<bool> IsPageFound(string page){
            var client = new HttpClient();
            var response = await client.GetAsync(page);
            if(response.StatusCode == System.Net.HttpStatusCode.NotFound){
                return false;
            } 
            return true; 
        }

        public static void WriteBatchToJson(List<Book> BookBatch){
            List<Book> ResultJson;
            string json = File.ReadAllText("book24.json");
            ResultJson = JsonConvert.DeserializeObject<List<Book>>(json) ?? new List<Book>();
            ResultJson.AddRange(BookBatch);
            json = JsonConvert
                .SerializeObject(ResultJson, new JsonSerializerSettings{Formatting = Formatting.Indented}); 
            File.WriteAllText("book24.json", json);
            Console.WriteLine("Батч добавлен в book24.json");
        }

        //public static void WriteToJson(Book BookInfo){
        //    List<Book> ResultJson;
        //    string json = File.ReadAllText("lyuteratura.json");
        //    ResultJson = JsonConvert.DeserializeObject<List<Book>>(json) ?? new List<Book>();
        //    ResultJson.Add(BookInfo);
        //    json = JsonConvert
        //        .SerializeObject(ResultJson, new JsonSerializerSettings{Formatting = Formatting.Indented}); 
        //    File.WriteAllText("lyuteratura.json", json);
        //     
        //}
        public static string getBookName(IDocument document){
            try{
                return document.GetElementsByClassName("product-detail-page__title")[0].TextContent;
            }
            catch(Exception ex){
                Console.WriteLine(ex.Message);
            }
            return "";
            
        }
        public static string  getVendor(IDocument document){
            try{
                return document.GetElementsByClassName("product-detail-page__article")[0].TextContent.Replace("Артикул: ", "").Replace(" ", "");
            }
            catch(Exception ex){
                Console.WriteLine(ex.Message);
            }
            return "";

        }

        public static int getPrice(IDocument document){
            try{
                return Int32.Parse(document.GetElementsByClassName("app-price product-sidebar-price__price")[0].TextContent.Replace("₽", "").Replace(" ", "").Replace(" ", ""));
            }
            catch(Exception ex){
                Console.WriteLine(ex.Message);
            }
            return 0;
        }

        public static string getDescription(IDocument document){
            try{
                return document.GetElementsByClassName("product-about__text")[0].TextContent
                    .Replace("\n", "");
            }
            catch(Exception ex){
                Console.WriteLine(ex.Message);
            }
            return "";
        }

        public static string getBreadcrumbs(IDocument document){
            try{
                return document.GetElementsByClassName("breadcrumbs__list")[0].TextContent.Replace("  ", "/");
            }
            catch(Exception ex){
                Console.WriteLine(ex.Message);
            }
            return "";
        }

        public static string getAuthor(IDocument document){
            return InfoFromTable(document, " Автор: ");
        }

        public static int  getPages(IDocument document){
            return Int32.Parse(InfoFromTable(document, " Количество страниц: "));
        }

        public static int  getYear(IDocument document){
            return Int32.Parse(InfoFromTable(document, " Год издания: "));
        }

        public static string  getISBN(IDocument document){
            return InfoFromTable(document, " ISBN: ");
        }

        public static string  getPublisher(IDocument document){
            return InfoFromTable(document, " Издательство: ");
        }

        public static string getGenre(IDocument document){
            return InfoFromTable(document, " Раздел: ");
        }

        public static string getImage(IDocument document){
            try{
                return document.GetElementsByClassName("product-poster__main-picture")[0].GetElementsByTagName("img")[0]
                   .Attributes["src"]
                   .Value
                   .Replace(" ", "")
                   .Replace("\t", "")
                   .Replace("\n", "");
            }
            catch(Exception ex){
                Console.WriteLine(ex.Message);
            }
            return "";
        }

        public static string InfoFromTable(IDocument document, string Field){
            try{
                var properties = document.GetElementsByClassName("product-characteristic__item").Where(p =>
                {
                    if (p.GetElementsByClassName("product-characteristic__label-holder").Length > 0)
                        return true;
                    return false;
                });
                foreach (var Prop in properties)
                {
                    var n = Prop.GetElementsByTagName("span")[0].TextContent;

                    var m = Prop.Children[1].TextContent.Replace("Издательство", "");
                    
                    if(n == Field){
                        return m;
                    }
                }
            }
            catch(Exception ex){
                Console.WriteLine(ex.Message);
            }
            return "";
        }
        public async Task AddToDatabase(List<Book> Batch){
            await _service.AddRangeAsync(Batch);
        }


    }
    public class Logger{
        private readonly string logFilePath;

        public Logger(string logFilePath)
        {
            this.logFilePath = logFilePath;
            // Create the log file if it does not exist
            if (!File.Exists(logFilePath))
            {
                using (StreamWriter streamWriter = File.CreateText(logFilePath))
                {
                    streamWriter.WriteLine($"{DateTime.UtcNow} : Log file created.");
                }
            }
        }

        public void Log(string message)
        {
            using (StreamWriter streamWriter = File.AppendText(logFilePath))
            {
                streamWriter.WriteLine($"{DateTime.UtcNow} Info: {message}");
            }
        } 
        public void Error(string message)
        {
            using (StreamWriter streamWriter = File.AppendText(logFilePath))
            {
                streamWriter.WriteLine($"{DateTime.UtcNow} Error: {message}");
            }
        } 
    }


}

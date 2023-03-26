using System.Diagnostics;
using System.Net;
using System.Text;
using AngleSharp;
using AngleSharp.Dom;
using Core.Database;
using ExtractorProject.Extractors.Models;
using InfrastructureProject;
using Newtonsoft.Json;

namespace LabirintExtractor;

public class ExtractorBooks{
    
    private readonly BookService _service;

    public ExtractorBooks(BookService service)
    {
        _service = service;
    }
    
    public static IDocument GetDocument(string url)
    {
        var config = Configuration.Default.WithDefaultLoader();
        var context = BrowsingContext.New(config);
        return context.OpenAsync(url).Result;
    }

    private List<Book> ParseBooksInfo(string url, int startId, int endId) 
    {
        List<Book> books = new List<Book>();
        for (int i = startId; i <= endId; i++)
        {
            var timer = Stopwatch.StartNew();
            try
            {
                string name = "";
                string author = "";
                string genre = "";
                string ISBN = "";
                string desc = "";
                string image = "";
                int numberOfPages = 0;
                int publishingYear = 0;
                string breadcrqmbs = "";
                var document = GetDocument(url + Convert.ToString(i));    
                string tag = "";
                try
                {
                    var documentUrl = document.Url;
                    if (!documentUrl.Contains("books") || document.StatusCode == HttpStatusCode.NotFound)//300025 
                        continue;
                    
                    //tag = document.QuerySelector("span.thermo-item").GetElementsByTagName("span")[0].TextContent;
                    //Console.WriteLine(tag);
                }
                catch (Exception e)
                {
                   // Console.WriteLine(e);
                }
                Book book = new Book();
                    book.SourceName = url + Convert.ToString(i);
                    try
                    {
                        name = document.QuerySelector("div.prodtitle").GetElementsByTagName("h1")[0].TextContent;
                        book.Name = name;
                    }
                    catch (Exception e)
                    {
                      //  Console.WriteLine(e);
                    }

                    try
                    {
                        author = document.QuerySelector("div.authors").GetElementsByTagName("a")[0].TextContent;
                        book.Author = author;
                    }
                    catch (Exception e)
                    {
                       // Console.WriteLine(e);
                    }

                    try
                    {
                        var divgenre = document.QuerySelector("div.genre");
                        if (divgenre != null)
                        {
                            var divgenre2 = divgenre.GetElementsByTagName("a")[0];
                            genre = divgenre2.TextContent;
                            book.Genre = genre;
                        }
                    }
                    catch (Exception e)
                    {
                       // Console.WriteLine(e);
                    }

                    try
                    {
                        ISBN = document.QuerySelector("div.isbn").TextContent.Replace("все", "").Replace("скрыть", "").Replace("ISBN: ", "");
                        //ISBN = document.QuerySelector("div.isbn").TextContent.ToString().Replace("ISBN: ", "");
                        book.ISBN = ISBN;
                    }
                    catch (Exception e)
                    {
                        //Console.WriteLine(e);
                    }

                    try
                    {
                        desc = document.GetElementById("product-about").GetElementsByTagName("p")[0].TextContent;
                        book.Description = desc;
                    }
                    catch (Exception e)
                    {
                       // Console.WriteLine(desc);
                    }
                    //
                    // try                 IMAGE ПРОГРУЖАЕТСЯ ЧУТЬ ПОЗЖЕ, НЕ ПОЛУЧИЛОСЬ ПОУЛУЧИТЬ
                    // {
                    //     // var divimage = document.GetElementById("product-image").Children[0];
                    //     // image = divimage.Attributes["src"].Value;
                    //     // Console.WriteLine(image);
                    // }
                    // catch (Exception e)
                    // {
                    //     Console.WriteLine(e);
                    // }

                    try
                    {
                        numberOfPages = Int32.Parse(document.QuerySelector("div.pages2")
                           .TextContent
                          .Replace("Страниц: ", "")
                          .Replace(" (Офсет)", "   ")
                          .Replace(" — прочитаете", "  ")
                          .Substring(0, 3)
                          .Trim());
                        book.NumberOfPages = numberOfPages; 
                    }
                    catch (Exception e)
                    {
                      //  Console.WriteLine(e);
                    }

                    try
                    {
                        var divpublyear = document.QuerySelector("div.publisher");
                        if (divpublyear != null) { 
                            string publisher = divpublyear.GetElementsByTagName("a")[0].TextContent;
                            publishingYear = Int32.Parse(divpublyear.TextContent
                                .Replace(publisher, "")
                                .Replace("Издательство: ", "")
                                .Replace(",", "")
                                .Replace(" г.", "")
                                .Trim());
                            book.PublisherYear = publishingYear;
                        }
                    }
                    catch (Exception e)
                    {
                        //Console.WriteLine(e);   
                    }

                    try
                    {
                        var tags = document.QuerySelectorAll("span.thermo-item");//GetElementsByTagName("span");
                        for (int tagn = 0; tagn < tags.Length; tagn++)
                        {
                            var qrmb = tags[tagn].TextContent.Replace("/", "");
                            if (tagn != tags.Length-1)
                                breadcrqmbs += qrmb + "/";
                            else
                                breadcrqmbs += qrmb;
                        }

                        book.Breadcrqmbs = breadcrqmbs;
                    }
                    catch (Exception e)
                    {
                        // Console.WriteLine(e);\
                    }
                    book.SiteBookId = i.ToString();
                    book.ParsingDate = DateTime.UtcNow;
                    books.Add(book);
                    timer.Stop();
                    //Console.WriteLine("Book was processed for "+timer.ElapsedMilliseconds+" ms");
                    if(books.Count%100 == 0)
                        Console.WriteLine($"Got {books.Count} books in thread since {startId} to {endId}");
            }
            catch (Exception ex) { 
                Console.WriteLine(ex); 
            }
        }
        return books;
    }

    private void WriteToJSON(string path, List<Book> books)
    {
        var json = JsonConvert.SerializeObject(books);
        File.WriteAllText(path, json, Encoding.UTF8);
        Console.WriteLine("Books writed to json, count " + books.Count);
        File.WriteAllText(path + "Count", Convert.ToString(books.Count));
    }

    private string GetValue(IHtmlCollection<IElement> collection)
    {
        if (collection.Length > 0)
            return collection[0].TextContent;
        return "";
    }

    private IElement GetElement(IHtmlCollection<IElement> collection)
    {
        if (collection.Length > 0)
            return collection[0];
        return null;
    }

    public async Task Parse((string URL, int startId, int endId) valueTuple)
    {
        List<Book> books = new List<Book>();
        try
        {
            var timer = Stopwatch.StartNew();
            //Console.WriteLine($"Task since {valueTuple.startId} to {valueTuple.endId} was started");
            books = ParseBooksInfo("https://www.labirint.ru/books/", valueTuple.startId, valueTuple.endId);
            //WriteToJSON($"Labirint-{valueTuple.startId}To{valueTuple.endId}_{DateTime.Now.ToShortDateString()}.json",books);
            await AddToDatabase(books);
            timer.Stop();

            Console.WriteLine($"------------------------------------------------\n" +
                              $"Task since {valueTuple.startId} to {valueTuple.endId} was finished for {timer.ElapsedMilliseconds / 1000} seconds\n" +
                              $"Parsed {books.Count} books\n" +
                              $"-------------------------------------------------");
        }
        catch (Exception ex)
        {
            string path = $"Labirint-{valueTuple.startId}To{valueTuple.endId}_{DateTime.Now.ToShortDateString()}.json";
            WriteToJSON(path,books);
            Console.WriteLine($"------------------------------------------------\n" +
                              $"Task since {valueTuple.startId} to {valueTuple.endId} was finished with error: {ex.Message}\n" +
                              $"Parsed {books.Count} books\n" +
                              $"Books was saved to {path}" +
                              $"-------------------------------------------------");
        }

    }

    public async Task AddToDatabase(List<Book> batch)
    {
        
        await _service.AddRangeAsync(batch);
    }
}

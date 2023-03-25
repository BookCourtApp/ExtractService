using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Threading.Channels;
using AngleSharp;
using AngleSharp.Dom;
using BookParser;
using Newtonsoft.Json;
using AngleSharp.Html.Parser;
using System.Linq.Expressions;

namespace Parser;
public class ExtractorBooks{

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
                var document = GetDocument(url + Convert.ToString(i));    
                string tag = "";
                try
                {
                    tag = document.QuerySelector("span.thermo-item").GetElementsByTagName("span")[0].TextContent;
                    //Console.WriteLine(tag);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                if (tag.Contains("Книги"))
                {
                    Book book = new Book();
                    book.SourceName = url + Convert.ToString(i);
                    try
                    {
                        name = document.QuerySelector("div.prodtitle").GetElementsByTagName("h1")[0].TextContent;
                        book.Name = name;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                    try
                    {
                        author = document.QuerySelector("div.authors").GetElementsByTagName("a")[0].TextContent;
                        book.Author = author;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
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
                        Console.WriteLine(e);
                    }

                    try
                    {
                        ISBN = document.QuerySelector("div.isbn").TextContent.ToString().Replace("ISBN: ", "");
                        book.ISBN = ISBN;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                    try
                    {
                        desc = document.GetElementById("product-about").GetElementsByTagName("p")[0].TextContent;
                        book.Description = desc;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(desc);
                    }

                    //try
                    //{
                    //    var divimage = document.GetElementsByClassName("book-img-cover")[0];
                    //    image = divimage.Attributes["src"].Value;
                    //    Console.WriteLine(image);
                    //}
                    //catch (Exception e)
                    //{
                    //    Console.WriteLine(e);
                    //}
                    // Возвращает пустую обложку

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
                        Console.WriteLine(e);
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
                            Console.WriteLine(publishingYear);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);   
                    }

                    book.LabirintBookId = i;
                    book.ParsingDate = DateTime.UtcNow;
                    books.Add(book);
                }
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

    public void Parse(string URL, int startPage, int endPage)
    {
        List<Book>books = ParseBooksInfo("https://www.labirint.ru/books/", 848248, 848250);
        WriteToJSON("Labirint_demochka.json",books);
    }

}

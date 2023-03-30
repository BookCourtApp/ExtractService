using AngleSharp;
using AngleSharp.Dom;
using Core.Extractor;
using Core.Models;
using System.Net;

namespace ExtractorProject.Extractors
{
    public class ExtractorLabirint : BookExtractor
    {
        public override IDocument GetRawData(ResourceInfo info)
        {
            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);
            if (context.OpenAsync(info.URLResource).Result.Url.Contains("books") && context.OpenAsync(info.URLResource).Result.StatusCode != HttpStatusCode.NotFound)
            {
                return context.OpenAsync(info.URLResource).Result;
            }
            else return null;
        }

        public override Book Handle(IDocument rawData)
        {            
        // Если ссылка невалидна, то вовзращает null
            if (rawData != null)
            {
                var document = rawData;
                Book book = new Book()
                {
                    ParsingDate = DateTime.UtcNow,
                    SourceName = rawData.Url
                };
                try
                {
                    var name = document.QuerySelector("div.prodtitle").GetElementsByTagName("h1")[0].TextContent.Split(":").Last();
                    book.Name = name;
                }
                catch (Exception e)
                {
                    //  Console.WriteLine(e);
                }

                try
                {
                    var author = document.QuerySelector("div.authors").GetElementsByTagName("a")[0].TextContent;
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
                        var genre = divgenre2.TextContent;
                        book.Genre = genre;
                    }
                }
                catch (Exception e)
                {
                    // Console.WriteLine(e);
                }

                try
                {
                    var ISBN = document.QuerySelector("div.isbn").TextContent.Replace("все", "").Replace("скрыть", "").Replace("ISBN: ", "");

                    book.ISBN = ISBN;
                }
                catch (Exception e)
                {
                    //Console.WriteLine(e);
                }

                try
                {
                    var desc = document.GetElementById("product-about").GetElementsByTagName("p")[0].TextContent;
                    book.Description = desc;
                }
                catch (Exception e)
                {
                    // Console.WriteLine(desc);
                }

                try
                {
                    var numberOfPages = Int32.Parse(document.QuerySelector("div.pages2")
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
                    var price = Int32.Parse(document.QuerySelector("span.buying-pricenew-val-number").TextContent);
                    book.Price = price;
                }
                catch (Exception e)
                {
                    //  Console.WriteLine(e);
                }
                try
                {
                    var divpublyear = document.QuerySelector("div.publisher");
                    if (divpublyear != null)
                    {
                        string publisher = divpublyear.GetElementsByTagName("a")[0].TextContent;
                        var publishingYear = Int32.Parse(divpublyear.TextContent
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
                book.ParsingDate = DateTime.UtcNow;
                return book;
            }
            return null;

        }
    }
}


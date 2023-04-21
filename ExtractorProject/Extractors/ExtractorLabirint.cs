using AngleSharp;
using AngleSharp.Dom;
using Core.Extractor;
using Core.Models;
using System.Net;

namespace ExtractorProject.Extractors
{
    /// <inheritdoc/>
    /// <summary>
    /// Класс для парсинга сайта https://www.labirint.ru/
    /// </summary>
    public class ExtractorLabirint : IExtractor<IDocument, Book>
    {
        /// <inheritdoc/>
        public async Task<IDocument> GetRawDataAsync(ResourceInfo info)
        {
            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);
            var page = await context.OpenAsync(info.URLResource);
            if (page.Url.Contains("books") && page.StatusCode != HttpStatusCode.NotFound)
            {
                return page;
            }
            else
                return null;
        }

        /// <inheritdoc/>
        public async Task<Book> HandleAsync(IDocument rawData)
        {
            if (rawData == null)
            {
                return null;
            }
            else
            {
                var document = rawData;
                Book book = new Book()
                {
                    ParsingDate = DateTime.UtcNow,
                    SourceName = rawData.Url,
                    SourceUrl = "https://www.labirint.ru/"
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
                    var tags = document.QuerySelectorAll("span.thermo-item");//GetElementsByTagName("span");
                    string breadcrumbs = "";
                    for (int tagn = 0; tagn < tags.Length; tagn++)
                    {
                        var qrmb = tags[tagn].TextContent.Replace("/", "");
                        if (tagn != tags.Length - 1)
                            breadcrumbs += qrmb + "/";
                        else
                            breadcrumbs += qrmb;
                    }

                    book.Breadcrumbs = breadcrumbs;
                }
                catch (Exception e)
                {
                    // Console.WriteLine(e);\
                }

                try
                {
                    var numberOfPages = Int32.Parse(document.QuerySelector("div.pages2")
                       .TextContent
                      .Replace("Страниц : ", "")
                      .Replace(" (Офсет)", "   ")
                      .Replace(" - прочитаете", "  ")
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

                try
                {
                    var divarticul = document.QuerySelector("div.articul").TextContent.Replace("ID товара: ", "");
                    book.SiteBookId = divarticul;
                }
                catch (Exception e)
                {
                    //Console.WriteLine(e); 
                }
                return book;
            }

        }
    }
}
using AngleSharp;
using AngleSharp.Dom;
using Core.Extractor;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorProject.Extractors
{
    public class ExtractorVault14 : IExtractor<IDocument, Book>
    {
        public IDocument GetRawData(ResourceInfo info)
        {
            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);
            var page = context.OpenAsync(info.URLResource).Result;
            if (page.StatusCode != HttpStatusCode.NotFound)
            {
                return page;
            }
            else
                return null;
        }

        public Book Handle(IDocument rawData)
        {
            if(rawData == null)
            {
                return null;
            }
            var document = rawData;
            Book book = new Book()
            {
                ParsingDate = DateTime.UtcNow,
                SourceName = rawData.Url,
                SourceUrl = "https://vault14.ru/"
            };
            try
            {
                var name = document.QuerySelector("h1.page-headding").TextContent;
                book.Name = name;
            }
            catch ( Exception e ) 
            { 
               // Console.WriteLine( e );
            }

            try
            {
                var descripton = document.QuerySelector("div.tab-block-inner editor").TextContent;
                book.Description = descripton;
            }
            catch (Exception e)
            {
                // Console.WriteLine( e );
            }

            try
            {
                var image = document.QuerySelector("a.slide-inner image-container").TextContent;
                book.Image = image;
            }
            catch (Exception e)
            {
                // Console.WriteLine( e );
            }

            try
            {
                var genre = document.QuerySelector("a.breadcrumb-link").TextContent;
                book.Genre = genre;
            }
            catch (Exception e)
            {
                // Console.WriteLine( e );
            }

            try
            {
                var siteBookIdTag = document.GetElementsByTagName("meta")[0].Attributes["data-config"].Value;
                var siteBookId = siteBookIdTag.Replace("{\"product_id\":", "").Replace("}", "");
                book.SiteBookId = siteBookId;
            }
            catch (Exception e)
            {
                // Console.WriteLine( e );
            }

            try
            {
                IHtmlCollection<IElement>tds = document.QuerySelector("table.table table-bordered table-striped table-hover").GetElementsByTagName("td");
                for(int i = 0;i < tds.Length; i++)
                {
                    if (tds[i].TextContent.Equals("Автор/Сценарист:"))
                    {
                        book.Author = tds[i + 1].TextContent;
                    }
                    if (tds[i].TextContent.Equals("Страниц:"))
                    {
                        int numberOfPages = Int32.Parse(tds[i + 1].TextContent.Trim().Replace(" ", ""));
                        book.NumberOfPages = numberOfPages;
                    }
                }
            }
            catch (Exception e)
            {
                // Console.WriteLine( e );
            }


            return book;
        }
    }
}

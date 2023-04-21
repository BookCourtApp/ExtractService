using AngleSharp;
using AngleSharp.Dom;
using Core.Extractor;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorProject.Extractors
{
    /// <summary>
    /// Класс для парсинга Vault14
    /// </summary>
    public class ExtractorVault14 : IExtractor<IDocument, Book>
    {
        /// <inheritdoc/>
        public async Task<IDocument> GetRawDataAsync(ResourceInfo info)
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

        /// <inheritdoc/>
        public async Task<Book> HandleAsync(IDocument rawData)
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
                var descripton = document.GetElementsByClassName("tab-block-inner editor")[0].TextContent;
                book.Description = descripton;
            }
            catch (Exception e)
            {
                // Console.WriteLine( e );
            }

            try
            {
                var image = document.GetElementsByClassName("slide-image")[0].Attributes["src"].Value;
                book.Image = image;
            }
            catch (Exception e)
            {
                // Console.WriteLine( e );
            }

            try
            {
                var genre = document.QuerySelector("a[class = breadcrumb-link]").TextContent;
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
                IHtmlCollection<IElement>tds = document.GetElementsByClassName("table table-bordered table-striped table-hover")[0].GetElementsByTagName("td");
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

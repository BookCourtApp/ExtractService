using AngleSharp;
using AngleSharp.Dom;
using Core.Extractor;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ExtractorProject.Extractors
{
    /// <inheritdoc/>
    /// <summary>
    /// Класс для парсинга сайта https://igraslov.store/
    /// </summary>
    public class ExtractorIgraSlov : IExtractor<IDocument, Book>
    {
        /// <inheritdoc/>
        public IDocument GetRawData(ResourceInfo info)
        {
            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);
            var page = context.OpenAsync(info.URLResource).Result;
            return page;
        }

        /// <inheritdoc/>
        public Book Handle(IDocument rawData)
        {
            if (rawData == null)
            {
                return null;
            }
            var document = rawData;
            Book book = new Book()
            {
                ParsingDate = DateTime.UtcNow,
                SourceName = rawData.Url,
                SourceUrl = "https://igraslov.store/"
            };
            try
            {
     

                var BoookName = document.GetElementsByClassName("single-post-title product_title entry-title")[0].TextContent;
                book.Name = BoookName;

                var Description = document.GetElementsByClassName("woocommerce-product-details__short-description")[0].TextContent
                    .Replace("\t", "")
                    .Replace("\n", ""); ;
                book.Description = Description;

                var Genre = document.GetElementsByClassName("posted_in")[0].GetElementsByTagName("a")[0].TextContent
                    .Replace("\t", "")
                    .Replace("\n", "");
                book.Genre = Genre;

                var Image = document.GetElementsByClassName("woocommerce-product-gallery__wrapper")[0].GetElementsByTagName("img")[0].Attributes["src"]
                    .Value
                    .Replace(" ", "")
                    .Replace("\t", "")
                    .Replace("\n", ""); ;
                book.Image = Image;

                var properties = document.GetElementsByClassName("woocommerce-product-attributes shop_attributes")[0].GetElementsByTagName("tr");
                foreach (var i in properties)
                {
                    var label = i.GetElementsByTagName("th")[0].TextContent;
                    var value = i.GetElementsByTagName("td")[0].TextContent
                        .Replace("\t", "")
                        .Replace("\n", "");
                    switch (label.ToLower())
                    {
                        case "автор":
                            book.Author = value;
                            break;
                        case "isbn/issn":
                            book.ISBN = value;
                            break;
                        case "кол-во страниц":
                            book.NumberOfPages = int.Parse(value);
                            break;

                    }
                }

            }
            catch (Exception ex)
            {
                //     Console.WriteLine($"{DateTime.Now} : ERROR for book - {ex.Message}");
            }
            return book;



        }
    }
}

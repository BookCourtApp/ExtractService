using System.Net;
using AngleSharp;
using AngleSharp.Dom;
using Core.Extractor;
using Core.Models;
using Newtonsoft.Json.Linq;

namespace ExtractorProject.Extractors;

/// <summary>
/// Экстрактор PrimBook
/// </summary>
public class ExtractorPrimbook : IExtractor<IDocument, Book>
{
    /// <inheritdoc />
    public IDocument GetRawData(ResourceInfo resource)
    {
        var config = Configuration.Default.WithDefaultLoader();
        var context = BrowsingContext.New(config);
        var page = context.OpenAsync(resource.URLResource).Result;
        return page;
    }

    /// <inheritdoc />
    public Book Handle(IDocument data)
    {
            string refToBook = data.Url;
            string BoookName = "";
            string Description = "";
            string Author = null;
            string Genre = null;
            string Image = "";
            int NumberPages = 0;
            string ISBN = "";
            string PublisherName = "";
            string SiteId = "";
            
            string BreadCrumbs = "";
            try
            {
                BoookName = data.GetElementsByClassName("intec-header")[0].TextContent.Replace("\t", "").Replace("\n", "").Replace("  ", "");

                SiteId = data.Url.Split("/", StringSplitOptions.RemoveEmptyEntries).Last();

                var breadCrumbsItems = data.GetElementsByClassName("breadcrumb-item");
                foreach (var crumbs in breadCrumbsItems)
                {
                    BreadCrumbs += crumbs.TextContent.Replace("\t", "").Replace("\n", "").Replace("  ", "") + "\\";
                }

                Image = "https://primbook.ru" +
                        data.GetElementsByClassName("catalog-element-gallery-picture intec-image")[0]
                            .Attributes["href"]
                            .Value
                            .Replace(" ", "")
                            .Replace("\t", "")
                            .Replace("\n", "");
                Description =
                    data.GetElementsByClassName("catalog-element-section-description intec-ui-markup-text")[0]
                        .TextContent
                        .Replace("\t", "")
                        .Replace("\n", "");
                

            }
            catch (Exception ex)
            {
                //Console.WriteLine($"{DateTime.Now} : ERROR for book - {ex.Message}");
            }

            if (string.IsNullOrEmpty(refToBook))
                return null;
            Book book = new Book()
            {
                Author = Author,
                Description = Description,
                Genre = Genre,
                Image = Image,
                Name = BoookName,
                NumberOfPages = NumberPages,
                ISBN = ISBN,
                Breadcrumbs = BreadCrumbs,
                ParsingDate = DateTime.UtcNow,
                SiteBookId = SiteId
            };

            book.SourceName = refToBook;
            book.SourceUrl = "https://primbook.ru";
            return book;
    }
}
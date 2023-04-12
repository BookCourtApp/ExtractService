using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Io;
using Core.Extractor;
using Core.Models;
using System.Net;


namespace ExtractorProject.Extractors;

/// <summary>
/// Экстрактор для Book24
/// </summary>
public class ExtractorBook24
{
    /// <inheritdoc />
    public IDocument? GetRawData(ResourceInfo resource)
    {
        try
        {
            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);
            var AngleSharpUrl = new Url(resource.URLResource);
            var request = DocumentRequest.Get(AngleSharpUrl);

            request.Headers["User-Agent"] = "Mozilla/4.0 (compatible; MSIE 7.0; AOL 9.1; AOLBuild 4334.27; Windows NT 6.0; WOW64; SLCC1; .NET CLR 2.0.50727; .NET CLR 3.0.04506; Media Center PC 5.0); UnAuth-State";
            request.Headers["Accept"] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            request.Headers["Accept-Language"] = "en-US,en;q=0.9";
            request.Headers["Referer"] = "http://google.com";
            request.Headers["Connection"] = "keep-alive";
            return context.OpenAsync(request).Result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while getting document: {ex.Message}");
        }
        return null;
    }
    /// <inheritdoc />
    public Book? Handle(IDocument RawData)
    {

        if (!IsProductValid(RawData))
            return null;

        try
        {
            Book BookInfo = new Book
            {
                Name            = getBookName(RawData),
                Author          = getAuthor(RawData),
                Genre           = getGenre(RawData),
                Description     = getDescription(RawData),
                NumberOfPages   = getPages(RawData),
                SourceName      = RawData.Url,
                Image           = getImage(RawData),
                ParsingDate     = DateTime.UtcNow,
                ISBN            = getISBN(RawData),
                PublisherYear   = getYear(RawData),
                Breadcrumbs     = getBreadcrumbs(RawData),
                SiteBookId      = GetVendor(RawData),
                SourceUrl       = "https://book24.ru"
            };
            return BookInfo;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при парсинге полей книги: " + ex.Message);
        }
        return null;
    }

    public bool IsProductValid(IDocument document)
    {
        var category = new List<string>() { " Книги с автографом ", " Художественная литература ", " Детские книги ", " Книги для подростков ", " Бизнес-литература ", " Самообразование и развитие ", " Хобби и досуг ", " Учебная литература ", " Педагогика и воспитание ", " Научно-популярная литература ", " Публицистика ", " Религия ", " Эксклюзивная продукция ", " Книги в кожаном переплете ", " Книжный развал ", " Букинистика и антикварные издания ", "" };
        if (category.Contains(document.GetElementsByClassName("breadcrumbs__link smartLink")[1].Children[0].TextContent))
            return true;
        return false;
    }

    public static async Task<bool> IsPageFound(string page)
    {
        var client = new HttpClient();
        var response = await client.GetAsync(page);
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return false;
        }
        return true;
    }

    public static string getBookName(IDocument document)
    {
        try
        {
            return document.GetElementsByClassName("product-detail-page__title")[0].TextContent;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return "";

    }
    public static string GetVendor(IDocument document)
    {
        try
        {
            return document.GetElementsByClassName("product-detail-page__article")[0].TextContent.Replace("Артикул: ", "").Replace(" ", "");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return "";

    }

    public static int getPrice(IDocument document)
    {
        try
        {
            return Int32.Parse(document.GetElementsByClassName("app-price product-sidebar-price__price")[0].TextContent.Replace("₽", "").Replace(" ", "").Replace(" ", ""));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return 0;
    }

    public static string getDescription(IDocument document)
    {
        try
        {
            return document.GetElementsByClassName("product-about__text")[0].TextContent
                .Replace("\n", "");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return "";
    }

    public static string getBreadcrumbs(IDocument document)
    {
        try
        {
            return document.GetElementsByClassName("breadcrumbs__list")[0].TextContent.Replace("  ", "/");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return "";
    }

    public static string getAuthor(IDocument document)
    {
        return InfoFromTable(document, " Автор: ");
    }

    public static int getPages(IDocument document)
    {
        return Int32.Parse(InfoFromTable(document, " Количество страниц: "));
    }

    public static int getYear(IDocument document)
    {
        return Int32.Parse(InfoFromTable(document, " Год издания: "));
    }

    public static string getISBN(IDocument document)
    {
        return InfoFromTable(document, " ISBN: ");
    }

    public static string getPublisher(IDocument document)
    {
        return InfoFromTable(document, " Издательство: ");
    }

    public static string getGenre(IDocument document)
    {
        return InfoFromTable(document, " Раздел: ");
    }

    public static string getImage(IDocument document)
    {
        try
        {
            return document.GetElementsByClassName("product-poster__main-picture")[0].GetElementsByTagName("img")[0]
               .Attributes["src"]
               .Value
               .Replace(" ", "")
               .Replace("\t", "")
               .Replace("\n", "");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return "";
    }

    public static string InfoFromTable(IDocument document, string Field)
    {
        try
        {
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

                if (n == Field)
                {
                    return m;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return "";
    }


}

using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Dom.Events;
using AngleSharp.Html.Parser;
using AngleSharp.Io;
using Core.Extractor;
using Core.Models;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection.PortableExecutable;
using System.Reflection;
using System.Text;
using System.Globalization;

namespace ExtractorProject.Extractors;

/// <summary>
/// Класс для парсинга LiveLib
/// </summary>
public class ExtractorLiveLib : IExtractor<IDocument, Book>
{
    ///<inheritdoc/>
    public async Task<IDocument> GetRawDataAsync(ResourceInfo resourceInfo)
    {
        HttpClient hc = new HttpClient();
        string headers = "'Host': 'www.livelib.ru', \n" +
            "'Sec-Ch-Ua-Mobile': '?0', \n" +
            "'Sec-Ch-Ua-Platformr': '\"Windows\"', \n " +
            "  'Upgrade-Insecure-Requests': '1', \n" +
            "'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.5615.50 Safari/537.36', \n " +
            "  'Accept': 'text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7', \n" +
            "'Sec-Fetch-Site': 'none',  \n" +
            "  'Sec-Fetch-Mode': 'navigate', \n" +
            "'Sec-Fetch-User': '?1',  \n " +
            " 'Sec-Fetch-Dest': 'document', \n" +
            "'Accept-Encoding': 'gzip, deflate',  \n" +
            "  'Accept-Language': 'ru-RU,ru;q=0.9,en-US;q=0.8,en;q= 0.7', \n" +
            "'X-Requested-With': 'XMLHttpRequest' \n";
        var headerscol = headers.Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(header => header.Split(':', StringSplitOptions.RemoveEmptyEntries));
        foreach (var strings in headerscol)
        {
            hc.DefaultRequestHeaders.TryAddWithoutValidation(strings[0], strings[1]);
        }
        hc.DefaultRequestHeaders.TryAddWithoutValidation("Sec-Ch-Ua:" , "\"Not:A-Brand\"; v = \"99\", \"Chromium\"; v = \"112\"  \n");
        hc.DefaultRequestHeaders.Add("Cookie",
       "__ll_tum=3199867700; __ll_ab_mp=1; tmr_lvid=cfec893d0e7bfe44acef9aa2191f6de9; tmr_lvidTS=1681617845503; _ga=GA1.2.1071574945.1681617846; _ym_uid=1681617847426953912; _ym_d=1681617847; __llutmz=-600; __ll_fv=1681617920; __popupmail_showed=1000; __popupmail_showed_uc=1; __ll_popup_count_shows=regc1_mailc1_; __gr=g392c1_g387c1_g510c1_g149c1_g1251c1_g1240c1_; __ll_unreg_session=faabdca5c8a8620777133fc5f09195f1; __ll_unreg_sessions_count=5; llsid=62301122102e5e88d574058fb24a433c; __utnx=12006965305; __llutmf=1; __ll_popup_count_pviews=regc1_challengec1_; _gid=GA1.2.366892746.1681810244; _ym_isad=2; iwatchyou=27784c29f78fe4bc8583452f6ea403d9; __ll_cp=16; tmr_detect=0%7C1681811878859; __ll_dv=1681815282;_GRECAPTCHA=09ALnTWt7JHSwrKwSKdW9r_r31F9BIrVHF8QA7KPAT_JeOQFW-RlAhxhcIdRJ224zRYnnh5TQaWNq_F_ft6Vp8aMk");
        var uri = new Uri(resourceInfo.URLResource);
        var responce = hc.PostAsync(uri, null);
        var html = responce.Result.Content.ReadAsStringAsync().Result;
        var parser = new HtmlParser();

        return parser.ParseDocument(html);
    }

    ///<inheritdoc/>
    public async Task<Book> HandleAsync(IDocument rawData)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        if (rawData == null)
        {
            return null;
        }
        var document = rawData;
        Book book = new Book()
        {
            ParsingDate = DateTime.UtcNow,
            SourceName = rawData.Url,
            SourceUrl = "https://www.livelib.ru/"
        };

        try
        {
            var name = document.GetElementsByClassName("bc__book-title ")[0].TextContent;
            book.Name = name;
        }
        catch (Exception e)
        {
            //Console.WriteLine(e);
        }

        try
        {
            var author = document.GetElementsByClassName("bc-author__link")[0].TextContent;
            book.Author = author;
        }
        catch (Exception e)
        {
            //Console.WriteLine(e);
        }

        try
        {
            IHtmlCollection<IElement>divdesc = document.GetElementsByClassName("bc-annotate without-readmore")[0].GetElementsByTagName("div");
            book.Description = divdesc[0].TextContent.Trim();
        }
        catch (Exception e)
        {
           //Console.WriteLine(e);
        }

        try
        {
            var image = document.GetElementsByClassName("lenta-card-book__img")[0].Attributes["src"].Value;
            book.Image = image;
        }
        catch (Exception e)
        {
            //Console.WriteLine(e);
        }
        try
        {
            var divnum = document.GetElementsByClassName("bc-info__wrapper")[1];
            IHtmlCollection<IElement> ps = divnum.GetElementsByTagName("p");
            for (int i = 0; i < ps.Length; i++)
            {
                try
                {
                    if (ps[i].TextContent.Contains("ISBN: "))
                    {
                        book.ISBN = ps[i].TextContent.Replace("ISBN: ", "");
                    }
                }
                catch (Exception e)
                {
                    //Console.WriteLine(e);
                }

                try {
                    if (ps[i].TextContent.Contains("Год издания: "))
                    {
                        book.PublisherYear = Int32.Parse(ps[i].TextContent.Replace("Год издания: ", ""));
                    }
                }
                catch (FormatException e)
                {
                    //Console.WriteLine(e);
                }

                try
                {
                    if (ps[i].TextContent.Contains("Количество страниц: "))
                    {
                        book.NumberOfPages = Int32.Parse(ps[i].TextContent.Replace("Количество страниц: ", "").Replace(" (офсет)", ""));
                    }
                }
                catch (Exception e)
                {
                    //Console.WriteLine(e);
                }

                try
                {
                    if (ps[i].TextContent.Contains("Язык: "))
                    {
                        book.Language = ps[i].TextContent.Replace("Язык: ", "").Trim();
                    }
                }
                catch (Exception e)
                {
                    //Console.WriteLine(e);
                }

                try
                {
                    if (ps[i].TextContent.Contains("Возрастные ограничения:"))
                    {
                        string temp = ps[i].TextContent.Replace("Возрастные ограничения: ", "").Replace("+","");
                        book.Age = Int32.Parse(temp);
                    }
                }
                catch (Exception e)
                {
                    //Console.WriteLine(e);
                }
                try
                {
                    if (ps[i].TextContent.Contains("Тираж: "))
                    {
                        int startIndex = ps[i].TextContent.IndexOf("Тираж: ");
                        int endIndex = ps[i].TextContent.IndexOf(" экз.");
                        book.Circulations = Convert.ToInt32(ps[i].TextContent.Substring(startIndex, endIndex - startIndex).Replace("Тираж: ", ""));

                    }
                }
                catch(Exception e)
                {
                    //Console.WriteLine(e);
                }
            }
        }
        catch (Exception e)
        {
            //Console.WriteLine(e);
        }

        try
        {
            var publs = document.GetElementsByClassName("bc-edition__link");
            string publisher = "";
            foreach(var publ in publs)
            {
                publisher += (publ.TextContent+";");
            }
            book.Publisher = publisher;
        }
        catch (Exception e)
        {
            //Console.WriteLine(e);
        }

        try
        {
            var divgen = document.GetElementsByClassName("bc-info__wrapper")[2];
            IHtmlCollection<IElement> ps = divgen.GetElementsByTagName("p");
            var pgenres = ps[0];
            var ptags = ps[1];
            IHtmlCollection<IElement> agenres = ps[0].GetElementsByTagName("a");

            string genres = "";
            for(int i = 0;i < agenres.Length;i++)
            {

                genres += (agenres[i].TextContent + ";");

            }
            string tags = "";
            IHtmlCollection<IElement> atags = ps[1].GetElementsByTagName("a");
            for(int i = 0;i < atags.Length-2; i++)
            {

                tags += (atags[i].TextContent + ";");

            }
            book.Genre = genres;        
            book.Tags = tags;

        }
        catch (Exception e)
        {
            //Console.WriteLine(e);
        }

        try
        {
            var siteBookId = document.GetElementsByClassName("read-more__link")[0].Attributes["data-object_id"].Value;
            book.SiteBookId = siteBookId;
        }
        catch (Exception e)
        {
            //Console.WriteLine(e);
        }

        try
        {
            var rating = document.GetElementsByClassName("bc-rating-medium")[0]
                .GetElementsByTagName("span")[0]
                .TextContent.Replace(",", ".")
                .Trim();
            book.Rating = (Double)Convert.ToDouble(rating);
        }
        catch (Exception e)
        {
            //Console.WriteLine(e);
        }

        try
        {
            var metainf = document.GetElementsByClassName("bc-stat")[0];
            IHtmlCollection<IElement> stats = metainf.GetElementsByClassName("bc-stat__link");
            foreach (IElement element in stats)
            {   
                if(element.TextContent.Contains("рецензий"))
                {
                    try
                    {
                        book.Reviews = Int32.Parse(element.TextContent.Replace("рецензий", "").Trim());
                    }
                    catch (Exception e)
                    {
                        //Console.WriteLine(e);
                    }
                }
                if (element.TextContent.Contains("цитат"))
                {
                    try
                    {
                        book.Quotes = Int32.Parse(element.TextContent.Replace("цитат", "").Trim());
                    }
                    catch (Exception e)
                    {
                        //Console.WriteLine(e);
                    }
                }
                if (element.TextContent.Contains("планируют"))
                {
                    try
                    {
                        book.Planning = Int32.Parse(element.TextContent.Replace("планируют", "").Trim());
                    }
                    catch (Exception e)
                    {
                        //Console.WriteLine(e);
                    }
                }
                if (element.TextContent.Contains("прочитали"))
                {
                    try
                    {
                        book.Reading = Int32.Parse(element.TextContent.Replace("прочитали", "").Trim());
                    }
                    catch (Exception e)
                    {
                        //Console.WriteLine(e);
                    }
                }
            }
        }
        catch (Exception e)
        {
            //Console.WriteLine(e);
        }

        return book;
    }
}

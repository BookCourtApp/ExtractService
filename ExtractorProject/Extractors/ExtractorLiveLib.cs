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
       "__ll_ab_mp=1; __ll_tum=3664026419; __llutmz=-600; __ll_fv=1681482795; __popupmail_showed=1000; __popupmail_showed_uc=1; llsid=9774898e440a647fec60edb78356b077; __utnx=12000205153; __llutmf=1; __ll_popup_count_pviews=regc1_challengec1_; __ll_popup_count_shows=regc1_mailc1_challenge_new2c1_; _ga=GA1.2.1016390543.1681524561; _ym_uid=1681524561639602016; _ym_d=1681524561; tmr_lvid=1832e426c0ca39c235988c92c05df500; tmr_lvidTS=1681525060884; LiveLibId=d170fcde8bc292ff99743ebdfff5b49d; __utnt=g0_y0_a15721_u0_c0; __ll_unreg_session=d170fcde8bc292ff99743ebdfff5b49d; __ll_unreg_sessions_count=4; __ll_dvs=1; ll_asid=1249188717; __ll_popup_showed=reg_; __ll_popup_last_show=1681808719; __ll_unreg_r=60; _gid=GA1.2.1819667972.1681811878; iwatchyou=521552be61ccf985dae6379b16b8df97; __gr=g1c9_g1222c2_g1217c3_g433c1_; tmr_detect=0%7C1681811982427; _gat=1; _ym_isad=1; _ym_visorc=w; __ll_cp=42; __ll_dv=1681897648;_GRECAPTCHA=09AMqPRJwCrCJqa1Dz9seWcvLVRxRuIWHft81RkeBiChzgi5USgczb22xEMPuihJr_zHPHA9InPKUypdM6h9g3Gvs; 1P_JAR=2023-04-16-12; NID=511=Ma22WXo7e_5MrejpmvCQQTO-e-6bPzeNVotx2fI9Comd4URzmswyuos8EnC8rCVjndO7F1v58f1OVxmz1VGQsL-FB095NQvwRclrIMrwLf7HeMnZtw4w6j_ONTust--veO1v8_GrQFj5XByZ8Vm8EVoKbc_TlZQRkY8JrmH4vi4");
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
            //SourceName = rawData.Url,
            SourceUrl = "https://www.livelib.ru/"
        };

        try
        {
            var link = document.GetElementsByClassName("footer-livelib__item").Last().GetElementsByTagName("a")[0].Attributes["href"].Value;
            book.SourceName = link; 
            //Console.WriteLine(book.SourceName);
        }
        catch (Exception e)
        {
            //Console.WriteLine(e);
        }

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

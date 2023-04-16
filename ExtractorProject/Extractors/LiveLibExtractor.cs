using System.Net;
using System.Text;
using System.Web;
using AngleSharp;
using AngleSharp.Common;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using AngleSharp.Io;
using Core.Extractor;
using Core.Models;

namespace ExtractorProject.Extractors;

public class LiveLibExtractor //: IExtractor<IDocument, string>
{
    public async Task<IDocument> GetRawDataAsync(ResourceInfo resource)
    {
        
         var url = new Url(resource.URLResource);  // URLResource = https://www.livelib.ru/genre
         string CookieStr =
             "LiveLibId=8e68775b8abd7b7904d0b76711cd3913; __ll_ab_mp=1; __ll_tum=3664026419; __ll_unreg_session=8e68775b8abd7b7904d0b76711cd3913; __ll_unreg_sessions_count=1; __llutmz=-600; __ll_fv=1681482795; __ll_dvs=5; ll_asid=1245240020; iwatchyou=5df35936713e92b51205077989eee0cc; __popupmail_showed=1000; __popupmail_showed_uc=1; __popupmail_showed_t=1000; __gr=g1c1_; __ll_google_oauth=; __ll_google_code=; __utrx=1; llsid=9774898e440a647fec60edb78356b077; __utnx=12000205153; __llutmf=1; __utnt=g0_y0_a15721_u12000205153_c0; __ll_popup_count_pviews=regc1_challengec1_; __ll_popup_showed=reg_mail_challenge_new2_; __ll_popup_last_show=1681520939; __ll_popup_count_shows=regc1_mailc1_challenge_new2c1_; _ga=GA1.2.1016390543.1681524561; _gid=GA1.2.1279072512.1681524561; _ym_uid=1681524561639602016; _ym_d=1681524561; promoLLid=7vu80h54b2nrj4kko6osmgou55; _ym_isad=1; tmr_lvid=1832e426c0ca39c235988c92c05df500; tmr_lvidTS=1681525060884; __ll_dv=1681526002; __ll_cp=13; _gat=1; _ym_visorc=b; tmr_detect=0%7C1681528616576;_GRECAPTCHA=09AMqPRJyjzBs7CrFbcMQqDIXyeg8oREfZUntYQER3n18N8I8cCWxamwhgR8HrtwRQXNfQ71GJXu0Ayhmv-AfLpeU; NID=511=EpcypsJ2LoWsW7LYUoscnT2MAfaxBiDtN9rNXcFepDZqtCGaZW1ilaN--GkLBWf-fW1cEd5J1IiDcDzC74dg2BQIahOV1OOO_aduK9xvT9oelhf6U1j_3Qb-yXcE9ogGvEFb5Obyo6TMvPju_SfEcM9XMN1RhXq9Uzs07BsXFo8; 1P_JAR=2023-04-15-01";// "_GRECAPTCHA=09AMqPRJzYIqjl_nnWZajcFKuJan3sfFPNNsxavtWPtsv8TdhH6V974W3FbQ_6BwlbsVZiQ-xRDGDLDJTMalxHzLE; NID=511=EpcypsJ2LoWsW7LYUoscnT2MAfaxBiDtN9rNXcFepDZqtCGaZW1ilaN--GkLBWf-fW1cEd5J1IiDcDzC74dg2BQIahOV1OOO_aduK9xvT9oelhf6U1j_3Qb-yXcE9ogGvEFb5Obyo6TMvPju_SfEcM9XMN1RhXq9Uzs07BsXFo8; 1P_JAR=2023-04-15-01";
            //
        
        string[] cookies = CookieStr.Split(';');

        HttpClient hc = new HttpClient();
        string headers = "'Host': 'www.livelib.ru', \n"+
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
        "  'Accept-Language': 'ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7', \n" +
        "'X-Requested-With': 'XMLHttpRequest' \n";
        var headerscol = headers.Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(header => header.Split(':', StringSplitOptions.RemoveEmptyEntries));
        foreach (var strings in headerscol)
        {
            hc.DefaultRequestHeaders.TryAddWithoutValidation(strings[0], strings[1]);
        }

        hc.DefaultRequestHeaders.TryAddWithoutValidation(
            "'Sec-Ch-Ua':" ,"'\"Not:A-Brand\";v=\"99\", \"Chromium\";v=\"112\"', ");
        
        hc.DefaultRequestHeaders.Add("Cookie",
            
          "LiveLibId=8e68775b8abd7b7904d0b76711cd3913; __ll_ab_mp=1; __ll_tum=3664026419; __ll_unreg_session=8e68775b8abd7b7904d0b76711cd3913; __ll_unreg_sessions_count=1; __llutmz=-600; __ll_fv=1681482795; __ll_dvs=5; ll_asid=1245240020; __popupmail_showed=1000; __popupmail_showed_uc=1; __popupmail_showed_t=1000; __gr=g1c1_; __ll_google_oauth=; __ll_google_code=; __utrx=1; llsid=9774898e440a647fec60edb78356b077; __utnx=12000205153; __llutmf=1; __utnt=g0_y0_a15721_u12000205153_c0; __ll_popup_count_pviews=regc1_challengec1_; __ll_popup_showed=reg_mail_challenge_new2_; __ll_popup_last_show=1681520939; __ll_popup_count_shows=regc1_mailc1_challenge_new2c1_; _ga=GA1.2.1016390543.1681524561; _gid=GA1.2.1279072512.1681524561; _ym_uid=1681524561639602016; _ym_d=1681524561; promoLLid=7vu80h54b2nrj4kko6osmgou55; tmr_lvid=1832e426c0ca39c235988c92c05df500; tmr_lvidTS=1681525060884; __ll_cp=14; __ll_dv=1681529585; _gat=1; _ym_isad=2; _ym_visorc=b; tmr_detect=0%7C1681611048331; iwatchyou=5df35936713e92b51205077989eee0cc;_GRECAPTCHA=09AMqPRJy3CqBYZ9GcRtZAgvqilKvBQ_qVOvkVBV9pAqfNi9mNQk8PRE01a6SIb4mJII_omvI0ReSLbnyYE61ZTBw; NID=511=EpcypsJ2LoWsW7LYUoscnT2MAfaxBiDtN9rNXcFepDZqtCGaZW1ilaN--GkLBWf-fW1cEd5J1IiDcDzC74dg2BQIahOV1OOO_aduK9xvT9oelhf6U1j_3Qb-yXcE9ogGvEFb5Obyo6TMvPju_SfEcM9XMN1RhXq9Uzs07BsXFo8; 1P_JAR=2023-04-15-01");
        var uri = new Uri(resource.URLResource);
        var responce = hc.PostAsync(uri, null);
        var html = responce.Result.Content.ReadAsStringAsync().Result;
        var parser = new HtmlParser();
        return parser.ParseDocument(html);
        
        var provider = new MemoryCookieProvider();
        //provider.(url, CookieStr);
        //var cok4 = provider.GetCookie(url);
        // foreach (var cookie in cookies)
        // {
        //     if(!cookie.Contains("__utnt"))
        //         provider.SetCookie(url, cookie);
        //     var cok = provider.GetCookie(url);
        //     Console.WriteLine("-------------------------------");
        //     Console.WriteLine($"Added: {cookie}");
        //     Console.WriteLine(cok);
        // }
        
        var cok2 = provider.GetCookie(url);
        var req = DocumentRequest.Get(url);
        req.Headers["Cookie"] = CookieStr;
        req.Headers["Host"] = "www.livelib.ru";
        req.Headers["Sec-Ch-Ua"] = "\"Not:A-Brand\";v=\"99\", \"Chromium\";v=\"99\"";
        req.Headers["Sec-Ch-Ua-Mobile"] = "70";
        req.Headers["Sec-Ch-Ua-Platform"] = "\"Windows\"";
        req.Headers["Upgrade-Insecure-Requests"] = "1";
        req.Headers["User-Agent"] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.5615.50 Safari/537.36";
        req.Headers["Accept"] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7";
        req.Headers["Sec-Fetch-Site"] = "same-origin";
        req.Headers["Sec-Fetch-Mode"] = "navigate";
        req.Headers["Sec-Fetch-User"] = "71";
        req.Headers["Sec-Fetch-Dest"] = "document";
        req.Headers["Accept-Encoding"] = "gzip, deflate";
        req.Headers["Accept-Language"] = "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7";
        req.Headers["Proxy-Authorization"] = "http://avanti:farmaapteka25@91.243.188.117:7951";
        
        var config = Configuration.Default.With(provider).WithDefaultLoader();
        var context = BrowsingContext.New(config);
        var cookie2 = context.GetCookie(url);
        var page = await context.OpenAsync(req);
        foreach (var item in context.ToDictionary())
        {
            Console.WriteLine($"{item.Key} : ( {item.Value} )");
        }
        
        context.ToDictionary();

        // var baseAddress = new Uri("https://www.livelib.ru");
        // var cookieContainer = new CookieContainer();
        // using (var handler2 = new HttpClientHandler() { CookieContainer = cookieContainer })
        // using (var client = new HttpClient(handler2) { BaseAddress = baseAddress })
        // {
        //     foreach (var cookie in cookies)
        //     {
        //         var split = cookie.Split('=');
        //         var utf8 = Encoding.GetEncoding("utf-8");
        //         string name = HttpUtility.UrlEncode(split[0], utf8);
        //         string value = HttpUtility.UrlEncode(split[0], utf8);
        //         cookieContainer.Add(baseAddress, new Cookie(name, value));
        //     }
        //     var result = await client.GetAsync("/genre");
        //     result.EnsureSuccessStatusCode();
        // }
        return page;


    }

    public Task<string> HandleAsync(IDocument data)
    {
        throw new NotImplementedException();
    }
}
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
using ExtractorProject.Settings;
using Microsoft.Extensions.Options;

namespace ExtractorProject.Extractors;

public class LiveLibUserExtractor : IExtractor<IDocument, IEnumerable<User>>
{
    private LiveLibUserExtractorSettingsInfo _settings;

    public LiveLibUserExtractor(IOptions<LiveLibUserExtractorSettingsInfo> settings)
    {
        _settings = settings.Value;
    }

    public async Task<IDocument> GetRawDataAsync(ResourceInfo resource)
    {
        var uri = new Uri(resource.URLResource);
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
        "  'Accept-Language': 'ru-RU,ru;q=0.9,en-US;q=0.8,en;q= 0.7', \n" +
        "'X-Requested-With': 'XMLHttpRequest' \n";
        var headerscol = headers.Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(header => header.Split(':', StringSplitOptions.RemoveEmptyEntries));
        foreach (var strings in headerscol)
        {
            hc.DefaultRequestHeaders.TryAddWithoutValidation(strings[0], strings[1]);
        }

        hc.DefaultRequestHeaders.TryAddWithoutValidation(
            "'Sec-Ch-Ua':" ,"'\"Not:A-Brand\";v=\"99\", \"Chromium\";v=\"112\"', ");

        hc.DefaultRequestHeaders.Add("Cookie", _settings.Cookie);
          //  "LiveLibId=b8b79824f39aee14fdc09e0ece2fae19; __ll_tum=3495147849; __ll_ab_mp=1; __ll_unreg_session=b8b79824f39aee14fdc09e0ece2fae19; __ll_unreg_sessions_count=1; tmr_lvid=fe30080f574fb4fbf3871dc58b83be4f; tmr_lvidTS=1681620833896; _ga=GA1.2.685237644.1681620834; _gid=GA1.2.1667689490.1681620834; _ym_uid=1681620835986693727; _ym_d=1681620835; _ym_isad=2; _ym_visorc=b; iwatchyou=5df35936713e92b51205077989eee0cc; __llutmz=-600; __ll_fv=1681620901; __ll_dvs=5; __ll_cp=1; ll_asid=1246767843; __ll_popup_count_pviews=regc1_; __ll_google_oauth=; __ll_google_code=; llsid=98440d279f26ffcd38e8b2d00d0ec0fe; __utnx=12000205153; __llutmf=1; __utnt=g0_y0_a15721_u12000205153_c0; __ll_dv=1681620997; tmr_detect=0%7C1681621244655 ;_GRECAPTCHA=09AMqPRJwCrCJqa1Dz9seWcvLVRxRuIWHft81RkeBiChzgi5USgczb22xEMPuihJr_zHPHA9InPKUypdM6h9g3Gvs; 1P_JAR=2023-04-16-12; NID=511=Ma22WXo7e_5MrejpmvCQQTO-e-6bPzeNVotx2fI9Comd4URzmswyuos8EnC8rCVjndO7F1v58f1OVxmz1VGQsL-FB095NQvwRclrIMrwLf7HeMnZtw4w6j_ONTust--veO1v8_GrQFj5XByZ8Vm8EVoKbc_TlZQRkY8JrmH4vi4");
        var responce = hc.PostAsync(uri, null);
        var byteArr = responce.Result.Content.ReadAsByteArrayAsync().Result;
       // var ht = Encoding.GetString(byteArr, 0, byteArr.Length - 1);
        var html = responce.Result.Content.ReadAsStringAsync().Result;
        var parser = new HtmlParser();
        
        return parser.ParseDocument(html);
        
    }

    public async Task<IEnumerable<User>> HandleAsync(IDocument data)
    {
        var users = new List<User>();
        var urowlogins = data.GetElementsByClassName("urow-login");
        foreach (var urowlogin in urowlogins)
        {
            try
            {
               
                var link = "https://www.livelib.ru" + urowlogin.GetAttribute("href");
                var userLogin = link.Split('/', StringSplitOptions.RemoveEmptyEntries).Last();
                User user = new User()
                {
                    SiteName = "https://www.livelib.ru",
                    UserLink = link,
                    UserLogin = userLogin
                };
                if(user.UserLink is null || user.UserLink == String.Empty)
                    continue;
                users.Add(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"fail: {ex.Message}");
            }
        }
        
        return users;
    }        
    
    
}
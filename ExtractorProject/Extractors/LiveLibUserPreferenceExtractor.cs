using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using Core.Extractor;
using Core.Models;
using ExtractorProject.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ExtractorProject.Extractors;

public class LiveLibUserPreferenceExtractor : IExtractor<IDocument, IEnumerable<UserPreference>>
{
    private readonly ILogger<LiveLibUserPreferenceExtractor> _logger;
    private LiveLibUserPreferenceExtractorSettingsInfo _settings;

    public LiveLibUserPreferenceExtractor(IOptions<LiveLibUserPreferenceExtractorSettingsInfo> settings, ILogger<LiveLibUserPreferenceExtractor> logger)
    {
        _logger = logger;
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
        "  'Accept-Language': 'ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7', \n" +
        "'X-Requested-With': 'XMLHttpRequest' \n";
        var headerscol = headers.Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(header => header.Split(':', StringSplitOptions.RemoveEmptyEntries));
        foreach (var strings in headerscol)
        {
            hc.DefaultRequestHeaders.TryAddWithoutValidation(strings[0], strings[1]);
        }

        hc.DefaultRequestHeaders.TryAddWithoutValidation(
            "'Sec-Ch-Ua':" ,"'\"Not:A-Brand\";v=\"99\", \"Chromium\";v=\"112\"', ");

        hc.DefaultRequestHeaders.Add("Cookie", _settings.Cookie);
           // "LiveLibId=b8b79824f39aee14fdc09e0ece2fae19; __ll_tum=3495147849; __ll_ab_mp=1; __ll_unreg_session=b8b79824f39aee14fdc09e0ece2fae19; __ll_unreg_sessions_count=1; tmr_lvid=fe30080f574fb4fbf3871dc58b83be4f; tmr_lvidTS=1681620833896; _ga=GA1.2.685237644.1681620834; _gid=GA1.2.1667689490.1681620834; _ym_uid=1681620835986693727; _ym_d=1681620835; _ym_isad=2; _ym_visorc=b; iwatchyou=5df35936713e92b51205077989eee0cc; __llutmz=-600; __ll_fv=1681620901; __ll_dvs=5; __ll_cp=1; ll_asid=1246767843; __ll_popup_count_pviews=regc1_; __ll_google_oauth=; __ll_google_code=; llsid=98440d279f26ffcd38e8b2d00d0ec0fe; __utnx=12000205153; __llutmf=1; __utnt=g0_y0_a15721_u12000205153_c0; __ll_dv=1681620997; tmr_detect=0%7C1681621244655 ;_GRECAPTCHA=09AMqPRJwCrCJqa1Dz9seWcvLVRxRuIWHft81RkeBiChzgi5USgczb22xEMPuihJr_zHPHA9InPKUypdM6h9g3Gvs; 1P_JAR=2023-04-16-12; NID=511=Ma22WXo7e_5MrejpmvCQQTO-e-6bPzeNVotx2fI9Comd4URzmswyuos8EnC8rCVjndO7F1v58f1OVxmz1VGQsL-FB095NQvwRclrIMrwLf7HeMnZtw4w6j_ONTust--veO1v8_GrQFj5XByZ8Vm8EVoKbc_TlZQRkY8JrmH4vi4");
        var responce = hc.PostAsync(uri, null);
        var html = responce.Result.Content.ReadAsStringAsync().Result;
        var parser = new HtmlParser();
        return parser.ParseDocument(html);
     }

     public async Task<IEnumerable<UserPreference>> HandleAsync(IDocument data)
     {
         //1 - делать обработку предпочтений вместе с юзером
         //2 - делать отдельно; на вход подается юзер,  провайдер качает страницу, делит кол-во предпочтений на максимум в странице
         List<UserPreference> preferences = new List<UserPreference>();
         try
         {
             var loginElem = data.GetElementsByClassName("header-profile-login")[0];
             string login = null;
             var titlelog = loginElem.GetAttribute("title");
             if (titlelog.Contains("(") && titlelog.Contains(")"))
                 login = titlelog.Substring(titlelog.IndexOf('(') + 1,
                     titlelog.IndexOf(')') - titlelog.IndexOf('(') - 1);
             else
                 login = titlelog;
             var divid = data.GetElementById("user-objects");
             var booklist = data.GetElementById("booklist").Children;
             if (booklist.Length < 0)
                 return null;
             string curData = "";
             var preftypecol = data.GetElementsByClassName("active")[0].TextContent.Split(' ');
             string preftype = null;
             if (preftypecol.Contains("Хочет"))
                 preftype = "хочет прочитать";
             else if (preftypecol.Contains("Читает"))
                 preftype = "не дочитал";
             else
                 preftype = "прочитал";
             
             foreach (var item in booklist)
             {
                 switch (item.ClassName)
                 {
                     case "brow-h2":
                         curData = item.TextContent;
                         break;
                     case "book-item-manage":
                         try
                         {
                             string? evaluation = null;
                             try
                             {
                                 evaluation = item.GetElementsByClassName("rating-value stars-color-green")[0]
                                     .TextContent;
                             }
                             catch
                             {

                             }

                             var userpref = new UserPreference()
                             {
                                 UserEvaluationDate = curData,
                                 LinkBook = "https://www.livelib.ru" +
                                            item.GetElementsByClassName("cover-wrapper")[0]
                                                .Children[0].GetAttribute("href"),
                                 PreferenceType = preftype,
                                 SiteName = "https://www.livelib.ru",
                                 UserEvaluationBook = evaluation,
                                 UserLink = "https://www.livelib.ru/reader/" + login,
                                 UserLogin = login
                             };
                             preferences.Add(userpref);
                         }
                         catch (Exception ex)
                         {

                         }

                         break;
                     default:
                         _logger.LogWarning($"КАКОЙ ТО НОВЫЙ ЭЛЕМЕНТ НА СТРАНИЦЕ {0}, {1}", data.Title ,item.ClassName);
                         break;
                 }
             }
         }
         catch (Exception ex)
         {
             _logger.LogError(ex.Message + "stack:" + ex.StackTrace);
         }

         return preferences;
     }
}
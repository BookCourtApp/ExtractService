using System.IO.Compression;
using System.Text;
using System.Xml.Linq;
using AngleSharp.Common;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using Core.Extractor;
using Core.Models;
using ExtractorProject.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ExtractorProject.Extractors;

public class LiveLibUserPreferenceExtractorJson : IExtractor<XDocument, IEnumerable<UserPreference>>
{
    private readonly ILogger<LiveLibUserPreferenceExtractor> _logger;
    private readonly LiveLibUserPreferenceExtractor _ex;
    private LiveLibUserPreferenceExtractorSettingsInfo _settings;

    public LiveLibUserPreferenceExtractorJson(IOptions<LiveLibUserPreferenceExtractorSettingsInfo> settings, ILogger<LiveLibUserPreferenceExtractor> logger, LiveLibUserPreferenceExtractor ex)
    {
        _logger = logger;
        _ex = ex;
        _settings = settings.Value;
    }

     public async Task<XDocument> GetRawDataAsync(ResourceInfo resource)
     {
         try
         {
             var uri = new Uri(resource.URLResource);
             HttpClient hc = new HttpClient();
             string headers = "Host: www.livelib.ru \n" +
                              "X-Requested-With: XMLHttpRequest \n";
             var headerscol = headers.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                 .Select(header => header.Split(':', StringSplitOptions.RemoveEmptyEntries));
             foreach (var strings in headerscol)
             {
                 hc.DefaultRequestHeaders.TryAddWithoutValidation(strings[0], strings[1]);
             }

             hc.DefaultRequestHeaders.Add("Cookie", _settings.Cookie);
             // "LiveLibId=b8b79824f39aee14fdc09e0ece2fae19; __ll_tum=3495147849; __ll_ab_mp=1; __ll_unreg_session=b8b79824f39aee14fdc09e0ece2fae19; __ll_unreg_sessions_count=1; tmr_lvid=fe30080f574fb4fbf3871dc58b83be4f; tmr_lvidTS=1681620833896; _ga=GA1.2.685237644.1681620834; _gid=GA1.2.1667689490.1681620834; _ym_uid=1681620835986693727; _ym_d=1681620835; _ym_isad=2; _ym_visorc=b; iwatchyou=5df35936713e92b51205077989eee0cc; __llutmz=-600; __ll_fv=1681620901; __ll_dvs=5; __ll_cp=1; ll_asid=1246767843; __ll_popup_count_pviews=regc1_; __ll_google_oauth=; __ll_google_code=; llsid=98440d279f26ffcd38e8b2d00d0ec0fe; __utnx=12000205153; __llutmf=1; __utnt=g0_y0_a15721_u12000205153_c0; __ll_dv=1681620997; tmr_detect=0%7C1681621244655 ;_GRECAPTCHA=09AMqPRJwCrCJqa1Dz9seWcvLVRxRuIWHft81RkeBiChzgi5USgczb22xEMPuihJr_zHPHA9InPKUypdM6h9g3Gvs; 1P_JAR=2023-04-16-12; NID=511=Ma22WXo7e_5MrejpmvCQQTO-e-6bPzeNVotx2fI9Comd4URzmswyuos8EnC8rCVjndO7F1v58f1OVxmz1VGQsL-FB095NQvwRclrIMrwLf7HeMnZtw4w6j_ONTust--veO1v8_GrQFj5XByZ8Vm8EVoKbc_TlZQRkY8JrmH4vi4");
             var responce = await hc.PostAsync(uri, null);
             string json = Encoding.UTF8.GetString(await responce.Content.ReadAsByteArrayAsync());
             string adapter = $"{{\"value\": {json}}}";
             var xDocument = JsonConvert.DeserializeXNode(adapter);
             return xDocument;
         }
         catch (Exception ex)
         {
             _logger.LogError("Не удалось получить страницу {0}", resource.URLResource);
         }

         return null;
     }

     public async Task<IEnumerable<UserPreference>> HandleAsync(XDocument data)
     {
         var preferences = new List<UserPreference>();
         try
         {

             var html = data.Element("value").Element("content").Value;
             var htmlParser = new HtmlParser();
             var document = htmlParser.ParseDocument(html);
             string curData = "";
             foreach (var item in document.GetElementsByTagName("body")[0].Children)
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
                                 SiteName = "https://www.livelib.ru",
                                 UserEvaluationBook = evaluation,
                             };
                             preferences.Add(userpref);
                         }
                         catch (Exception ex)
                         {

                         }

                         break;
                     case "block-border card-block":
                         break;
                     default:
                         _logger.LogWarning("КАКОЙ ТО НОВЫЙ ЭЛЕМЕНТ НА СТРАНИЦЕ  CLASS: {0}",  item.ClassName);
                         break;
                 }
             }
         }
         catch (Exception ex)
         {
             _logger.LogError("Ошибка при обработке предпочтений: {0}, stack: {1}", ex.Message, ex.StackTrace);
         }
         return preferences;
     }
}
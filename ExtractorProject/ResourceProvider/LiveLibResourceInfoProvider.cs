using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using AngleSharp.Media;
using Core.Extractor;
using Core.Models;
using Core.Settings;
using ExtractorProject.Settings;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorProject.ResourceProvider;

/// <summary>
/// провайдер ресурсов для LiveLib
/// </summary>
public class LiveLibResourceInfoProvider : IResourceInfoProvider
{
    private readonly List<string>_categoriesURL;

    public LiveLibResourceInfoProvider(IOptions<LiveLibProviderSettingsInfo> settings)
    {
        _categoriesURL = settings.Value.CategoriesURL;
    }

    /// <summary>
    /// Получение HTML-страницы по заданному URL
    /// </summary>
    private IDocument GetHTMLPage(string URL)
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
        hc.DefaultRequestHeaders.TryAddWithoutValidation("Sec-Ch-Ua:", "\"Not:A-Brand\"; v = \"99\", \"Chromium\"; v = \"112\"  \n");
        hc.DefaultRequestHeaders.Add("Cookie",
       "__ll_ab_mp=1; __ll_tum=3664026419; __llutmz=-600; __ll_fv=1681482795; __popupmail_showed=1000; __popupmail_showed_uc=1; llsid=9774898e440a647fec60edb78356b077; __utnx=12000205153; __llutmf=1; __ll_popup_count_pviews=regc1_challengec1_; __ll_popup_count_shows=regc1_mailc1_challenge_new2c1_; _ga=GA1.2.1016390543.1681524561; _ym_uid=1681524561639602016; _ym_d=1681524561; tmr_lvid=1832e426c0ca39c235988c92c05df500; tmr_lvidTS=1681525060884; LiveLibId=d170fcde8bc292ff99743ebdfff5b49d; __utnt=g0_y0_a15721_u0_c0; __ll_unreg_session=d170fcde8bc292ff99743ebdfff5b49d; __ll_unreg_sessions_count=4; __ll_dvs=1; ll_asid=1249188717; __ll_popup_showed=reg_; __ll_popup_last_show=1681808719; __ll_unreg_r=60; _gid=GA1.2.1819667972.1681811878; iwatchyou=521552be61ccf985dae6379b16b8df97; __gr=g1c9_g1222c2_g1217c3_g433c1_; tmr_detect=0%7C1681811982427; _gat=1; _ym_isad=1; _ym_visorc=w; __ll_cp=42; __ll_dv=1681897648;_GRECAPTCHA=09AMqPRJwCrCJqa1Dz9seWcvLVRxRuIWHft81RkeBiChzgi5USgczb22xEMPuihJr_zHPHA9InPKUypdM6h9g3Gvs; 1P_JAR=2023-04-16-12; NID=511=Ma22WXo7e_5MrejpmvCQQTO-e-6bPzeNVotx2fI9Comd4URzmswyuos8EnC8rCVjndO7F1v58f1OVxmz1VGQsL-FB095NQvwRclrIMrwLf7HeMnZtw4w6j_ONTust--veO1v8_GrQFj5XByZ8Vm8EVoKbc_TlZQRkY8JrmH4vi4");
        var uri = new Uri(URL);
        var responce = hc.PostAsync(uri, null);
        var html = responce.Result.Content.ReadAsStringAsync().Result;
        var parser = new HtmlParser();

        return parser.ParseDocument(html);
    }

    ///<inheritdoc/>
    public IEnumerable<ResourceInfo> GetResources()
    {
        foreach (var category in _categoriesURL)
        {
            int i = 1;
            IDocument document = null;
            try
            {
                document = GetHTMLPage(category + "/listview/biglist/~" + i.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("Документ не был получен" + e);
            }
            IHtmlCollection<IElement> links;
            while (document.Title != "404 @ LiveLib")
            {
                i++;
                if(document.Title == "LiveLib")
                {
                    Console.WriteLine("CAPTCHA CAPTCHA CAPTCHA CAPTCHA CAPTCHA CAPTCHA \n CAPTCHA CAPTCHA CAPTCHA CAPTCHA CAPTCHA CAPTCHA CAPTCHA CAPTCHA CAPTCHA CAPTCHA");
                    document = GetHTMLPage(category + "/listview/biglist/~" + i.ToString());
                    continue;
                }
                links = document.GetElementsByClassName("brow-book-name with-cycle");
                foreach (var link in links)
                {
                    var resourceUrl = new ResourceInfo() { URLResource = "https://www.livelib.ru/" + link.GetAttribute("href") };
                    yield return resourceUrl;
                }
                document = GetHTMLPage(category + "/listview/biglist/~" + i.ToString());

            }
        }
    }   
}

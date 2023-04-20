using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using AngleSharp.Media;
using Core.Extractor;
using Core.Models;
using Core.Settings;
using ExtractorProject.Settings;
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

    public LiveLibResourceInfoProvider(ResourceProviderSettings settings)
    {
        LiveLibProviderSettingsInfo providerSettingsInfo = settings.Info as LiveLibProviderSettingsInfo
                                       ?? throw new NullReferenceException($"{nameof(settings)} не подходит для итератора лабиринта");
        _categoriesURL = providerSettingsInfo.CategoriesURL;
    }

    /// <summary>
    /// Получение HTML-страницы по заданному URL
    /// </summary>
    private IDocument GetHTMLPage(string URL)
    {
        Console.WriteLine("111");
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
       "__ll_tum=3199867700; __ll_ab_mp=1; tmr_lvid=cfec893d0e7bfe44acef9aa2191f6de9; tmr_lvidTS=1681617845503; _ga=GA1.2.1071574945.1681617846; _ym_uid=1681617847426953912; _ym_d=1681617847; __llutmz=-600; __ll_fv=1681617920; __popupmail_showed=1000; __popupmail_showed_uc=1; __ll_popup_count_shows=regc1_mailc1_; __gr=g392c1_g387c1_g510c1_g149c1_g1251c1_g1240c1_; __ll_unreg_session=faabdca5c8a8620777133fc5f09195f1; __ll_unreg_sessions_count=5; llsid=62301122102e5e88d574058fb24a433c; __utnx=12006965305; __llutmf=1; __ll_popup_count_pviews=regc1_challengec1_; _gid=GA1.2.366892746.1681810244; _ym_isad=2; iwatchyou=27784c29f78fe4bc8583452f6ea403d9; __ll_cp=16; tmr_detect=0%7C1681811878859; __ll_dv=1681815282;_GRECAPTCHA=09ALnTWt7JHSwrKwSKdW9r_r31F9BIrVHF8QA7KPAT_JeOQFW-RlAhxhcIdRJ224zRYnnh5TQaWNq_F_ft6Vp8aMk");
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

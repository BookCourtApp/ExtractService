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
using System.IO.Compression;
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
    private async Task<IDocument> GetHTMLPage(string URL)
    {
        var parser = new HtmlParser();
        try
        {

       
        HttpClient hc = new HttpClient();
        string headers = //"Sec-Ch-Ua: "Not:A-Brand";v="99", "Chromium";v="112"
            // "Sec-Ch-Ua-Mobile: ?0 \n " +
            // "Sec-Ch-Ua-Platform: \"Windows\" \n " +
            // "Upgrade-Insecure-Requests: 1 \n " +
            "User-Agent: Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; KKman2.0; .NET CLR 1.0.3705)" +// Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.5615.50 Safari/537.36 \n ";
        "Referer: google.com";
            // "Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7 \n ";
            var headerscol = headers.Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(header => header.Split(':', StringSplitOptions.RemoveEmptyEntries).Select(s => s.Replace(" ", "").Replace("\n", "")));
        foreach (var strings in headerscol)
        {
            var arraystr = strings.ToArray();
            if (arraystr.Length != 2)
            {
                //Console.WriteLine("Ошибка в массиве куках");
                continue;
            }
            var res = hc.DefaultRequestHeaders.TryAddWithoutValidation(arraystr[0], arraystr[1]);
        }
        hc.DefaultRequestHeaders.TryAddWithoutValidation("Sec-Ch-Ua:", "\"Not:A-Brand\"; v = \"99\", \"Chromium\"; v = \"112\"  \n");
        hc.DefaultRequestHeaders.Add("Cookie",
            "__ll_ab_mp=1; __ll_tum=3664026419; __llutmz=-600; __ll_fv=1681482795; __popupmail_showed=1000; __popupmail_showed_uc=1; llsid=9774898e440a647fec60edb78356b077; __utnx=12000205153; __llutmf=1; __ll_popup_count_pviews=regc1_challengec1_; __ll_popup_count_shows=regc1_mailc1_challenge_new2c1_; _ga=GA1.2.1016390543.1681524561; _ym_uid=1681524561639602016; _ym_d=1681524561; tmr_lvid=1832e426c0ca39c235988c92c05df500; tmr_lvidTS=1681525060884; _gid=GA1.2.409091525.1682111664; _ym_isad=2; iwatchyou=521552be61ccf985dae6379b16b8df97; __gr=g1c11_g1222c2_g1217c3_g433c1_; __ll_cp=45; __ll_dv=1682159879; LiveLibId=7b8f90d47513970402bc57ed5bf915ab; __utnt=g0_y0_a15721_u0_c0; __ll_unreg_session=7b8f90d47513970402bc57ed5bf915ab; __ll_unreg_sessions_count=5; _gat=1; _ym_visorc=b; tmr_detect=0%7C1682163058197;_GRECAPTCHA=09AJ2rgEOpdTTMf2elGiX2uy2cwqX3SFIQNp9MuPGTZqdBoMMhYDHg1SvwQgthqqGAkOuR_e5xl2qKj7aDKQX9rOo; 1P_JAR=2023-04-18-09; NID=511=AkwlwrcqrTjIbHVd2SAwjWSGY-S896K8sJj8XN7HsiAbrcTaTaOxmTCsg-z8yOJz6VpfHkMpZtglCdFDaAIVCwDECFuKCHOcB7pl5Yz8wh30y3-2Ly2PmVMTiQ063JV6NrRNFzNUTFUQ8d21G0oi_7D9nJOKTANnQzF5x7LYjz0"); //_GRECAPTCHA=09AJ2rgEPGBOonczHqMcidqBJ-AgxJnUG9KW5ucQeH6nw8m8ftPzkHcymazV7gIIVRq3DAdoAP_afLnip1FmRO70g; 1P_JAR=2023-04-18-09; NID=511=AkwlwrcqrTjIbHVd2SAwjWSGY-S896K8sJj8XN7HsiAbrcTaTaOxmTCsg-z8yOJz6VpfHkMpZtglCdFDaAIVCwDECFuKCHOcB7pl5Yz8wh30y3-2Ly2PmVMTiQ063JV6NrRNFzNUTFUQ8d21G0oi_7D9nJOKTANnQzF5x7LYjz0");
        
        var uri = new Uri(URL);
        var responce = await hc.PostAsync(uri, null);
        var html =await responce.Content.ReadAsStringAsync();


        return parser.ParseDocument(html);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return parser.ParseDocument("");
    }

    ///<inheritdoc/>
    public IEnumerable<ResourceInfo> GetResources()
    {
        foreach (var category in _categoriesURL)
        {
            Console.WriteLine("Started "+category);
            int i = 1;
            IDocument document = null;
            try
            {
                document = GetHTMLPage(category + "/listview/biglist/~" + i.ToString()).Result;
                if(document == null)
                    continue;
            }
            catch (Exception e)
            {
                Console.WriteLine("Документ не был получен" + e + $"\nСсылка: {category + "/listview/biglist/~" + i.ToString()}");

            }
            IHtmlCollection<IElement> links;
            
            //links = document.GetElementsByClassName("brow-book-name with-cycle");
            while (document.Title != "404 @ LiveLib")
            {
                i++;
                if(document.Title == "LiveLib")
                {
                    Console.WriteLine($"DANGER! {document.Title}, link: {category + "/listview/biglist/~" + i.ToString()}");
                    document = GetHTMLPage(category + "/listview/biglist/~" + i.ToString()).Result;
                    continue;
                }
                links = document.GetElementsByClassName("brow-book-name with-cycle");
                foreach (var link in links)
                {
                    var resourceUrl = new ResourceInfo() { URLResource = "https://www.livelib.ru/" + link.GetAttribute("href") };
                    yield return resourceUrl;
                }
                document = GetHTMLPage(category + "/listview/biglist/~" + i.ToString()).Result;

            }
            Console.WriteLine("Finished "+category);
            break; //TODO: убрать
        }
    }   
}

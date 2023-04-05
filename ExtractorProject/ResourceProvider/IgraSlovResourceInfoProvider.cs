using AngleSharp;
using AngleSharp.Dom;
using Core.Extractor;
using Core.Models;
using Core.Settings;
using ExtractorProject.Settings;

namespace ExtractorProject.ResourceProvider;

/// <summary>
/// провайдер ресурсов для игры слов
/// </summary>
public class IgraSlovResourceInfoProvider : IResourceInfoProvider
{
    private readonly string _catalogUrl;
    private readonly string _Url;
    public IgraSlovResourceInfoProvider(ResourceProviderSettings settings)
    {
        IgraSlovProviderSettings providerSettingsInfo = settings.Info as IgraSlovProviderSettings
                                       ?? throw new NullReferenceException($"{nameof(settings)} не подходит для Игры Слов");
        _catalogUrl = settings.Site;
        _Url = providerSettingsInfo.Url;
        
    }


    /// <inheritdoc />
    public IEnumerable<ResourceInfo> GetResources()
    {
        var config = Configuration.Default.WithDefaultLoader();
        var context = BrowsingContext.New(config);
        var page = context.OpenAsync(_Url).Result;

        var bookElementsFromPage = page.GetElementsByClassName("woo-entry-inner clr");
        foreach (var bookFromList in bookElementsFromPage)
        {
            var refToBook = bookFromList.GetElementsByClassName("woo-entry-image clr")[0].Children[0].Attributes["href"].Value;
            var resourceUrl = new ResourceInfo() { URLResource = refToBook };
            yield return resourceUrl;
        }
    }
}

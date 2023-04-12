using AngleSharp;
using AngleSharp.Dom;
using Core.Extractor;
using Core.Models;
using Core.Settings;
using ExtractorProject.Settings;

namespace ExtractorProject.ResourceProvider;

/// <summary>
/// Провайдер ресурсов для Primbook
/// </summary>
public class PrimbookResourceInfoProvider : IResourceInfoProvider
{
    private string[] _catalogUrls;
    
    
    public PrimbookResourceInfoProvider(ResourceProviderSettings settings)
    {
        PrimbookProviderSettingsInfo providerSettingsInfo = settings.Info as PrimbookProviderSettingsInfo
                                                        ?? throw new NullReferenceException($"{nameof(settings)} не подходит для итератора primbook");
        _catalogUrls = providerSettingsInfo.CatalogUrls;
    }
    
    /// <summary>
    /// метод для взятия IDocument через AngleSharp
    /// </summary>
    /// <param name="url">url по которому будет скачиваться IDocument</param>
    private async Task<IDocument> GetDocumentAsync(string url)
    {
        var config = Configuration.Default.WithDefaultLoader();
        var context = BrowsingContext.New(config);
        var page = await context.OpenAsync(url);
        return page;
    }


    /// <inheritdoc />
    public IEnumerable<ResourceInfo> GetResources()
    {
        //todo: try-catch обёртки
        foreach (var catalogUrl in _catalogUrls)
        {
            var catalogPage = GetDocumentAsync(catalogUrl).Result;
            int countPages = Int32.Parse(catalogPage.GetElementsByClassName("system-pagenavigation-item")[^2].TextContent);
            for (int i = 0; i < countPages; i++)
            {
                var pageUrl = catalogUrl + i;
                var pageDocument = GetDocumentAsync(pageUrl).Result;
                var bookPageUrls = pageDocument
                    .GetElementsByClassName("catalog-section-item intec-grid-item-3 intec-grid-item-700-2 intec-grid-item-720-3 intec-grid-item-950-2")
                    .Select(e => "https://primbook.ru" + e.GetElementsByClassName("catalog-section-item-image-wrapper intec-image-effect")[0]
                                                                .Attributes["href"].Value);
                foreach (var bookPageUrl in bookPageUrls)
                {
                    var result = new ResourceInfo() { URLResource = bookPageUrl };
                    yield return result;
                }
            }
        }
    }
}
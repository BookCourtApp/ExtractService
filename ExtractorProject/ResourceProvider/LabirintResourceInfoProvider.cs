using Core.Extractor;
using Core.Models;
using Core.Settings;
using ExtractorProject.Settings;

namespace ExtractorProject.ResourceProvider;

/// <summary>
/// провайдер ресурсов для лабиринта
/// </summary>
public class LabirintResourceInfoProvider : IResourceInfoProvider
{
    private readonly string _catalogUrl;
    private readonly int _minId;
    private readonly int _maxId;

    public LabirintResourceInfoProvider(IProviderSettingsInfo settings)
    {
        LabirintProviderSettings providerSettingsInfo = settings as LabirintProviderSettings
                ?? throw new NullReferenceException($"{nameof(settings)} не подходит для итератора лабиринта");
        _catalogUrl = providerSettingsInfo.CatalogUrl;
        _minId = providerSettingsInfo.MinId;
        _maxId = providerSettingsInfo.MaxId;
    }


    /// <inheritdoc />
    public IEnumerable<ResourceInfo> GetResourcesAsync()
    {
        for (int i = _minId; i < _maxId; i++)
        {
            var resourceUrl = new ResourceInfo() { URLResource = _catalogUrl + i };
            yield return resourceUrl;
        }
    }
}
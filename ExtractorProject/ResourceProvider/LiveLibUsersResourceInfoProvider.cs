using AngleSharp.Media;
using Core.Extractor;
using Core.Models;
using Core.Settings;
using ExtractorProject.Settings;
using Microsoft.Extensions.Options;

namespace ExtractorProject.ResourceProvider;

public class LiveLibUsersResourceInfoProvider : IResourceInfoProvider
{
    private readonly string _catalogUrl;
    private readonly int _minId;
    private readonly int _maxId;

    public LiveLibUsersResourceInfoProvider(IOptions<LiveLibUserProviderSettingsInfo> settings)
    {
        _catalogUrl = settings.Value.CatalogUrl;
        _minId = settings.Value.MinId;
        _maxId = settings.Value.MaxId;
    }
    
    public IEnumerable<ResourceInfo> GetResources()
    {
        for (int i = _minId; i < _maxId; i++)
        {
            var resource = new ResourceInfo() { URLResource = _catalogUrl + i };
            yield return resource;
        }
        
    }
}
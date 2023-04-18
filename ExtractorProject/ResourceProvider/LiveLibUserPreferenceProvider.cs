using Core.Extractor;
using Core.Models;
using Microsoft.Extensions.Options;

namespace ExtractorProject.ResourceProvider;

public class LiveLibUserPreferenceProvider : IResourceInfoProvider
{
    public LiveLibUserPreferenceProvider()
    {

    }

    public IEnumerable<ResourceInfo> GetResources()
    {
        return null;
    }
}
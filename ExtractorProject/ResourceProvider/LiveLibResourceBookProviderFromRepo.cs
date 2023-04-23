using Core.Extractor;
using Core.Models;
using Core.Repository;

namespace ExtractorProject.ResourceProvider;

public class LiveLibResourceBookProviderFromRepo  : IResourceInfoProvider
{
    private readonly IBookRepository _repository;

    public LiveLibResourceBookProviderFromRepo(IBookRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<ResourceInfo> GetResources()
    {
        var list = _repository.Get1000BookLinksNotProcessedAsync().Result;
        foreach (var l in list)
        {
            yield return new ResourceInfo(){URLResource = l};
        }
        while (list.Count() > 0)
        {
            list = _repository.Get1000BookLinksNotProcessedAsync().Result;
            foreach (var l in list)
            {
                yield return new ResourceInfo(){URLResource = l};
            }
        }
    }
}
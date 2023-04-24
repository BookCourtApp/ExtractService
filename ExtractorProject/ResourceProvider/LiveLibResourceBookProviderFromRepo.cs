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
        var list = _repository.Get1000BookLinksNotProcessedAsync(1, 1000).Result;
        foreach (var l in list)
        {
            yield return new ResourceInfo(){URLResource = l};
        }
        int page = 1;
        while (list.Count() > 0)
        {
            page++;
            list = _repository.Get1000BookLinksNotProcessedAsync(page, 1000).Result;
            foreach (var l in list)
            {
                yield return new ResourceInfo(){URLResource = l};
            }
        }
    }
}
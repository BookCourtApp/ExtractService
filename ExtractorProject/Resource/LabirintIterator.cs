using ExtractorProject.Config;

namespace ExtractorProject.Resource;

public class LabirintIterator
{
    private readonly string _catalogUrl;
    private readonly int _minId;
    private readonly int _maxId;

    public LabirintIterator(LabirintIteratorConfig config)
    {
        _catalogUrl = config.CatalogUrl;
        _minId = config.MinId;
        _maxId = config.MaxId;
    }

    public IEnumerable<string> GetResources()
    {
        for (int i = _minId; i < _maxId; i++)
        {
            string resourceUrl = _catalogUrl + i;
            yield return resourceUrl;
        }
    }
}
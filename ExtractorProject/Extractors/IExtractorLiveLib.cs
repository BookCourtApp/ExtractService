using ExtractorService.Models;

namespace ExtractorService.Extractor
{
    public interface IExtractorLiveLib<T>
    {
        ExtractBatchResult<T> ExtractNextBatch();
        bool IsEndData();
    }
}
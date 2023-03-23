using ExtractorService.ExtractorProject.Extractors.Entity;

namespace ExtractorService.ExtractorProject.Extractors
{
    public interface IExtractorLiveLib<T>
    {
        ExtractBatchResult<T> ExtractNextBatch();
        bool IsEndData();
    }
}
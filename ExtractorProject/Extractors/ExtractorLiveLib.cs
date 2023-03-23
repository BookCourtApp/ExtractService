using ExtractorService.Extractor;
using ExtractorService.Models;

namespace ExtractorProject.Extractors
{
    public class ExtractorLiveLib<T> : AbstractExtractor, IExtractor<T>
    {
        public ExtractorLiveLib(ExtractorSettings Settings) : base(Settings)
        {
        }

        public ExtractBatchResult<T> ExtractNextBatch()
        {
            throw new NotImplementedException();
        }

        public bool IsEndData()
        {
            throw new NotImplementedException();
        }
    }
}

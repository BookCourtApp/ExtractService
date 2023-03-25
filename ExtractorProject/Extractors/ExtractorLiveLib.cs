using Core;
using Core.Object;

namespace ExtractorProject.Extractors
{
    public class ExtractorLiveLib<T> : AbstractExtractor, IExtractor<T>
    {
        public ExtractorLiveLib(ExtractorSettings Settings) : base(Settings)
        {
        }

        public Task<ExtractBatchResult<T>> ExtractNextBatch()
        {
            throw new NotImplementedException();
        }

        public bool IsEndData()
        {
            throw new NotImplementedException();
        }
    }
}

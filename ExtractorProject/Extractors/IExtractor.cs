using ExtractorService.Models;

namespace ExtractorService.Extractor{
    public interface IExtractor{
        public bool IsEndData {get;set;}
        public ExtractBatchResult ExtractNextBatch();
    }
}
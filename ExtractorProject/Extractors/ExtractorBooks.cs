using ExtractorService.Models;

namespace ExtractorService.Extractor{
    public class ExtractorBooks : AbstractExtractor, IExtractor{
        ExtractorBooks(ExtractorSettings Settings) : base(Settings){
            throw new NotImplementedException();
        }
        public bool IsEndData(){
            throw new NotImplementedException();
        }
        public ExtractBatchResult ExtractNextBatch(){
            throw new NotImplementedException(); 
        }
    }
}

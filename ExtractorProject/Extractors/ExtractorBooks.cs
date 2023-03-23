using ExtractorService.Models;

namespace ExtractorService.Extractor{
    public class ExtractorBooks<T> : AbstractExtractor, IExtractor<T>{
        ExtractorBooks(ExtractorSettings Settings) : base(Settings){
            throw new NotImplementedException();
        }
        public bool IsEndData(){
            throw new NotImplementedException();
        }
        public ExtractBatchResult<T> ExtractNextBatch(){
            throw new NotImplementedException(); 
        }
    }
}

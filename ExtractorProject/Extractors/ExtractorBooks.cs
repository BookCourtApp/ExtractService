using ExtractorService.Models;

namespace ExtractorService.Extractor{
    public class ExtractorBooks : AbstractExtractor, IExtractor{
        ExtractorBooks(ExtractorSettings Settings) : base(Settings){
            throw new NotImplementedException();
        }
        public bool IsEndData {get;set;}
        //public IDocument ExtractReviews();
        //public List<Book> HandleData(IDocument Page); 
        public ExtractBatchResult ExtractNextBatch(){
            throw new NotImplementedException(); 
        }
    }
}

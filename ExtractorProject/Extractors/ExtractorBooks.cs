using ExtractorService.Models;

namespace ExtractorService.Extractor{
    public ExtractorBooks{
        public IDocument ExtractReviews();
        public List<Book> HandleData(IDocument Page); 
    }
}

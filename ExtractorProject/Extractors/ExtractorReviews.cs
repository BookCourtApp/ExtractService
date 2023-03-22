using ExtractorService.Models;

namespace ExtractorService.Extractor{
    public ExtractorReviews{
        public IDocument ExtractReviews();
        public List<Review> HandleData(IDocument Page); 
    }
}

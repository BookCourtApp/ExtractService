using ExtractorService.Models;

namespace ExtractorService.Extractor{
    public abstract class AbstractExtractor : ExtractorReviews, ExtractorBooks, IExtractor{
        public AbstractExtractor(ExtractorSettings) 
    }
}


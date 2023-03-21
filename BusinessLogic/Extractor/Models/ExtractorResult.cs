

namespace ExtractorService.Models{
    public class ExtractorResult{
        public int ExtratctorDataCount {get;set;}
        public ErrorResultInfo ErrorResult{get;set;}
        public Datetime AverageBookProcessing {get;set;}
        public Datetime TimeOfCompletion {get;set;} 
        public List<T> Buffer {get;set;}
    }
}
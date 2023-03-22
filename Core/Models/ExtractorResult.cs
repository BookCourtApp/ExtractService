namespace ExtractorService.Models
{
    public class ExtractorResult
    {
        public int ExtratctorDataCount { get; set; }
        public ErrorResultInfo ErrorResult { get; set; }
        public DateTime AverageBookProcessing { get; set; }
        public DateTime TimeOfCompletion { get; set; }
        public List<T> Buffer { get; set; }
    }
}

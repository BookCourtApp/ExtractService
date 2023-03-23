namespace ExtractorService.Models
{
    public class ExtractBatchResult
    {
        public int ExtratctorDataCount { get; set; }
        public ErrorResultInfo ErrorResult { get; set; }
        public DateTime AverageBookProcessing { get; set; }
        public DateTime TimeOfCompletion { get; set; }
        public List<Book> Buffer { get; set; }
    }
}

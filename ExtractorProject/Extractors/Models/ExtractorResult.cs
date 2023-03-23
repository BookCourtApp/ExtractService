namespace ExtractorService.Models
{
    public class ExtractorResult
    {
        public Guid Id {get;set;}
        public int ExtratctorDataCount { get; set; }
        public ErrorResultInfo ErrorResult { get; set; }
        public DateTime AverageBookProcessing { get; set; }
        public DateTime TimeOfCompletion { get; set; }
        public List<Book> Buffer { get; set; }
    }
}

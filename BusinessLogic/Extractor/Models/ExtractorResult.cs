

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

    public class ErrorResultInfo
    {
        public List<Error> Errors { get; set; }
        public int ErrorCount { get; set; }

    }

    public class Error
    {
        public string Reason { get; set; }
        public int idLogs { get; set; }
        public Enum PlaceType { get; set; } 
    }

    public enum PlaceType
    {
        Extracting = 0,
        Handling = 1,
        Saving = 2
    }

    public enum RawData
    {
        API = 0,
        HTML = 1,
        SCV = 2
    }

    public class ExtractorSetting
    {
        public string URL { get; set; }
        public ExtractorType Type { get; set; }
        public int BatchCount { get; set; }
    }

    public class ExtractorType
    {
        public Enum SourceType { get; set; }
        public ExtractInputData InputData { get; set; }
    }

    public enum SourceType 
    {
        API = 0,
        HTML = 1,
        SCV = 2
    }

    public class T
    {
        public class Review
        {
            public string SiteName { get; set; }
            public string UserLogin { get; set; }
            public string UserLink { get; set; }
            public string ReviewType { get; set; }
            public string LinkBook { get; set; }
            public string UserReviewText { get; set; }
            public int UserEvaluationBook { get; set; }
            public DateOnly UserEvaluationDate { get; set; }
            public DateTime UserEvaluationTime { get; set; }
            public int UserNumberWhoLikeReview { get; set; }
            public int NumberCommentsReview { get; set; }
            public int UserNumberWhoFavoritesReview { get; set; }

        }

        public class Book
        {
            public string Name { get; set; }
            public string Author { get; set; }
            public string Description { get; set; }
            public int Price { get; set; }
            public int Remainder { get; set; }
            public string SourceName { get; set; }
            public string Image { get; set; }
            public string Genre { get; set; }
            public int NumberOfPages { get; set; }
            public string ISBN { get; set; }
            public DateTime ParsingDate { get; set; }
            public int PublisherYear { get; set; }
            public int RecommendedAge { get; set; }

        }
    }

    

}

namespace ExtractorService.Models
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
}

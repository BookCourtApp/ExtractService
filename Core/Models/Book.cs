
namespace ExtractorService.Models
{
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

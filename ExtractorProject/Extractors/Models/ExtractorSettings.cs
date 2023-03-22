
namespace ExtractorService.Models
{
    public class ExtractorSetting
    {
        public string URL { get; set; }
        public ExtractorType Type { get; set; }
        public int BatchCount { get; set; }
    }
}

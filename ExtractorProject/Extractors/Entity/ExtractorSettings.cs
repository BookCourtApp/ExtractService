
namespace ExtractorService.Models
{
    public class ExtractorSettings
    {
        /// <summary>
        /// Ссылка для обращения к ресурсу с записями 
        /// </summary>
        public string URL { get; set; }
        public ExtractorType Type { get; set; }
        /// <summary>
        /// Количество записей, которые парсятся за один заход парсером
        /// </summary>
        public int BatchCount { get; set; }
    }
}

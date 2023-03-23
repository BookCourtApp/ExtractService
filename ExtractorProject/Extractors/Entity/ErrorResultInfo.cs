
namespace ExtractorService.Models{
    public class ErrorResultInfo
    {
        /// <summary>
        /// Коллекция ошибок
        /// </summary>
        public List<Error> Errors { get; set; }
        /// <summary>
        /// Количество ошибок
        /// </summary>
        public int ErrorCount { get; set; }
    }
}

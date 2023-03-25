using ExtractorProject.Extractors.Models;

namespace ExtractorProject.Extractors.Entity
{
    /// <summary>
    /// Сущность для хранения информации об ошибке
    /// </summary>
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

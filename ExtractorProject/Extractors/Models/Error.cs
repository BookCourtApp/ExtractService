using ExtractorService.ExtractorProject.Extractors.Entity;

namespace ExtractorService.ExtractorProject.Extractors.Models
{
    /// <summary>
    /// Модель для хранении информации об ошибке в бд.
    /// </summary>
    public class Error
    {
        /// <summary>
        /// Id для хранения в бд.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Причина ошибки
        /// </summary>
        public string Reason { get; set; }
        public PlaceType Type{ get; set; }

        /// <summary>
        /// Внешний ключ для таблицы ExtractorResult
        /// </summary>
        public Guid ExtractorResultId { get; set; }
    }
}

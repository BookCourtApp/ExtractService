using Core.Object;

namespace Core.Models
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
        
        /// <summary>
        /// место возникновения ошибки
        /// </summary>
        public PlaceType Type{ get; set; }

        /// <summary>
        /// Внешний ключ для таблицы ExtractorResult
        /// </summary>
        public Guid ExtractorResultId { get; set; }
    }
}

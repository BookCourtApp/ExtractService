using Core.ExtractModels;
using Core.Models;

namespace Core.Object
{
    /// <summary>
    /// Сущность для хранения информации о результате парсинга 
    /// </summary>
    public class ExtractBatchResult<T>
    {
        /// <summary>
        /// Количество выгруженных записей
        /// </summary>
        public int ExtratctorDataCount { get; set; }
        /// <summary>
        /// Коллекция результатов ошибок
        /// </summary>
        public List<Error> Errors { get; set; }
        /// <summary>
        /// Среднее время обработки ошибок
        /// </summary>
        public DateTime AverageBookProcessing { get; set; }
        /// <summary>
        /// Время выполнения парсинга
        /// </summary>
        public DateTime TimeOfCompletion { get; set; }
        
        /// <summary>
        /// Коллекция спарешнных данных
        /// </summary>
        public List<T> Buffer { get; set; }
    }
}

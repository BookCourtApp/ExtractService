namespace ExtractorService.Models
{
    public class ExtractorResult
    {
        /// <summary>
        /// Поле Id для хранения записей ExtractorResult в базеданных
        /// </summary>
        public Guid Id {get;set;} 
        
        /// <summary>
        /// Количество выгруженных записей
        /// </summary>
        public int ExtractorDataCount { get; set; }

        /// <summary>
        /// Список ошибок
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
    }
}

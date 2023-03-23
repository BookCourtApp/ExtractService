namespace ExtractorService.Models
{
    public class ExtractBatchResult
    {
        /// <summary>
        /// Количество выгруженных записей
        /// </summary>
        public int ExtratctorDataCount { get; set; }
        /// <summary>
        /// Коллекция результатов ошибок
        /// </summary>
        public ErrorResultInfo ErrorResult { get; set; }
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
        public List<Book> Buffer { get; set; }
    }
}

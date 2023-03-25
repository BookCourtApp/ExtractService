namespace ExtractorProject.Extractors.Entity
{
    /// <summary>
    /// Сущность для хранения информации о результате парсинга 
    /// </summary>
    /// <typeparam name="T">Обобщенный тип, нужный для парсинга разного рода данных(например книги, или отзывы)</typeparam>
    public class ExtractBatchResult<T>
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
        public List<T> Buffer { get; set; }
    }
}

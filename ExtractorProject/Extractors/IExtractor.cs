using ExtractorService.Models;

namespace ExtractorService.Extractor{
    public interface IExtractor{
        /// <summary>
        /// Возвращает окончание работы Extractor'а
        /// </summary>
        /// <returns>True, если выкачка закончена</returns>
        public bool IsEndData();
        /// <summary>
        /// Метод для парсинга информации
        /// </summary>
        /// <returns>Класс ExtractBatchResult с информацией о парсинге и коллекцией записей парсинга</returns>
        public ExtractBatchResult ExtractNextBatch();
    }
}
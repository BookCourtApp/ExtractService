using ExtractorService.ExtractorProject.Extractors.Entity;

namespace ExtractorService.ExtractorProject.Extractors
{
    /// <summary>
    /// Интерфейс, описывающий реализацию парсеров 
    /// </summary>
    /// <typeparam name="T">Обобщенный тип, нужный для парсинга разного рода данных(например книги, или отзывы)</typeparam>
    public interface IExtractor<T>{
        /// <summary>
        /// Возвращает окончание работы Extractor'а
        /// </summary>
        /// <returns>True, если выкачка закончена</returns>
        public bool IsEndData();
        /// <summary>
        /// Метод для парсинга информации
        /// </summary>
        /// <returns>Класс ExtractBatchResult с информацией о парсинге и коллекцией записей парсинга</returns>
        public ExtractBatchResult<T> ExtractNextBatch();
    }
}
using Core.Object;

namespace Core
{
    /// <summary>
    /// Интерфейс, описывающий реализацию парсеров 
    /// </summary>
    public interface IExtractor <T> 
    {
        /// <summary>
        /// Возвращает окончание работы Extractor'а
        /// </summary>
        /// <returns>True, если выкачка закончена</returns>
        public bool IsEndData();
        /// <summary>
        /// Метод для парсинга информации
        /// </summary>
        /// <returns>Класс ExtractBatchResult с информацией о парсинге и коллекцией записей парсинга</returns>
        public Task<ExtractBatchResult<T>> ExtractNextBatch();

        // /// <summary>
        // /// Возвращает тип модели, в которую происходит выкачка данных
        // /// </summary>
        // public Type GetParsingModelType();
    }
}
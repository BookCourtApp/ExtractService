using Core.Models;

namespace Core.Extractor
{
    /// <summary>
    /// Интерфейс, описывающий реализацию парсеров
    /// </summary>
    /// <typeparam name="TRawData">Тип сырой информации</typeparam>
    /// <typeparam name="TOutputData">Тип выходной информации</typeparam>
    public interface IExtractor <TRawData, TOutputData> 
    {
        /// <summary>
        /// Метод который получает единицу сырой информации, например, html-документ с карточкой книги
        /// </summary>                                                                               
        /// <param name="resource">ресурс для извлечения, например, ссылка до карточки с книгой</param>
        /// <returns>единица сырой информации</returns>
        public Task<TRawData> GetRawDataAsync(ResourceInfo resource);
        
        /// <summary>
        /// Метод который обрабатывает единицу сырой информации:
        /// На вход дается, например, html-документ с карточкой книги;
        /// С помощью селекторов из неё извлекаются данные в модель и возвращаются
        /// </summary>
        /// <param name="data">единица обработанной информации</param>
        public Task<TOutputData> HandleAsync(TRawData data);
        
    }
}
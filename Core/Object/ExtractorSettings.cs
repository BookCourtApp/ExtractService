
namespace Core.Object
{
    /// <summary>
    /// Сущность для хранения настроек парсера
    /// </summary>
    public class ExtractorSettings
    {
        /// <summary>
        /// Ссылка для обращения к ресурсу с записями 
        /// </summary>
        public string URL { get; set; }

        /// <summary>
        /// Тип класса экстрактора, реализующий интерфейс IExtractor
        /// </summary>
        public Type ExtractorType { get; set; }

        /// <summary>
        /// Количество записей, которые парсятся за один заход парсером
        /// </summary>
        public int BatchCount { get; set; }
    }
}

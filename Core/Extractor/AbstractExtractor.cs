using Core.Object;

namespace ExtractorProject.Extractors
{
    /// <summary>
    /// Абстрактный базовый класс, который задает вектор разработки парсера.
    /// </summary>
    public abstract class AbstractExtractor
    {
        /// <summary>
        /// Поле для хранения настроек парсера и их последующего использования в них
        /// </summary>
        private readonly ExtractorSettings _settings;
        /// <summary>
        /// Конструктор для инициализации настроек парсера
        /// </summary>
        /// <param name="Settings">Поле для настроек парсера</param>
        public AbstractExtractor(ExtractorSettings Settings)
        {
            _settings = Settings;
        }
    }
}


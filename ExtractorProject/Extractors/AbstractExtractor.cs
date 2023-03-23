using ExtractorService.Models;

namespace ExtractorService.Extractor{
    public abstract class AbstractExtractor{
        /// <summary>
        /// Конструктор для инициализации парсера
        /// </summary>
        private readonly ExtractorSettings _settings;
        public AbstractExtractor(ExtractorSettings Settings){
            _settings = Settings;
        }
    }
}


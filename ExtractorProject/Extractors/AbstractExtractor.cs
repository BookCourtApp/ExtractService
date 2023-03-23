using ExtractorService.Models;

namespace ExtractorService.Extractor{
    public abstract class AbstractExtractor{
        /// <summary>
        /// Конструктор для инициализации парсера
        /// </summary>
        ExtractorSettings _settings{ get; set; }
        public AbstractExtractor(ExtractorSettings Settings){
            _settings = Settings;
        }
    }
}


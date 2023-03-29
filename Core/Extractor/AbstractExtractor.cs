using Core.Object;

namespace ExtractorProject.Extractors
{
    /// <summary>
    /// Абстрактный базовый класс, который задает вектор разработки парсера.
    /// </summary>
    public abstract class AbstractExtractor<RawDataT, ExtractorBooksDto>
    {
        abstract public RawDataT ExtractData();

        abstract public ExtractorBooksDto HandleData(RawDataT rawData);
    }
}


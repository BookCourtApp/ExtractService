

using AngleSharp.Dom;
using Core.Models;

namespace Core.Extractor
{
    /// <summary>
    /// Абстрактный базовый класс парсеров книг
    /// </summary>
    public abstract class BookExtractor : IExtractor<IDocument, Book>
    {
        /// <inheritdoc />
        abstract public IDocument GetRawData(ResourceInfo info);

        /// <inheritdoc />
        abstract public Book Handle(IDocument rawData);
    }
}


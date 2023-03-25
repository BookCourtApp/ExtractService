namespace Core;

/// <summary>
/// интерфейс ExtractorWorker
/// </summary>
public interface IExtractorWorker
{
    /// <summary>
    /// Запуск Extractor по заданным условиям
    /// </summary>
    /// <returns>Возвращает результат работы с условиями</returns>
    public void ExtractData(string setings);




}
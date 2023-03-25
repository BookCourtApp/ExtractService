using Core.Object;

namespace Core;

/// <summary>
/// интерфейс ExtractorWorker
/// </summary>
public interface IExtractorWorker
{
    /// <summary>
    /// Запуск Extractor по заданным условиям
    /// </summary>
    /// <param name="settings">Настройки экстрактора</param>
    /// <typeparam name="T">T тип выкачиваемой модели</typeparam>
    /// <returns>Возвращает результат работы экстрактора</returns>
    public Task ExtractDataAsync<T>(ExtractorSettings settings);




}
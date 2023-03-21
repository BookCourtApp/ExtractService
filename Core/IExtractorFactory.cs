namespace Core;

/// <summary>
/// интерфейс фабрики для экстрактора
/// </summary>
public interface IExtractorFactory
{
    /// <summary>
    /// Создание экстрактора по настройкам
    /// </summary>
    /// <param name="extractorSettings">Настройки для создания экстрактора</param>
    /// <returns>Экстрактор с определенным типом</returns>
    public IExtractor CreateExtractor(string extractorSettings); // todo: заменить string на норм типы
}
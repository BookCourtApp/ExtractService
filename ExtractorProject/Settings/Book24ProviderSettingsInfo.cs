using Core.Settings;

namespace ExtractorProject.Settings;

/// <summary>
/// Настройки для провайдера ресурсов для Book24
/// </summary>
public class Book24ProviderSettingsInfo : IProviderSettingsInfo
{
    /// <summary>
    /// Страница с пейджами без номера
    /// </summary>
    public string Catalog {get;set;}
    /// <summary>
    /// Стартовая страница для парсинга
    /// </summary>
    public int MinPage { get; set; }
    /// <summary>
    /// Конечная страница для парсинга
    /// </summary>
    public int MaxPage { get; set; }
}
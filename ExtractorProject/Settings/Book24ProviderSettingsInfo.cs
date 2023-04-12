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
    /// Номер страницы для старта парсинга
    /// </summary>
    public int MinPage { get; set; }
    /// <summary>
    /// Номер страницы для конца парсинга
    /// </summary>
    public int MaxPage { get; set; }
}
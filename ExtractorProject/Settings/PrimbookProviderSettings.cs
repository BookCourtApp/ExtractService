using Core.Settings;

namespace ExtractorProject.Settings;

/// <summary>
/// Настройки для провайдера ресурсов Primbook
/// </summary>
public class PrimbookProviderSettings : IProviderSettingsInfo
{
    /// <summary>
    /// массив url до каталогов с готовым параметром Page
    /// </summary>
    public string[] CatalogUrls { get; set; }
}
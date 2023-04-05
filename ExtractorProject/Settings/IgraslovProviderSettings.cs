using Core.Settings;

namespace ExtractorProject.Settings;

/// <summary>
/// настройки  для провайдера игры слов
/// </summary>
public class IgraSlovProviderSettings : IProviderSettingsInfo
{
    /// <summary>
    /// сслыка на каталог всех книг
    /// </summary>
    public string Url { get; set; }

    
}
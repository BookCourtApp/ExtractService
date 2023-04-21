using Core.Settings;

namespace BusinessLogin.ExtTask;

/// <summary>
/// модель задачи для экстрактора
/// </summary>
public class ExtractorTask
{
    /// <summary>
    /// тип класса экстрактора
    /// </summary>
    public Type ExtractorType { get; set; }

    /// <summary>
    /// тип класса провайдера
    /// </summary>
    public Type ResourceProviderType { get; set; }

    /// <summary>
    /// настройки провайдера
    /// </summary>
    public IProviderSettingsInfo ProviderSettings { get; set; }
}
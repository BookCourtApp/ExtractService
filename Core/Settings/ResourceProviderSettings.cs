namespace Core.Settings;

public class ResourceProviderSettings
{
    /// <summary>
    /// url до страницы откуда брать UrlResources
    /// </summary>
    public string Site { get; set; }
    
    /// <summary>
    /// информация для провайдера ресурсов, детерменирована в зависимости от назначения провайдера
    /// </summary>
    public IProviderSettingsInfo Info { get; set; }
}
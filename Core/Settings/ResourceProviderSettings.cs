namespace Core.Settings;

public class ResourceProviderSettings
{
    /// <summary>
    /// url до страницы откуда брать UrlResources
    /// </summary>
    public string Site { get; set; }

    public ISettingsInfo Info { get; set; }
}
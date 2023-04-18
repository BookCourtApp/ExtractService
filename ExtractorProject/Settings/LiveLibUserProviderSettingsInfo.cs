using Core.Settings;

namespace ExtractorProject.Settings;

public class LiveLibUserProviderSettingsInfo : IProviderSettingsInfo
{
    public string CatalogUrl { get; set; }
    
    public int MinId { get; set; }

    public int MaxId { get; set; }
}
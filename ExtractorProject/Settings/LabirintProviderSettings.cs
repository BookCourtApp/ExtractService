using Core.Settings;

namespace ExtractorProject.Settings;

public class LabirintProviderSettings : IProviderSettingsInfo
{   
    public int MinId { get; set; }

    public int MaxId { get; set; }
}
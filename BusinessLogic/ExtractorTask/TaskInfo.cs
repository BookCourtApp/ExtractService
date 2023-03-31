using Core.Settings;

namespace BusinessLogin.ExtractorTask;

public class TaskInfo
{
    public Type ExtractorType { get; set; }

    public Type ResourceProviderType { get; set; }

    public ResourceProviderSettings ProviderSettings { get; set; }
}
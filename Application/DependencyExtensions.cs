using ExtractorProject.Extractors;
using ExtractorProject.ResourceProvider;
using ExtractorProject.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyExtensions
{
    public static IServiceCollection AddExtractors(this IServiceCollection services)
    {
        services.AddSingleton<LabirintResourceInfoProvider>();
        services.AddSingleton<ExtractorLabirint>();
        services.AddSingleton<LabirintResourceInfoProvider>();
        services.AddSingleton<LiveLibResourceInfoProvider>();
        services.AddSingleton<ExtractorLiveLib>();
        services.Configure<LiveLibProviderSettingsInfo>(config => new LiveLibProviderSettingsInfo());
        return services;
    }
}
using ExtractorProject.Extractors;
using ExtractorProject.ResourceProvider;
using ExtractorProject.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyExtensions
{
    public static IServiceCollection AddExtractors(this IServiceCollection services, IConfiguration config)
    {
        
        services.AddSingleton<LabirintResourceInfoProvider>();
        services.AddSingleton<ExtractorLabirint>();
        services.AddSingleton<LiveLibUserExtractor>();
        services.AddSingleton<LiveLibUserPreferenceExtractor>();
        services.AddSingleton<LiveLibUserPreferenceExtractorJson>();
        services.Configure<LabirintProviderSettings>(config.GetSection(nameof(LabirintProviderSettings)));
        
        services.Configure<LiveLibUserExtractorSettingsInfo>(config.GetSection(nameof(LiveLibUserExtractorSettingsInfo)));
        services.Configure<LiveLibUserPreferenceExtractorSettingsInfo>(config.GetSection(nameof(LiveLibUserPreferenceExtractorSettingsInfo)));
        return services;
    }
}
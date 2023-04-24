using ExtractorProject.Extractors;
using ExtractorProject.ResourceProvider;
using ExtractorProject.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
namespace Application;

public static class DependencyExtensions
{
    public static IServiceCollection AddExtractors(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<LabirintResourceInfoProvider>();
        services.AddSingleton<ExtractorLabirint>();
        //services.AddSingleton<LabirintResourceInfoProvider>();
        services.AddSingleton<LiveLibResourceInfoProvider>();
        //services.AddSingleton<LiveLibResourceBookProviderFromRepo>();
        services.AddSingleton<ExtractorLiveLib>();
        var res = configuration.GetSection("LiveLibProviderSettingsInfo").Get<LiveLibProviderSettingsInfo>();
        
        //services.Configure<LiveLibProviderSettingsInfo>(config => configuration.GetSection(nameof(LiveLibProviderSettingsInfo)));
        
        return services;
    }
}
using ExtractorProject.Extractors;
using ExtractorProject.ResourceProvider;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyExtensions
{
    public static IServiceCollection AddExtractors(this IServiceCollection services)
    {
        services.AddSingleton<LabirintResourceInfoProvider>();
        services.AddSingleton<ExtractorLabirint>();
        services.AddSingleton<LabirintResourceInfoProvider>();
        return services;
    }
}
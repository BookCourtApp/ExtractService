using System.Reflection;
using Core.Settings;
using ExtractorProject;
using ExtractorProject.Settings;
using Microsoft.Extensions.Configuration;

namespace BusinessLogin.ExtTask;

/// <summary>
/// Фабрика задач для экстракторов
/// </summary>
public class ExtractorTaskFactory
{
    private readonly IConfiguration _configuration;
    private const string ResourceProvidersNamespace = "ExtractorProject.ResourceProvider";
    private const string ExtractorsNamespace = "ExtractorProject.Extractors";
    private const string ResourceProvidersSettingsNamespace = "ExtractorProject.Settings";
    private Assembly _assemblyExtractorProject;
    public ExtractorTaskFactory(IConfiguration configuration)
    {
        _configuration = configuration;
        _assemblyExtractorProject = typeof(ReflectionHelper).Assembly;
    }
    
    /// <summary>
    /// Создание задачи для экстрактора
    /// </summary>
    /// <param name="providerTypeStr">название типа провайдера</param>
    /// <param name="extractorTypeStr">название типа экстрактора</param>
    /// <param name="providerSettingsTypeStr">название типа настроек провайдера</param>
    /// <returns>Созданную задачу для экстрактора</returns>
    public ExtractorTask CreateExtractorTask(string providerTypeStr, string extractorTypeStr, string providerSettingsTypeStr)
    {
        var providerType = _assemblyExtractorProject.GetType($"{ResourceProvidersNamespace}.{providerTypeStr}");
        var extractorType = _assemblyExtractorProject.GetType($"{ExtractorsNamespace}.{extractorTypeStr}");
        var settingsType = _assemblyExtractorProject.GetType($"{ResourceProvidersSettingsNamespace}.{providerSettingsTypeStr}");
        var settings = _configuration.GetSection(providerSettingsTypeStr).Get(settingsType) as IProviderSettingsInfo;  //todo: возможно стоит перенести это в какой-нибудь провайдер
        var task = new ExtractorTask()
        {
            ExtractorType = extractorType,
            ResourceProviderType = providerType,
            ProviderSettings = settings
        };
        return task;
    }

    /// <summary>
    /// Получение списка названий типов экстракторов
    /// </summary>
    public IEnumerable<string> GetExtractors()
    {
        return _assemblyExtractorProject
            .GetTypes()
            .Where(t => t.Namespace == ExtractorsNamespace)
            .Select(t => t.Name);
    }
    
    /// <summary>
    /// Получение списка названий типов провайдеров
    /// </summary>
    public IEnumerable<string> GetProviders()
    {
        return _assemblyExtractorProject
            .GetTypes()
            .Where(t => t.Namespace == ResourceProvidersNamespace)
            .Select(t => t.Name);
    }
    
    /// <summary>
    /// Получение списка названий типов настроек для провайдеров
    /// </summary>
    public IEnumerable<string> GetSettings()
    {
        return _assemblyExtractorProject
            .GetTypes()
            .Where(t => t.Namespace == ResourceProvidersSettingsNamespace)
            .Select(t => t.Name);
    }
}
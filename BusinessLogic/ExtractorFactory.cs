using AngleSharp.Dom;
using Core.Extractor;
using Core.Models;
using Core.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLogin;

/// <summary>
/// Класс фабрики для создания экстрактора или провайдера
/// </summary>
public class ExtractorFactory
{
    private readonly IServiceProvider _services;

    public ExtractorFactory(IServiceProvider services)
    {
        _services = services;
    }
    
    /// <summary>
    /// Создание провайдера ресурсов для экстрактора
    /// </summary>
    /// <param name="settings">настройки для конкретного типа провайдера</param>
    /// <param name="providerType">Тип данных провайдера</param>
    /// <returns>Инкапсулированный провайдер</returns>
    /// <exception cref="NullReferenceException">когда не удалось создать нужный провайдер</exception>
    public IResourceInfoProvider GetResourceInfoProvider(ResourceProviderSettings settings, Type providerType)
    {
        var provider = ActivatorUtilities.CreateInstance(_services, providerType, settings) as IResourceInfoProvider;
        if (provider is null)
            throw new NullReferenceException($"{nameof(providerType)} can not be created");
        return provider;
    }
    
    /// <summary>
    /// Создание экстрактора
    /// </summary>
    /// <param name="extractorType">Тип данных экстрактора</param>
    /// <returns>Инкапсулированный экстрактор</returns>
    /// <exception cref="NullReferenceException">когда не удалось создать нужный экстрактор</exception>
    public IExtractor<IDocument, Book> GetBookExtractor(Type extractorType)
    {
        var extractor = ActivatorUtilities.CreateInstance(_services, extractorType) as IExtractor<IDocument, Book>;
        if (extractor is null)
            throw new NullReferenceException($"{nameof(extractorType)} can not be created");
        return extractor;
    }
}
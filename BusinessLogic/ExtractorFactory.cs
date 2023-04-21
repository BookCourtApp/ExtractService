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
    public IResourceInfoProvider GetResourceInfoProvider(IProviderSettingsInfo settings, Type providerType)
    {
        IResourceInfoProvider provider;
        try
        { 
            provider = ActivatorUtilities.CreateInstance(_services, providerType) as IResourceInfoProvider;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
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
        IExtractor<IDocument, Book> provider;
        var extractor = ActivatorUtilities.CreateInstance(_services, extractorType) as IExtractor<IDocument, Book>;
        if (extractor is null)
            throw new NullReferenceException($"{nameof(extractorType)} can not be created");
        return extractor;
    }
}
using Core.Models;
using Core.Settings;

namespace Core.Extractor;

/// <summary>
/// Интерфейс для провайдеров ресурсов экстрактора
/// </summary>
public interface IResourceInfoProvider
{
    /// <summary>
    /// Метод возвращающий перечислимую коллекцию ресурсов экстрактора
    /// </summary>
    public abstract IEnumerable<ResourceInfo> GetResources();
}
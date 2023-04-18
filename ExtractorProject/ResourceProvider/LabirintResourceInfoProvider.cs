﻿using Core.Extractor;
using Core.Models;
using Core.Settings;
using ExtractorProject.Settings;
using Microsoft.Extensions.Options;

namespace ExtractorProject.ResourceProvider;

/// <summary>
/// провайдер ресурсов для лабиринта
/// </summary>
public class LabirintResourceInfoProvider : IResourceInfoProvider
{
    private readonly string _catalogUrl;
    private readonly int _minId;
    private readonly int _maxId;

    public LabirintResourceInfoProvider(IOptions<LabirintProviderSettings> settings)
    {
        _catalogUrl = settings.Value.CatalogUrl;
        _minId = settings.Value.MinId;
        _maxId = settings.Value.MaxId;
    }


    /// <inheritdoc />
    public IEnumerable<ResourceInfo> GetResources()
    {
        for (int i = _minId; i < _maxId; i++)
        {
            var resourceUrl = new ResourceInfo() { URLResource = _catalogUrl + i };
            yield return resourceUrl;
        }
    }
}
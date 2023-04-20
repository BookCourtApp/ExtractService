﻿using Core.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractorProject.Settings;

/// <summary>
/// Настройки для провайдера LiveLib
/// </summary>
public class LiveLibProviderSettingsInfo : IProviderSettingsInfo
{
    public List<string> CategoriesURL { get; set; } = new List<string>()
    {
        "https://www.livelib.ru/genre/%D0%91%D0%B8%D0%B7%D0%BD%D0%B5%D1%81-%D0%BA%D0%BD%D0%B8%D0%B3%D0%B8",
        "https://www.livelib.ru/genre/%D0%9A%D0%BB%D0%B0%D1%81%D1%81%D0%B8%D1%87%D0%B5%D1%81%D0%BA%D0%B0%D1%8F-%D0%BB%D0%B8%D1%82%D0%B5%D1%80%D0%B0%D1%82%D1%83%D1%80%D0%B0",
        "https://www.livelib.ru/genre/%D0%97%D0%B0%D1%80%D1%83%D0%B1%D0%B5%D0%B6%D0%BD%D0%B0%D1%8F-%D0%BB%D0%B8%D1%82%D0%B5%D1%80%D0%B0%D1%82%D1%83%D1%80%D0%B0",
        "https://www.livelib.ru/genre/%D0%A0%D1%83%D1%81%D1%81%D0%BA%D0%B0%D1%8F-%D0%BB%D0%B8%D1%82%D0%B5%D1%80%D0%B0%D1%82%D1%83%D1%80%D0%B0",
        "https://www.livelib.ru/genre/%D0%94%D0%B5%D1%82%D1%81%D0%BA%D0%B8%D0%B5-%D0%BA%D0%BD%D0%B8%D0%B3%D0%B8",
        "https://www.livelib.ru/genre/%D0%94%D0%B5%D1%82%D0%B5%D0%BA%D1%82%D0%B8%D0%B2%D1%8B",
        "https://www.livelib.ru/genre/%D0%A4%D1%8D%D0%BD%D1%82%D0%B5%D0%B7%D0%B8",
        "https://www.livelib.ru/genre/%D0%A4%D0%B0%D0%BD%D1%82%D0%B0%D1%81%D1%82%D0%B8%D0%BA%D0%B0",
        "https://www.livelib.ru/genre/%D0%A1%D0%BE%D0%B2%D1%80%D0%B5%D0%BC%D0%B5%D0%BD%D0%BD%D0%B0%D1%8F-%D0%BF%D1%80%D0%BE%D0%B7%D0%B0",
        "https://www.livelib.ru/genre/%D0%9F%D1%80%D0%B8%D0%BA%D0%BB%D1%8E%D1%87%D0%B5%D0%BD%D0%B8%D1%8F",
        "https://www.livelib.ru/genre/%D0%A3%D0%B6%D0%B0%D1%81%D1%8B-%D0%BC%D0%B8%D1%81%D1%82%D0%B8%D0%BA%D0%B0",
        "https://www.livelib.ru/genre/%D0%9F%D1%83%D0%B1%D0%BB%D0%B8%D1%86%D0%B8%D1%81%D1%82%D0%B8%D1%87%D0%B5%D1%81%D0%BA%D0%B0%D1%8F-%D0%BB%D0%B8%D1%82%D0%B5%D1%80%D0%B0%D1%82%D1%83%D1%80%D0%B0",
        "https://www.livelib.ru/genre/%D0%9A%D0%BD%D0%B8%D0%B3%D0%B8-%D0%B4%D0%BB%D1%8F-%D0%BF%D0%BE%D0%B4%D1%80%D0%BE%D1%81%D1%82%D0%BA%D0%BE%D0%B2",
        "https://www.livelib.ru/genre/%D0%9B%D1%8E%D0%B1%D0%BE%D0%B2%D0%BD%D1%8B%D0%B5-%D1%80%D0%BE%D0%BC%D0%B0%D0%BD%D1%8B",
        "https://www.livelib.ru/genre/%D0%91%D0%BE%D0%B5%D0%B2%D0%B8%D0%BA%D0%B8-%D0%BE%D1%81%D1%82%D1%80%D0%BE%D1%81%D1%8E%D0%B6%D0%B5%D1%82%D0%BD%D0%B0%D1%8F-%D0%BB%D0%B8%D1%82%D0%B5%D1%80%D0%B0%D1%82%D1%83%D1%80%D0%B0",
        "https://www.livelib.ru/genre/%D0%9A%D0%BD%D0%B8%D0%B3%D0%B8-%D0%BF%D0%BE-%D0%BF%D1%81%D0%B8%D1%85%D0%BE%D0%BB%D0%BE%D0%B3%D0%B8%D0%B8",
        "https://www.livelib.ru/genre/%D0%9F%D0%BE%D0%B2%D0%B5%D1%81%D1%82%D0%B8-%D1%80%D0%B0%D1%81%D1%81%D0%BA%D0%B0%D0%B7%D1%8B",
        "https://www.livelib.ru/genre/%D0%9F%D0%BE%D1%8D%D0%B7%D0%B8%D1%8F-%D0%B8-%D0%B4%D1%80%D0%B0%D0%BC%D0%B0%D1%82%D1%83%D1%80%D0%B3%D0%B8%D1%8F",
        "https://www.livelib.ru/genre/%D0%9D%D0%B0%D1%83%D0%BA%D0%B0-%D0%B8-%D0%BE%D0%B1%D1%80%D0%B0%D0%B7%D0%BE%D0%B2%D0%B0%D0%BD%D0%B8%D0%B5",
        "https://www.livelib.ru/genre/%D0%94%D0%BE%D0%BC-%D1%81%D0%B5%D0%BC%D1%8C%D1%8F-%D1%85%D0%BE%D0%B1%D0%B1%D0%B8-%D0%B8-%D0%B4%D0%BE%D1%81%D1%83%D0%B3",
        "https://www.livelib.ru/genre/%D0%9A%D0%BE%D0%BC%D0%B8%D0%BA%D1%81%D1%8B-%D0%BC%D0%B0%D0%BD%D0%B3%D0%B0-%D0%B3%D1%80%D0%B0%D1%84%D0%B8%D1%87%D0%B5%D1%81%D0%BA%D0%B8%D0%B5-%D1%80%D0%BE%D0%BC%D0%B0%D0%BD%D1%8B",
        "https://www.livelib.ru/genre/%D0%AD%D0%B7%D0%BE%D1%82%D0%B5%D1%80%D0%B8%D0%BA%D0%B0",
        "https://www.livelib.ru/genre/%D0%9A%D1%83%D0%BB%D1%8C%D1%82%D1%83%D1%80%D0%B0-%D0%B8-%D0%B8%D1%81%D0%BA%D1%83%D1%81%D1%81%D1%82%D0%B2%D0%BE",
        "https://www.livelib.ru/genre/%D0%AE%D0%BC%D0%BE%D1%80%D0%B8%D1%81%D1%82%D0%B8%D1%87%D0%B5%D1%81%D0%BA%D0%B0%D1%8F-%D0%BB%D0%B8%D1%82%D0%B5%D1%80%D0%B0%D1%82%D1%83%D1%80%D0%B0",
        "https://www.livelib.ru/genre/%D0%A0%D0%B5%D0%BB%D0%B8%D0%B3%D0%B8%D1%8F",
        "https://www.livelib.ru/genre/%D0%A1%D0%BB%D0%BE%D0%B2%D0%B0%D1%80%D0%B8-%D1%81%D0%BF%D1%80%D0%B0%D0%B2%D0%BE%D1%87%D0%BD%D0%B8%D0%BA%D0%B8",
        "https://www.livelib.ru/genre/%D0%9A%D1%80%D0%B0%D1%81%D0%BE%D1%82%D0%B0-%D0%B8-%D0%B7%D0%B4%D0%BE%D1%80%D0%BE%D0%B2%D1%8C%D0%B5",
        "https://www.livelib.ru/genre/%D0%9A%D0%BD%D0%B8%D0%B3%D0%B8-%D0%BD%D0%B0-%D0%B8%D0%BD%D0%BE%D1%81%D1%82%D1%80%D0%B0%D0%BD%D0%BD%D1%8B%D1%85-%D1%8F%D0%B7%D1%8B%D0%BA%D0%B0%D1%85",
        "https://www.livelib.ru/genre/%D0%9A%D0%BE%D0%BC%D0%BF%D1%8C%D1%8E%D1%82%D0%B5%D1%80%D0%BD%D0%B0%D1%8F-%D0%BB%D0%B8%D1%82%D0%B5%D1%80%D0%B0%D1%82%D1%83%D1%80%D0%B0",
        "https://www.livelib.ru/genre/%D0%AD%D1%80%D0%BE%D1%82%D0%B8%D0%BA%D0%B0-%D0%B8-%D1%81%D0%B5%D0%BA%D1%81",
        "https://www.livelib.ru/genre/%D0%9F%D0%B5%D1%80%D0%B8%D0%BE%D0%B4%D0%B8%D1%87%D0%B5%D1%81%D0%BA%D0%B8%D0%B5-%D0%B8%D0%B7%D0%B4%D0%B0%D0%BD%D0%B8%D1%8F",
        "https://www.livelib.ru/genre/%D0%A3%D1%87%D0%B5%D0%B1%D0%BD%D0%B0%D1%8F-%D0%BB%D0%B8%D1%82%D0%B5%D1%80%D0%B0%D1%82%D1%83%D1%80%D0%B0",
        "https://www.livelib.ru/genre/%D0%98%D1%81%D1%82%D0%BE%D1%80%D0%B8%D1%87%D0%B5%D1%81%D0%BA%D0%B8%D0%B9-%D1%80%D0%BE%D0%BC%D0%B0%D0%BD",
        "https://www.livelib.ru/genre/%D0%9C%D0%B0%D0%B3%D0%B8%D1%87%D0%B5%D1%81%D0%BA%D0%B8%D0%B9-%D1%80%D0%B5%D0%B0%D0%BB%D0%B8%D0%B7%D0%BC",
        "https://www.livelib.ru/genre/%D0%A0%D0%B5%D0%B0%D0%BB%D0%B8%D0%B7%D0%BC",
        "https://www.livelib.ru/genre/%D0%98%D1%81%D1%82%D0%BE%D1%80%D0%B8%D1%87%D0%B5%D1%81%D0%BA%D0%B0%D1%8F-%D0%BF%D1%80%D0%BE%D0%B7%D0%B0",
        "https://www.livelib.ru/genre/%D0%90%D1%80%D1%82%D0%B1%D1%83%D0%BA%D0%B8",
        "https://www.livelib.ru/genre/%D0%9F%D1%80%D0%B8%D1%82%D1%87%D0%B8"
    };
}

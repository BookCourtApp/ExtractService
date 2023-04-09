using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Io;
using Core.Extractor;
using Core.Models;
using Core.Settings;
using ExtractorProject.Settings;
using System.Net;

namespace ExtractorProject.ResourceProvider;

/// <summary>
/// провайдер ресурсов для Book24
/// </summary>
public class Book24ResourceInfoProvider : IResourceInfoProvider
{
    private readonly string _catalogUrl;
    private readonly int _minPage;
    private readonly int _maxPage;

    public Book24ResourceInfoProvider(ResourceProviderSettings settings)
    {
        Book24ProviderSettingsInfo providerSettingsInfo = settings.Info as Book24ProviderSettingsInfo
                                       ?? throw new NullReferenceException($"{nameof(settings)} не подходит для итератора лабиринта");
        _catalogUrl = providerSettingsInfo.Catalog;
        _minPage = providerSettingsInfo.MinPage;
        _maxPage = providerSettingsInfo.MaxPage;
    }


    /// <inheritdoc />
    public IEnumerable<ResourceInfo> GetResources()
    {
        for (int i = _minPage; i < _maxPage; i++)
        {
            var document = GetDocument(_catalogUrl + i);
            if(document == null)
                continue;

            if(document.StatusCode == (HttpStatusCode)520)
                Console.WriteLine("HEADER WAS BANNED ON " + i);

            var BookPages = document
                .GetElementsByClassName("product-list__item");
            
            if ((BookPages.Length == 0) && BookPages == null)
            {
                Console.WriteLine($"Нет книг на странице");
                continue;
            }

            foreach(var BookPage in BookPages){
                string BookPageUrl;
                try{
                    BookPageUrl = "https://book24.ru" + BookPage
                        .GetElementsByClassName("product-card__image-holder")[0]
                        .Children[0]
                        .Attributes["href"]
                        .Value;

                }
                catch(Exception ex){
                    Console.WriteLine("Error was occured while getting BookPage");
                    continue;
                }
                var resourceUrl = new ResourceInfo() { URLResource = BookPageUrl};
                yield return resourceUrl;
            }

        }
    }

    /// <summary>
    /// Функция для скачивания страницы с использованием прокси 
    /// </summary>
    /// <param name="url">Ссылка на страницу для скачивания</param>
    /// <returns></returns>
    public static IDocument? GetDocument(string url)
    {
        try{
            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);
            //var requestMessage = new HttpRequestMessage(System.Net.Http.HttpMethod.Get, url);
            //requestMessage.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:58.0) Gecko/20100101 Firefox/58.0");
            var AngleSharpUrl = new Url(url);
            var request = DocumentRequest.Get(AngleSharpUrl);
            
            request.Headers["User-Agent"] = "Mozilla/4.0 (compatible; MSIE 7.0; AOL 9.1; AOLBuild 4334.27; Windows NT 6.0; WOW64; SLCC1; .NET CLR 2.0.50727; .NET CLR 3.0.04506; Media Center PC 5.0); UnAuth-State";
            request.Headers["Accept"] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            request.Headers["Accept-Language"] = "en-US,en;q=0.9";
            request.Headers["Referer"] = "http://google.com";
            request.Headers["Connection"] = "keep-alive";
            return context.OpenAsync(request).Result;
        }
        catch(Exception ex){
            Console.WriteLine($"Error while getting document: {ex.Message}");
        }
        return null;
    }
}
using AngleSharp;
using AngleSharp.Dom;
using Core.Extractor;
using Core.Models;
using Core.Settings;
using ExtractorProject.Settings;
using System.Net;

namespace ExtractorProject.ResourceProvider;

public class Vault14ResourceInfoProvider : IResourceInfoProvider
{
    private readonly List<string> _categoriesURL;

    public Vault14ResourceInfoProvider(ResourceProviderSettings settings)
    {
        Vault14ProviderSettings providerSettingsInfo = settings.Info as Vault14ProviderSettings
                                       ?? throw new NullReferenceException($"{nameof(settings)} не подходит для итератора лабиринта");
        _categoriesURL = providerSettingsInfo.CategoriesURL;
    }

    private IDocument GetHTMLPage(string URL)
    {
        var config = Configuration.Default.WithDefaultLoader();
        var context = BrowsingContext.New(config);
        var page = context.OpenAsync(URL).Result;
        if (page.StatusCode != HttpStatusCode.NotFound)
        {
            return page;
        }
        else
            return null;
    }
    public IEnumerable<ResourceInfo> GetResources()
    {
        foreach (var category in _categoriesURL) 
        {
            IDocument document;
            IHtmlCollection<IElement> divnum = null;
            int numberOfPages = 0;
            try
            {
                document = GetHTMLPage(category);
                if (document == null)
                {
                    continue;
                }
                divnum = document.GetElementsByClassName("pagination-item");
            }
            catch (Exception e)
            {
                //Console.WriteLine(e);
            }
            try
            {
                numberOfPages = Int32.Parse(divnum[divnum.Length - 1].TextContent.Trim());
            }
            catch (Exception e)
            {
                //Console.WriteLine(e);
            }
            for (int i = 1; i <= numberOfPages; i++)
            {
                var pageURL = category + "?page=" + i.ToString();
                IDocument linkedDocument = null;
                try
                {
                    linkedDocument = GetHTMLPage(pageURL);
                }
                catch (Exception e) 
                {
                    //Console.WriteLine(e);
                }
                var booksFromPageURL = linkedDocument.GetElementsByClassName("product-card-photo image-container is-square");
                foreach(var book in booksFromPageURL)
                {
                    var resourceUrl = new ResourceInfo() { URLResource = "https://vault14.ru" + book.Attributes["href"].Value };
                    yield return resourceUrl;
                }

                  
               // yield return resourceUrl;
            }
        }
    }
}

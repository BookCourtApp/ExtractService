using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using AngleSharp;
using AngleSharp.Dom;
using BookParser;
using Newtonsoft.Json;

namespace Parser;

public class ExtractorBook24
{
    private const string Aut = "Автор";
    private const string Isd = "Страниц";
    private bool EndDate = false;

    public static IDocument GetDocument(string url)
    {
        var config = Configuration.Default.WithDefaultLoader();
        var context = BrowsingContext.New(config);
        return context.OpenAsync(url).Result;
    }

    
    private async Task<List<Book>> ParseBookInfo(string urlWithCollection, int startPage, int endPage)
    {
        Console.WriteLine($"Started thread since {startPage} to {endPage}");
        var books = new List<Book>();
        DateTime nowThread = DateTime.Now;
        for (int i = startPage; i <= endPage; i++)
        {
            try
            {
                DateTime now = DateTime.Now;
                var document2 = GetDocument(urlWithCollection + Convert.ToString(i));


                var textWitHResultSearchElements =
                    document2.GetElementsByClassName("product-list__item");

                if ((textWitHResultSearchElements.Length == 0))
                {
                    Console.WriteLine($"Page - {i}: Книги не найдены");
                    continue;
                }
                Console.WriteLine($"Page - {i} was readed, count = {textWitHResultSearchElements.Length}");
                // Console.WriteLine("Page - "+(i+1));
                foreach (var bookFromList in textWitHResultSearchElements)
                {
                    var book = ParseICollection(bookFromList);
                    books.Add(book);
                    Console.WriteLine(book.Name + ' ' + book.Author);
                    //Console.WriteLine($"Got book -  {book.Name}; Price - {book.Price}; Remainder - {book.Remainder}");
                    //Console.WriteLine();
                }
                DateTime end = DateTime.Now;
                Console.WriteLine($"Page - {i} was parsed, count in thread = {books.Count}; time - {new TimeSpan((end - now).Ticks).TotalMinutes} min");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"{DateTime.Now} : ERROR for page- {ex.Message}");
            }
        }
        DateTime endThread = DateTime.Now;
        Console.WriteLine($"Thread parsed all pages.  time - {new TimeSpan((endThread - nowThread).Ticks).TotalMinutes} min");

        return books;
    }

    private Book ParseICollection(IElement element)
    {
            var refToBook = "https://book24.ru" + element.GetElementsByClassName("product-card__image-holder")[0].Children[0]
            .Attributes["href"].Value;
            Console.WriteLine("AddToRef is " + refToBook);
            var BookInfo = GetDocument(refToBook);
        var category = new List<string>() { " Книги с автографом ", " Художественная литература ", " Детские книги ", " Книги для подростков ", " Бизнес-литература ", " Самообразование и развитие ", " Хобби и досуг ", " Учебная литература ", " Педагогика и воспитание ", " Научно-популярная литература ", " Публицистика ", " Религия ", " Эксклюзивная продукция ", " Книги в кожаном переплете ", " Книжный развал ", " Букинистика и антикварные издания ", "" };
        //var ff = BookInfo.GetElementsByClassName("breadcrumbs__link smartLink")[1].Children[0].TextContent;
        foreach(var i in category)
        {
            if(i == BookInfo.GetElementsByClassName("breadcrumbs__link smartLink")[1].Children[0].TextContent)
            {

        
            String Name = "";
            int Remainder = 0;
            int Price = 0;
            String Description = "";
            String Author = "";
            String Genre = "";
            String Image = "";
            String NumberOfPages = "";
            String CoverType = "";
            String Publisher = "";
            String ISBN = "";
            String PublisherYear = "";
            String Series = "";
            String AgeRestrictions = "";
            String Format = "";
            String Weight = "";
            int Sales = 0;
            String Reviews = "";
            String Rating = "";
            String VendorCode = "";
            DateTime ParsingDate = DateTime.Now;


            //var prop = element.GetElementsByClassName("author-list product-card__authors-holder")[0].Children[0].TextContent;

            try
            {


                Name = BookInfo.GetElementsByClassName("product-detail-page__title")[0].TextContent;


                //var rew = BookInfo.GetElementsByClassName("reviews-widget__main-text")[0].TextContent.Replace(" ", "");
                Reviews = BookInfo.GetElementsByClassName("reviews-widget__main-text")[0].TextContent.Replace(" ", "");


                //var reit = BookInfo.GetElementsByClassName("rating-widget__main-text")[0].TextContent.Replace(" ", "");

                Rating = BookInfo.GetElementsByClassName("rating-widget__main-text")[0].TextContent.Replace(" ", "");


                VendorCode = BookInfo.GetElementsByClassName("product-detail-page__article")[0].TextContent.Replace("Артикул: ", "").Replace(" ", "");


                //var PricE = BookInfo.GetElementsByClassName("app-price product-sidebar-price__price")[0].TextContent.Replace(" ₽", "").Replace(" ", "");
                Price = Int32.Parse(BookInfo.GetElementsByClassName("app-price product-sidebar-price__price")[0].TextContent.Replace(" ₽", "").Replace(" ", "").Replace(" ", ""));

                //var sel = BookInfo.GetElementsByClassName("product-detail-page__purchased-text")[0].TextContent.Split(' ')[2];
                Sales = Int32.Parse(BookInfo.GetElementsByClassName("product-detail-page__purchased-text")[0].TextContent.Split(' ')[2]);


                Description = BookInfo.GetElementsByClassName("product-about__text")[0].TextContent
                    .Replace("\n", "");

                var properties = BookInfo.GetElementsByClassName("product-characteristic__item").Where(p =>
                {
                    if (p.GetElementsByClassName("product-characteristic__label-holder").Length > 0)
                        return true;
                    return false;
                });
                foreach (var Prop in properties)
                {
                    var n = Prop.GetElementsByTagName("span")[0].TextContent;

                    var m = Prop.Children[1].TextContent.Replace("Издательство", "").Replace(" ", "");

                    switch (n)
                    {
                        case " Автор: ":
                            Author = m;
                            break;

                        case " Серия: ":
                            Series = m;
                            break;

                        case " Раздел: ":
                            Genre = m;
                            break;

                        case " Издательство: ":
                            Publisher = m;
                            break;

                        case " ISBN: ":
                            ISBN = m;
                            break;

                        case " Возрастное ограничение: ":
                            AgeRestrictions = m;
                            break;

                        case " Год издания: ":
                            PublisherYear = m;
                            break;

                        case " Переплет: ":
                            CoverType = m;
                            break;

                        case " Формат: ":
                            Format = m;
                            break;

                        case " Вес: ":
                            Weight = m;
                            break;

                        case " Количество страниц: ":
                            NumberOfPages = m;
                            break;


                    }
                }


                Image = BookInfo.GetElementsByClassName("product-poster__main-picture")[0].GetElementsByTagName("img")[0]
                   .Attributes["src"]
                   .Value
                   .Replace(" ", "")
                   .Replace("\t", "")
                   .Replace("\n", "");


            }


            catch (Exception ex)
            {
                //     Console.WriteLine($"{DateTime.Now} : ERROR for book - {ex.Message}");
            }

            Book book = new Book()
            {
                Author = Author,
                Description = Description,
                Genre = Genre,
                Image = Image,
                Name = Name,
                Remainder = Remainder,
                Price = Price,
                NumberOfPages = NumberOfPages,
                CoverType = CoverType,
                Publisher = Publisher,
                ISBN = ISBN,
                PublisherYear = PublisherYear,
                Series = Series,
                AgeRestrictions = AgeRestrictions,
                Format = Format,
                Weight = Weight,
                Sales = Sales,
                Reviews = Reviews,
                Rating = Rating,
                VendorCode = VendorCode,
                ParsingDate = ParsingDate
            };

            book.SourceName = "https://book24.ru";
            return book;
            }
        }
        Book vot = new Book();
        vot.SourceName = "https://book24.ru";
        return vot;
    }

    public async Task StartParsingAsync()
    {
        Console.WriteLine("ВВЕДИТЕ СТРАНИЦУ С КОТОРОЙ ПАРСИТЬ, БЕЗ ПРОБЕЛОВ И ДРУГИХ ЗНАКОВ");
        //string startStr = Console.ReadLine();
        var finalBooks = new List<Book>();
        List<Book> parsedBooks1 = new List<Book>();

        var address = "https://book24.ru/catalog/page-";
        var countOne = 1;
        int start = 1;
        Task t1 = Task.Run(async () =>
        {
            parsedBooks1 = await ParseBookInfo(address, start, countOne + start);

            WriteToJSON("BooksFromVcennosti.json", parsedBooks1);
            parsedBooks1.Clear();

        });

        Task.WaitAll(t1);
        Console.ReadLine();
    }

    private void WriteToJSON(string path, List<Book> books)
    {
        var json = JsonConvert.SerializeObject(books);
        File.WriteAllText(path, json, Encoding.UTF8);
        Console.WriteLine("Books writed to json, count " + books.Count);
        File.WriteAllText(path + "Count", Convert.ToString(books.Count));
    }

    private string GetValue(IHtmlCollection<IElement> collection)
    {
        if (collection.Length > 0)
            return collection[0].TextContent;
        return "";
    }

    private IElement GetElement(IHtmlCollection<IElement> collection)
    {
        if (collection.Length > 0)
            return collection[0];
        return null;
    }
}
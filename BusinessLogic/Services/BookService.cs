using Core.Models;
using Core.Repository;

namespace BusinessLogin.Services;

/// <summary>
/// Сервис работы с книгами (обёртка над репозиторием)
/// </summary>
public class BookService
{
    private readonly IBookRepository _repository;

    public BookService(IBookRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// добавляет в бд батч книг, если экземпляр книги уже существует в бд - обновляет её
    /// </summary>
    public async Task AddRangeAsync(IEnumerable<Book> books)
    {
        foreach (var book in books)
        {
            if(book.SiteBookId is null)
                continue;
            var exists = await _repository.GetEqualBookAsync(book);
            
            if (!(exists is null))
            {
                book.Id = exists.Id; 
                await _repository.UpdateAsync(book);
            }
            else
            {
                await _repository.CreateAsync(book);
            }
        }
    }

    public async Task<string> AddBookAsync(Book book)
    {
        if (book.SiteBookId is null)
        {
            return "error";
        }
        var exists = await _repository.GetEqualBookAsync(book);

        if (!(exists is null))
        {
            book.Id = exists.Id;
            return "exist";
            await _repository.UpdateAsync(book);
        }
        else
        {
            
            await _repository.CreateAsync(book);
            return "";
        }
    }
}
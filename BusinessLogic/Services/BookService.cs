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
    public void AddRange(IEnumerable<Book> books)
    {
        foreach (var book in books)
        {
            if(book.SiteBookId is null)
                continue;
            var exists = _repository.GetEqualBook(book);
            
            if (!(exists is null))
            {
                book.Id = exists.Id; 
                _repository.Update(book);
            }
            else
            {
                _repository.Create(book);
            }
        }
    }


}
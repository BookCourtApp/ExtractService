using Core.Models;

namespace Core.Repository;

/// <summary>
/// интерфейс репозитория книг
/// </summary>
public interface IBookRepository
{
    /// <summary>
    /// создать книгу
    /// </summary>
    /// <param name="book">книга с заполненными полями, id не заполнен</param>
    /// <returns>книга с новым id</returns>
    public Task<Book> CreateAsync(Book book);

    /// <summary>
    /// Обновление существующей книги
    /// </summary>
    /// <param name="book">книга с существующим id</param>
    public Task UpdateAsync(Book book);

    /// <summary>
    /// удаление существующей книги
    /// </summary>
    /// <param name="id">id книги</param>
    public Task<bool> DeleteAsync(Guid id);

    /// <summary>
    /// получение существующей книги
    /// </summary>
    /// <param name="id">id существующей книги</param>
    public Task GetAsync(Guid id);

    /// <summary>
    /// получение существующей книги по заданному экземпляру
    /// </summary>
    public Task<Book?> GetEqualBookAsync(Book book);

    /// <summary>
    /// Получение всех книг
    /// </summary>
    public Task<IEnumerable<Book>> GetAllAsync();
}
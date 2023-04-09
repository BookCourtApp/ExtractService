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
    public Book Create(Book book);

    /// <summary>
    /// Обновление существующей книги
    /// </summary>
    /// <param name="book">книга с существующим id</param>
    public void Update(Book book);

    /// <summary>
    /// удаление существующей книги
    /// </summary>
    /// <param name="id">id книги</param>
    public bool Delete(Guid id);

    /// <summary>
    /// получение существующей книги
    /// </summary>
    /// <param name="id">id существующей книги</param>
    public void Get(Guid id);

    /// <summary>
    /// получение существующей книги по заданному экземпляру
    /// </summary>
    public Book? GetEqualBook(Book book);

    /// <summary>
    /// Получение всех книг
    /// </summary>
    public IEnumerable<Book> GetAll();
}
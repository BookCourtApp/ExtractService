using Core.Models;
using Core.Repository;
using Microsoft.EntityFrameworkCore;

namespace InfrastructureProject.Data;

/// <summary>
/// реализация репозитория книг
/// </summary>
public class BookRepository : IBookRepository  //todo: сделать асинхронность
{
    private readonly IDbContextFactory<ApplicationContext> _contextFactory;

    public BookRepository(IDbContextFactory<ApplicationContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }
    
    /// <inheritdoc />
    public Book Create(Book book)
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            var result = context.Books.Add(book).Entity;
            return result; 
        }
        
    }
    
    /// <inheritdoc />
    public void Update(Book book)
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            context.Update(book);

            context.SaveChanges();
        }
    }

    public bool Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    public void Get(Guid id)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Book? GetEqualBook(Book book)
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            var result = context.Books.ToListAsync().Result.FirstOrDefault(b => b.IsEqualBook(book));
            return result;
        }
    }

    /// <inheritdoc />
    public IEnumerable<Book> GetAll()
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            return context.Books.ToListAsync().Result;
        }
    }
}
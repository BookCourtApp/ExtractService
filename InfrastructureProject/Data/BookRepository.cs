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
    public async Task<Book> CreateAsync(Book book)
    {
        using (var context = await _contextFactory.CreateDbContextAsync())
        {
            var result = (await context.Books.AddAsync(book)).Entity;
            await context.SaveChangesAsync();
            return result; 
        }
        
    }
    
    /// <inheritdoc />
    public async Task UpdateAsync(Book book)
    {
        using (var context = await _contextFactory.CreateDbContextAsync())
        {
            context.Update(book);

            await context.SaveChangesAsync();
        }
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task GetAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public async Task<Book?> GetEqualBookAsync(Book book)
    {
        using (var context = await _contextFactory.CreateDbContextAsync())
        {
            var result = (await context.Books.ToListAsync()).FirstOrDefault(b => b.IsEqualBook(book));
            return result;
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        using (var context = await _contextFactory.CreateDbContextAsync())
        {
            return await context.Books.ToListAsync();
        }
    }
}
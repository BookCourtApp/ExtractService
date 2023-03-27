using ExtractorProject.Extractors.Models;
using InfrastructureProject;
using Microsoft.EntityFrameworkCore;
using Core;

namespace Core.Database;

public class BookService
{
    private readonly DbContextFactory _contextFactory;
    private readonly string _dbPath;

    public BookService(DbContextFactory contextFactory, string dbPath)
    {
        _contextFactory = contextFactory;
        _dbPath = dbPath;
    }

    public async Task AddRangeAsync(List<Book> books)
    {
        var context = _contextFactory.Create(_dbPath);
        var notExistingBooks = await books.AsyncParallelWhereOrderedByCompletion(async b1 => await context.Books.FirstOrDefaultAsync(b2 => b1.SiteBookId == b2.SiteBookId) is null);
        await context.Books.AddRangeAsync(notExistingBooks);
        await context.SaveChangesAsync();
        await context.DisposeAsync();
    }
}
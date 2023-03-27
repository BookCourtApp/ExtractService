using ExtractorProject.Extractors.Models;
using InfrastructureProject;
<<<<<<< HEAD
using Microsoft.EntityFrameworkCore;
using Core;
=======
>>>>>>> 21e9a4b6591dae6a0ffc6f4aca297bcf6b4f32e5

namespace Core.Database;

public class BookService
{
<<<<<<< HEAD
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
=======
    private readonly ApplicationContext _context;

    public BookService(ApplicationContext context)
    {
        _context = context;
    }

    public async Task AddRange(List<Book> books)
    {
        var notExistingBooks = books.Where(b1 => _context.Books.FirstOrDefault(b2 => b1.SiteBookId == b2.SiteBookId) is null);
        await _context.Books.AddRangeAsync(notExistingBooks);
        await _context.SaveChangesAsync();
>>>>>>> 21e9a4b6591dae6a0ffc6f4aca297bcf6b4f32e5
    }
}
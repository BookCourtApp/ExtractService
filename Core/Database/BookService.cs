using ExtractorProject.Extractors.Models;
using InfrastructureProject;
using Microsoft.EntityFrameworkCore;
using Core;

namespace Core.Database;

public class BookService
{
    private readonly ApplicationContext _context;

    public BookService(ApplicationContext context)
    {
        _context = context;
    }

    public async Task AddRangeAsync(List<Book> books)
    {
        var notExistingBooks = await books.AsyncParallelWhereOrderedByCompletion(async b1 => await _context.Books.FirstOrDefaultAsync(b2 => b1.SiteBookId == b2.SiteBookId) is null);
        await _context.Books.AddRangeAsync(notExistingBooks);
        await _context.SaveChangesAsync();
    }
}
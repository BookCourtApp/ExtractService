using ExtractorProject.Extractors.Models;
using InfrastructureProject;

namespace Core.Database;

public class BookService
{
    private readonly ApplicationContext _context;

    public BookService(ApplicationContext context)
    {
        _context = context;
    }

    public async Task AddRange(List<Book> books)
    {
        books.Where(b1 => _context.Books.FirstOrDefault(b2 => b1.SiteBookId == b2.SiteBookId))
    }
}
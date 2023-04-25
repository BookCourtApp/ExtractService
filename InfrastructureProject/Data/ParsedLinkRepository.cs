using Microsoft.EntityFrameworkCore;

namespace InfrastructureProject.Data;

public class ParsedLinkRepository
{
    private readonly IDbContextFactory<ApplicationContext> _contextFactory;

    public ParsedLinkRepository(IDbContextFactory<ApplicationContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public bool IsLinkParsed(string link)
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            var res = context.ParsedLinks.FirstOrDefault(p => p.Link == link);
            if (res is null)
                return false;
        }

        return true;
    }
}
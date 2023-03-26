using Microsoft.EntityFrameworkCore;

namespace InfrastructureProject;

public class DbContextFactory
{
    public ApplicationContext Create(string path)
    {
        var options = new DbContextOptionsBuilder<ApplicationContext>()
            .UseSqlite($"Data Source={path}")
            .Options;
        return new ApplicationContext(options);
    }
}
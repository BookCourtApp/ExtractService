
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace InfrastructureProject;

/// <summary>
/// Контекст для доступа к БД через EF
/// </summary>
public class ApplicationContext : DbContext
{
    /// <summary>
    /// Коллекция дата сетов Книг
    /// </summary>
    public DbSet<Book> Books { get; set; }
    
    /// <summary>
    /// Коллекция дата сетов Ошибок
    /// </summary>
    public DbSet<Error> Errors{ get; set; }
    
    /// <summary>
    /// Коллекция дата сетов результатов экстрактора
    /// </summary>
    public DbSet<ExtractorResult> ExtractorResults { get; set; }
    
    public DbSet<UserPreference> UserPreferences { get; set; }

    public DbSet<ParsedLink> ParsedLinks { get; set; }

    /// <summary>
    /// .cotr
    /// </summary>
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserPreference>().HasKey("UserLogin", "LinkBook", "SiteName", "PreferenceType");
        modelBuilder.Entity<ParsedLink>().HasKey(p => p.Link);
        base.OnModelCreating(modelBuilder);
    }
}
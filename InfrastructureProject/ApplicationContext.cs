
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

    /// <summary>
    /// Коллекция с юзерами
    /// </summary>
    public DbSet<User> Users { get; set; }

    public DbSet<UserPreference> UserPreferences { get; set; }

    /// <summary>
    /// .cotr
    /// </summary>
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasKey(u => u.UserLogin);
        modelBuilder.Entity<UserPreference>().HasKey("UserLogin", "LinkBook", "SiteName", "PreferenceType");
        base.OnModelCreating(modelBuilder);
    }
}
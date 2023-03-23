using ExtractorService.Models;
using InfrastructureProject.Config;
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
    
    // /// <summary>
    // /// Коллекция дата сетов результатов экстрактора
    // /// </summary>
    // /// 
     public DbSet<ExtractorResult> ExtractorResults { get; set; }
    
    /// <summary>
    /// .cotr
    /// </summary>
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

}
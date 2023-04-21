using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace InfrastructureProject.Data;

public class UserRepository
{
    private readonly IDbContextFactory<ApplicationContext> _contextFactory;

    public UserRepository(IDbContextFactory<ApplicationContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }
    
    /// <inheritdoc />
    public async Task<User> CreateAsync(User user)
    {
        using (var context = await _contextFactory.CreateDbContextAsync())
        {
            var result = (await context.Users.AddAsync(user)).Entity;
            await context.SaveChangesAsync();
            return result; 
        }
        
    }
    
    /// <inheritdoc />
    public async Task UpdateAsync(User user)
    {
        using (var context = await _contextFactory.CreateDbContextAsync())
        {
            context.Users.Update(user);

            await context.SaveChangesAsync();
        }
    }
    
    /// <inheritdoc />
    public async Task<User?> GetEqualUserAsync(User user)
    {
        using (var context = await _contextFactory.CreateDbContextAsync())
        {
            var result = await context.Users.FirstOrDefaultAsync(b => b.UserLogin == user.UserLogin && b.SiteName == user.SiteName);
            return result;
        }
    }
}
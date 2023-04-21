using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace InfrastructureProject.Data;

public class UserPreferenceRepository
{
    private readonly IDbContextFactory<ApplicationContext> _contextFactory;

    public UserPreferenceRepository(IDbContextFactory<ApplicationContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }
    
    /// <inheritdoc />
    public async Task<UserPreference> CreateAsync(UserPreference userPreference)
    {
        using (var context = await _contextFactory.CreateDbContextAsync())
        {
            var result = (await context.UserPreferences.AddAsync(userPreference)).Entity;
            await context.SaveChangesAsync();
            return result; 
        }
        
    }
    
    /// <inheritdoc />
    public async Task UpdateAsync(UserPreference userPreference)
    {
        using (var context = await _contextFactory.CreateDbContextAsync())
        {
            context.UserPreferences.Update(userPreference);

            await context.SaveChangesAsync();
        }
    }
    
    /// <inheritdoc />
    public async Task<UserPreference?> GetEqualUserPreferenceAsync(UserPreference user)
    {
        using (var context = await _contextFactory.CreateDbContextAsync())
        {
            var result = await context.UserPreferences.FirstOrDefaultAsync(b => b.UserLogin == user.UserLogin &&  b.LinkBook == user.LinkBook);
            return result;
        }
    }
}
using Core.Models;
using ExtractorProject.Extractors;
using InfrastructureProject.Data;

namespace BusinessLogin.Services;

public class UserPreferenceService
{
    private readonly UserPreferenceRepository _repository;

    public UserPreferenceService(UserPreferenceRepository repository)
    {
        _repository = repository;
    }
    
    
    /// <summary>
    /// добавляет в бд батч книг, если экземпляр книги уже существует в бд - обновляет её
    /// </summary>
    public async Task AddRangeAsync(IEnumerable<UserPreference> userPreferences)
    {
        foreach (var userPreference in userPreferences)
        {
            if(userPreference.UserLink is null || userPreference.LinkBook is null || userPreference.UserLogin is null)
                continue;
            var exists = await _repository.GetEqualBookAsync(userPreference);
            
            if (!(exists is null))
            {
                await _repository.UpdateAsync(userPreference);
            }
            else
            {
                await _repository.CreateAsync(userPreference);
            }
        }
    }
}
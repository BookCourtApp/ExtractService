using Core.Models;
using ExtractorProject.Extractors;
using InfrastructureProject.Data;
using Microsoft.Extensions.Logging;

namespace BusinessLogin.Services;

public class UserPreferenceService
{
    private readonly UserPreferenceRepository _repository;
    private readonly ILogger<UserPreferenceService> _logger;
    private readonly Queue<User> usersForGetPreference;

    public UserPreferenceService(UserPreferenceRepository repository, ILogger<UserPreferenceService> logger)
    {
        _repository = repository;
        this._logger = logger;
        usersForGetPreference = new Queue<User>();
    }
    
    
    /// <summary>
    /// добавляет в бд батч книг, если экземпляр книги уже существует в бд - обновляет её
    /// </summary>
    public async Task AddRangeAsync(IEnumerable<UserPreference> userPreferences)
    {try
            {
        foreach (var userPreference in userPreferences)
        {
            
                if (userPreference.UserLink is null || userPreference.LinkBook is null || userPreference.UserLogin is null)
                    continue;
                var exists = await _repository.GetEqualUserPreferenceAsync(userPreference);

                if (!(exists is null))
                {
                   continue; await _repository.UpdateAsync(userPreference);
                }
                else
                {
                    await _repository.CreateAsync(userPreference);
                }
          
        }  
        }
            catch (Exception ex)
            {
                _logger.LogCritical("ошибка при добавлении предпочтения юзера \n{0} {1}", ex.Message, ex.StackTrace);
            }
    }

    public User Dequeue()
    {
        return usersForGetPreference.Dequeue();
    }

    public bool IsEnd()
    {
        return usersForGetPreference.Count == 0;
    }

    public void Enqueue(User user)
    {
        usersForGetPreference.Enqueue(user);
    }
}
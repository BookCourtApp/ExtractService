using Core.Models;
using ExtractorProject.Extractors;
using InfrastructureProject.Data;
using Microsoft.Extensions.Logging;

namespace BusinessLogin.Services;

public class UserService
{
    private readonly UserRepository _repository;
    private readonly ILogger<UserService> _logger;

    public UserService(UserRepository repository, ILogger<UserService> logger)
    {
        _repository = repository;
        this._logger = logger;
    }
    
    
    /// <summary>
    /// добавляет в бд батч книг, если экземпляр книги уже существует в бд - обновляет её
    /// </summary>
    public async Task AddRangeAsync(IEnumerable<User> users)
    {try
            {
        foreach (var user in users)
        {
            
                if (user.UserLink is null)
                    continue;
                var exists = await _repository.GetEqualUserAsync(user);

                if (!(exists is null))
                {
                    user.UserLink = exists.UserLink;
                    await _repository.UpdateAsync(user);
                }
                else
                {
                    await _repository.CreateAsync(user);
                }
            
        }}
            catch (Exception ex)
            {
                _logger.LogCritical("ошибка при добавлении юзера \n{0} {1}", ex.Message, ex.StackTrace);
            }
    }

    public async Task AddAsync(User user)
    {
        try
        {

            if (user.UserLink is null)
                    return;
            var exists = await _repository.GetEqualUserAsync(user);

            if (!(exists is null))
            {
                user.UserLink = exists.UserLink;
                await _repository.UpdateAsync(user);
            }
            else
            {
                await _repository.CreateAsync(user);
            }
        
        }
        catch (Exception ex)
        {
            _logger.LogCritical("ошибка при добавлении юзера \n{0} {1}", ex.Message, ex.StackTrace);
        }
    }
}
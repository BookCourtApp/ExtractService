using Core.Models;
using ExtractorProject.Extractors;
using InfrastructureProject.Data;

namespace BusinessLogin.Services;

public class UserService
{
    private readonly UserRepository _repository;

    public UserService(UserRepository repository)
    {
        _repository = repository;
    }
    
    
    /// <summary>
    /// добавляет в бд батч книг, если экземпляр книги уже существует в бд - обновляет её
    /// </summary>
    public async Task AddRangeAsync(IEnumerable<User> users)
    {
        foreach (var book in users)
        {
            if(book.UserLink is null)
                continue;
            var exists = await _repository.GetEqualBookAsync(book);
            
            if (!(exists is null))
            {
                book.UserLink = exists.UserLink; 
                await _repository.UpdateAsync(book);
            }
            else
            {
                await _repository.CreateAsync(book);
            }
        }
    }
}
using Microsoft.EntityFrameworkCore;

namespace Core.Models;


public class User
{
    public string UserLogin { get; set; }

    public string? UserLink { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Sex { get; set; }

    public string? BirthDate { get; set; }

    public string? Location { get; set; }

    public string? ReadingDevices { get; set; }

    public string? RegistrationDate { get; set; }

    public string? Status { get; set; }

    public int? Rating { get; set; }
    
    public int? ActivityIndex { get; set; }
    
    public string? Tags { get; set; }

    public string SiteName { get; set; }
    
    /// <summary>
    /// проверка является ли книга тем же экземпляром
    /// </summary>
    /// <param name="other">проверяемая книга</param>
    /// <returns>true если siteBookId и SourceUrl одинаковые</returns>
    public bool IsEqualUser(User other)
    {
        if (other.UserLogin == this.UserLogin && other.SiteName == this.SiteName)
            return true;
        return false;
    }
}
namespace Core.Models;

public class UserPreference
{
    public string SiteName { get; set; }

    
    public string UserLink { get; set; }
    
    public string UserLogin { get; set; }

    public string PreferenceType { get; set; }

    public string LinkBook { get; set; }

    public string? UserEvaluationBook { get; set; }

    public string? UserEvaluationDate { get; set; }
    
    /// <summary>
    /// проверка является ли книга тем же экземпляром
    /// </summary>
    /// <param name="other">проверяемая книга</param>
    /// <returns>true если siteBookId и SourceUrl одинаковые</returns>
    public bool IsEqualUser(UserPreference other)
    {
        if (other.UserLogin == this.UserLogin && other.SiteName == this.SiteName && other.LinkBook == this.LinkBook && other.PreferenceType == this.PreferenceType)
            return true;
        return false;
    }
}
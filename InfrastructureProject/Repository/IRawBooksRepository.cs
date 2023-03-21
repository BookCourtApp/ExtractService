namespace InfrastructureProject.Repository;
                
/// <summary>
/// Репозиторий для работы с сырыми книгами в БД
/// </summary>
public interface IRawBooksRepository
{
    /// <summary>
    /// Добавить множество книг
    /// </summary>
    /// <param name="books"></param>
    void AddRange(List<string> books);  // todo: заменить string на норм типы

    /// <summary>
    /// Получить список книг
    /// </summary>
    /// <returns></returns>
    List<string> GetAll();
}
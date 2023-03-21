namespace InfrastructureProject.Repository;

/// <summary>
/// Интерфейс репозитория который будет работать с результатом экстракторов
/// </summary>
public interface IExtractorResultRepository
{
    /// <summary>
    /// Создать результат экстрактора по значению Argument
    /// </summary>
    /// <param name="argument">Аргумент со значениями для создания</param>
    /// <returns></returns>
    string Create(string argument);     // todo: заменить string на норм типы, пока они не реализованы
    
    /// <summary>
    /// Изменить результат экстрактора на значения из Argument
    /// </summary>
    /// <param name="argument">Аргумент со значениями для изменения</param>
    void Update(string argument);
    
    /// <summary>
    /// Получить значение результата по id
    /// </summary>
    /// <param name="id">id по которому искать результат</param>
    /// <returns>Значение результата</returns>
    string Get(Guid id);
}
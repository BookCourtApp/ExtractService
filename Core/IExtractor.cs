namespace Core;

/// <summary>
/// интерфейс Extractor
/// </summary>
public interface IExtractor
{

    /// <summary>
    /// метод, который возвращает окончание работы Extract
    /// </summary>
    /// <returns>True, если выкачка закончилась</returns>
    public bool IsEndData(); 


    /// <summary>
    /// Выкачка следующих буферов(пачек) 
    /// </summary>
    /// <returns>Возвращает Id ExtractorResult</returns>
    public Guid ExtractNextBatch();

    

}    





namespace Core.Object
{
    /// <summary>
    /// Перечисление для определения процесса, при котором произошла ошибка
    /// </summary>
    public enum PlaceType
    {
        /// <summary>Ошибка при процессе получения данных</summary>
        Extracting = 0,
        /// <summary>Ошибка при обработке полученных данных</summary>
        Handling = 1,
        /// <summary>Ошибка при сохранении данных куда-либо</summary>
        Saving = 2
    }
}

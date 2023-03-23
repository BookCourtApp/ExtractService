
namespace ExtractorService.Models
{
    /// <summary>
    /// Перечисление для выбора метода парсинга данных с сайта
    /// </summary>
    public enum ExtractorType
    {
        /// <summary>При обращении к апи при выгрузке записей</summary>
        API = 0,
        /// <summary>При выгрузке записей из html документа</summary>
        HTML = 1,
        /// <summary>При выгрузке записей из Excel документа</summary>
        SCV = 2
    }
}

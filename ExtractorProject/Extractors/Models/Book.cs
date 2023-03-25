
namespace ExtractorProject.Extractors.Models
{
    /// <summary>
    /// Модель для хранения информации о книге в бд. 
    /// </summary>
    public class Book
    {
        /// <summary>
        /// Id книги
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Имя книги
        /// </summary>
        public string? Name { get; set; }
        
        /// <summary>
        /// Автор книги
        /// </summary>
        public string? Author { get; set; }
        
        /// <summary>
        /// Описание книги
        /// </summary>
        public string? Description { get; set; }
        
        /// <summary>
        /// Цена книги
        /// </summary>
        public int? Price { get; set; }
        
        /// <summary>
        /// Ссылка на книгу
        /// </summary>
        public string? SourceName { get; set; }
        
        /// <summary>
        /// Изображение обложки книги 
        /// </summary>
        public string? Image { get; set; }
        
        /// <summary>
        /// Жанр книги
        /// </summary>
        public string? Genre { get; set; }
        
        /// <summary>
        /// Количество страниц книги
        /// </summary>
        public int? NumberOfPages { get; set; }
        
        /// <summary>
        /// Isbn номер книги 
        /// </summary>
        public string? ISBN { get; set; }
        
        /// <summary>
        /// Дата парсинга книги
        /// </summary>
        public DateTime ParsingDate { get; set; }
        
        /// <summary>
        /// Год выпуска книги
        /// </summary>
        public int? PublisherYear { get; set; }
        
        /// <summary>
        /// id книги на сайте(артикль?)
        /// </summary>
        public string? SiteBookId { get; set; }

        /// <summary>
        ///  Хлебные крошки,
        /// Есть крошки на сайте следующие 
        /// Книги /  Нехудожественная литература /  Информационные технологии /  Информатика
        ///
        /// Парсятся они как отдельные элементы, получается массив этих крошек со значениями, например значение "Книги"
        ///
        ///В поле Breadcrqmbs я запишу их в формате "Книги/Нехудожественная литература/..."
        /// </summary>
        public string? Breadcrqmbs { get; set; }
    }
}

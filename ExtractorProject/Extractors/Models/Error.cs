namespace ExtractorService.Models
{
    public class Error
    {
        /// <summary>
        /// Причина ошибки
        /// </summary>
        public string Reason { get; set; }
        public PlaceType Type{ get; set; } 
    }
}

namespace ExtractorService.Models
{
    public class Error
    {
        /// <summary>
        /// Причина ошибки
        /// </summary>

        public Guid Id { get; set; }
        public string Reason { get; set; }
        public PlaceType Type{ get; set; }

        public Guid ExtractorResultId { get; set; }
    }
}

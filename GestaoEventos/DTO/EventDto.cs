// DTOs/EventDto.cs
namespace GestaoEventos.DTOs
{
    public class EventDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Date { get; set; }
        public string Location { get; set; }
        public int TotalTickets { get; set; }
        public int AvailableTickets { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public int CreatedBy { get; set; }
    }
}

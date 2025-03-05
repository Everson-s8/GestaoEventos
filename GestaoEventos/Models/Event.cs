using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoEventos.Models
{
    public class Event
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTimeOffset Date { get; set; }
        public DateTimeOffset EndDate { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public int TotalTickets { get; set; }

        public int AvailableTickets { get; set; }

        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }

        public int CreatedBy { get; set; }

        [ForeignKey("CreatedBy")]
        public User Creator { get; set; }
    }
}

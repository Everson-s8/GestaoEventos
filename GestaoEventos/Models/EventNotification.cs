using System;
using System.ComponentModel.DataAnnotations;

namespace GestaoEventos.Models
{
    public class EventNotification
    {
        public int Id { get; set; }

        [Required]
        public int EventId { get; set; }

        // Tipo pode ser "email" ou "phone"
        [Required]
        public string Type { get; set; }

        [Required]
        public string Value { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}

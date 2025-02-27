using System;
using System.ComponentModel.DataAnnotations;

namespace GestaoEventos.Models
{
    public class EventStaff
    {
        public int Id { get; set; }

        [Required]
        public int EventId { get; set; }  // Chave estrangeira para o evento

        [Required]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; } // Em produção, armazene um hash

        public DateTime CreatedAt { get; set; }

        [Required]
        public UserRole Role { get; set; }
    }
}

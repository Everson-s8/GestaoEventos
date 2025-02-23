using System.ComponentModel.DataAnnotations;

namespace GestaoEventos.Models
{
    public enum UserRole
    {
        Client,
        EventManager
    }

    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; } // Em produção, utilize um método seguro de hashing

        [Required]
        public UserRole Role { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace GestaoEventos.Models
{
    public class EventProduct
    {
        public int Id { get; set; }

        [Required]
        public int EventId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public string Category { get; set; }

        // Armazena os dados binários da imagem (coluna do tipo bytea no PostgreSQL)
        [Required]
        public byte[] ImageData { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}

using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace GestaoEventos.DTOs
{
    public class AddProductDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public IFormFile File { get; set; }  // Recebe o arquivo de imagem
    }
}

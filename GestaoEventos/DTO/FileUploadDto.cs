using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace GestaoEventos.DTOs
{
    public class FileUploadDto
    {
        [Required]
        public IFormFile File { get; set; }
    }
}

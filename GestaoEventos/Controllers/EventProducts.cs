using GestaoEventos.Data;
using GestaoEventos.DTOs;
using GestaoEventos.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading.Tasks;

namespace GestaoEventos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventProductsController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly ApplicationDbContext _context;

        public EventProductsController(IWebHostEnvironment env, ApplicationDbContext context)
        {
            _env = env;
            _context = context;
        }

        // Endpoint para cadastrar produto com upload de imagem
        // Exemplo: POST api/EventProducts/{eventId}
        [HttpPost("{eventId}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddProduct(int eventId, [FromForm] AddProductDto dto)
        {
            if (dto.File == null || dto.File.Length == 0)
            {
                return BadRequest("Arquivo inválido.");
            }

            // Define a pasta de destino (ex: wwwroot/uploads)
            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Cria um nome único para o arquivo
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.File.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            // Salva o arquivo na pasta
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.File.CopyToAsync(stream);
            }

            // Gera a URL para acessar a imagem
            var imageUrl = $"{Request.Scheme}://{Request.Host}/uploads/{fileName}";

            // Cria o registro do produto, armazenando a URL da imagem
            var product = new EventProduct
            {
                EventId = eventId,
                Name = dto.Name,
                Price = dto.Price,
                Quantity = dto.Quantity,
                ImageUrl = imageUrl,
                CreatedAt = DateTime.UtcNow
            };

            _context.EventProducts.Add(product);
            await _context.SaveChangesAsync();

            return Ok(product);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using GestaoEventos.DTOs;
using System;
using System.IO;
using System.Threading.Tasks;

namespace GestaoEventos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public UploadController(IWebHostEnvironment env)
        {
            _env = env;
        }

        // POST: api/Upload/product-image
        [HttpPost("product-image")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadProductImage([FromForm] FileUploadDto fileUpload)
        {
            var file = fileUpload.File;
            if (file == null || file.Length == 0)
            {
                return BadRequest("Arquivo inválido.");
            }

            // Validação opcional: verifique o tamanho ou extensão

            // Define a pasta de destino, por exemplo, "uploads"
            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Cria um nome único para o arquivo
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Cria a URL para acessar o arquivo (ajuste conforme sua configuração)
            var imageUrl = $"{Request.Scheme}://{Request.Host}/uploads/{fileName}";

            return Ok(new { imageUrl });
        }
    }
}

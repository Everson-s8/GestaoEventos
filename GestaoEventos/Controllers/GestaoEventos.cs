using GestaoEventos.Data;
using GestaoEventos.DTO;
using GestaoEventos.DTOs;
using GestaoEventos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestaoEventos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventSettingsController(IWebHostEnvironment env, ApplicationDbContext context) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IWebHostEnvironment _env = env;

        // GET: api/EventSettings/{eventId}
        [HttpGet("{eventId}")]
        public async Task<IActionResult> GetSettings(int eventId)
        {
            var staff = await _context.EventStaffs.Where(s => s.EventId == eventId).ToListAsync();
            var products = await _context.EventProducts.Where(p => p.EventId == eventId).ToListAsync();
            var notifications = await _context.EventNotifications.Where(n => n.EventId == eventId).ToListAsync();
            return Ok(new { staff, products, notifications });
        }
        [HttpPost("{eventId}/staff")]
        public async Task<IActionResult> AddStaff(int eventId, [FromBody] AddStaffDto dto)
        {

            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
            {
                return BadRequest("Email já está em uso.");
            }

            // Cria o registro de EventStaff
            var staff = new EventStaff
            {
                EventId = eventId,
                Name = dto.Name,
                Email = dto.Email,
                Password = dto.Password,  // Em produção, aplique um hash
                CreatedAt = DateTime.UtcNow,
                Role = UserRole.Employee // ou o valor que você definir para funcionário temporário
            };

            _context.EventStaffs.Add(staff);
            await _context.SaveChangesAsync();  // Salva primeiro para que o Id seja gerado

            // Agora, verifique se o usuário correspondente já existe na tabela Users; se não, crie-o
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null)
            {
                user = new User
                {
                    Name = dto.Name,
                    Email = dto.Email,
                    PasswordHash = dto.Password, // Em produção, aplique o hash da senha
                    Role = UserRole.Employee // Role definido para funcionário
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }

            return Ok(staff);
        }


        [HttpDelete("staff/{id}")]
        public async Task<IActionResult> DeleteStaff(int id)
        {
            var staff = await _context.EventStaffs.FindAsync(id);
            if (staff == null)
                return NotFound();

            // Remove o registro de EventStaff
            _context.EventStaffs.Remove(staff);

            // Procura e remove o usuário correspondente na tabela Users
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == staff.Email);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }


        // Endpoint para cadastrar produto com upload de imagem
        // Exemplo: POST api/EventProducts/{eventId}
        [HttpPost("products/{eventId}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddProduct(int eventId, [FromForm] AddProductDto dto)
        {
            if (dto.File == null || dto.File.Length == 0)
            {
                return BadRequest("Arquivo inválido.");
            }

            byte[] imageData;
            // Lê o arquivo para um array de bytes usando MemoryStream
            using (var ms = new MemoryStream())
            {
                await dto.File.CopyToAsync(ms);
                imageData = ms.ToArray();
            }

            // Cria o registro do produto, usando os dados binários da imagem
            var product = new EventProduct
            {
                EventId = eventId,
                Name = dto.Name,
                Price = dto.Price,
                Quantity = dto.Quantity,
                Category = dto.Category,
                ImageData = imageData,
                CreatedAt = DateTime.UtcNow
            };

            _context.EventProducts.Add(product);
            await _context.SaveChangesAsync();

            return Ok(product);
        }
    
        // DELETE: api/EventSettings/products/{id}
        [HttpDelete("products/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.EventProducts.FindAsync(id);
            if (product == null)
                return NotFound();
            _context.EventProducts.Remove(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/EventSettings/{eventId}/notifications
        [HttpPost("{eventId}/notifications")]
        public async Task<IActionResult> AddNotification(int eventId, [FromBody] AddNotificationDto dto)
        {
            var notification = new EventNotification
            {
                EventId = eventId,
                Type = dto.Type,
                Value = dto.Value,
                CreatedAt = DateTime.UtcNow
            };

            _context.EventNotifications.Add(notification);
            await _context.SaveChangesAsync();
            return Ok(notification);
        }

        // DELETE: api/EventSettings/notifications/{id}
        [HttpDelete("notifications/{id}")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            var notification = await _context.EventNotifications.FindAsync(id);
            if (notification == null)
                return NotFound();
            _context.EventNotifications.Remove(notification);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

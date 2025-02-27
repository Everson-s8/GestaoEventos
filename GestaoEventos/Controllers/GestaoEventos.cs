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
    public class EventSettingsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public EventSettingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/EventSettings/{eventId}
        [HttpGet("{eventId}")]
        public async Task<IActionResult> GetSettings(int eventId)
        {
            var staff = await _context.EventStaffs.Where(s => s.EventId == eventId).ToListAsync();
            var products = await _context.EventProducts.Where(p => p.EventId == eventId).ToListAsync();
            var notifications = await _context.EventNotifications.Where(n => n.EventId == eventId).ToListAsync();
            return Ok(new { staff, products, notifications });
        }

        // POST: api/EventSettings/{eventId}/staff
        [HttpPost("{eventId}/staff")]
        public async Task<IActionResult> AddStaff(int eventId, [FromBody] AddStaffDto dto)
        {
            var staff = new EventStaff
            {
                EventId = eventId,
                Name = dto.Name,
                Email = dto.Email,
                Password = dto.Password,  // Em produção, aplique um hash
                CreatedAt = DateTime.UtcNow
            };

            _context.EventStaffs.Add(staff);
            await _context.SaveChangesAsync();
            return Ok(staff);
        }

        // DELETE: api/EventSettings/staff/{id}
        [HttpDelete("staff/{id}")]
        public async Task<IActionResult> DeleteStaff(int id)
        {
            var staff = await _context.EventStaffs.FindAsync(id);
            if (staff == null)
                return NotFound();
            _context.EventStaffs.Remove(staff);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/EventSettings/{eventId}/products
        [HttpPost("{eventId}/products")]
        public async Task<IActionResult> AddProduct(int eventId, [FromBody] AddProductDto dto)
        {
            var product = new EventProduct
            {
                EventId = eventId,
                Name = dto.Name,
                Price = dto.Price,
                Quantity = dto.Quantity,
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

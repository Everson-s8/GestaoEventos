using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestaoEventos.Data;
using GestaoEventos.Models;
using GestaoEventos.DTO;

namespace GestaoEventos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public TicketsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST api/tickets/purchase – efetua a compra de um ingresso
        [HttpPost("purchase")]
        public async Task<IActionResult> PurchaseTicket([FromBody] PurchaseTicketDto dto)
        {
            var ev = await _context.Events.FindAsync(dto.EventId);
            if (ev == null)
                return NotFound("Evento não encontrado.");

            if (ev.AvailableTickets <= 0)
                return BadRequest("Ingressos esgotados.");

            // Diminui a quantidade de ingressos disponíveis
            ev.AvailableTickets--;

            var ticket = new Ticket
            {
                EventId = ev.Id,
                BuyerId = dto.BuyerId,
                PurchaseDate = DateTime.UtcNow // Use UTC para evitar conflito com o PostgreSQL
            };

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            return Ok(ticket);
        }


        // GET api/tickets/client/{clientId} – lista os ingressos comprados por um cliente
        [HttpGet("client/{clientId}")]
        public async Task<IActionResult> GetClientTickets(int clientId)
        {
            var tickets = await _context.Tickets
                .Include(t => t.Event)
                .Where(t => t.BuyerId == clientId)
                .ToListAsync();
            return Ok(tickets);
        }
    }


}

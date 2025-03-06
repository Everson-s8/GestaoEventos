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

        [HttpPost("purchase")]
        public async Task<IActionResult> PurchaseTicket([FromBody] PurchaseTicketDto dto)
        {
            var ev = await _context.Events.FindAsync(dto.EventId);
            if (ev == null)
                return NotFound("Evento não encontrado.");

            // Verifica se há ingressos suficientes
            if (ev.AvailableTickets < dto.Quantity)
                return BadRequest("Ingressos esgotados.");

            // Diminui a quantidade disponível pelo número de ingressos comprados
            ev.AvailableTickets -= dto.Quantity;

            var tickets = new List<Ticket>();

            // Cria um registro para cada ingresso
            for (int i = 0; i < dto.Quantity; i++)
            {
                var ticket = new Ticket
                {
                    EventId = ev.Id,
                    BuyerId = dto.BuyerId,
                    PurchaseDate = DateTime.UtcNow
                };
                tickets.Add(ticket);
                _context.Tickets.Add(ticket);
            }

            await _context.SaveChangesAsync();

            return Ok(tickets);
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

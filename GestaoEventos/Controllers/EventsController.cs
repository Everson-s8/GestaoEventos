﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestaoEventos.Data;
using GestaoEventos.Models;
using GestaoEventos.DTOs;

namespace GestaoEventos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET api/events – lista eventos futuros
        // GET api/events – lista eventos que ainda não terminaram
        [HttpGet]
        public async Task<IActionResult> GetEvents()
        {
            var events = await _context.Events
                .Where(e => e.EndDate >= DateTime.Now)
                .Select(e => new EventDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Description = e.Description,
                    Date = e.Date,
                    EndDate = e.EndDate, // Certifique-se de incluir o EndDate no DTO
                    Location = e.Location,
                    TotalTickets = e.TotalTickets,
                    AvailableTickets = e.AvailableTickets,
                    ContactPhone = e.ContactPhone,
                    ContactEmail = e.ContactEmail,
                    CreatedBy = e.CreatedBy
                })
                .ToListAsync();

            return Ok(events);
        }


        // GET api/events/{id} – detalhes de um evento
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEvent(int id)
        {
            var ev = await _context.Events.FindAsync(id);
            if (ev == null)
                return NotFound("Evento não encontrado.");

            return Ok(ev);
        }

        // POST api/events – criação de evento (apenas para gestores)
        [HttpPost]
        public async Task<IActionResult> CreateEvent(CreateEventDto dto)
        {
            // Verifica se o usuário (criador) existe
            var user = await _context.Users.FindAsync(dto.CreatedBy);
            if (user == null)
            {
                return BadRequest("Usuário não encontrado para ser o criador do evento.");
            }

            var newEvent = new Event
            {
                Name = dto.Name,
                Description = dto.Description,
                Date = dto.Date,
                EndDate = dto.EndDate,
                Location = dto.Location,
                TotalTickets = dto.TotalTickets,
                AvailableTickets = dto.TotalTickets, // ou outra lógica
                ContactPhone = dto.ContactPhone,
                ContactEmail = dto.ContactEmail,
                CreatedBy = dto.CreatedBy
            };

            _context.Events.Add(newEvent);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Trate o erro conforme necessário
                return StatusCode(500, "Erro ao criar o evento.");
            }

            return CreatedAtAction(nameof(GetEvent), new { id = newEvent.Id }, newEvent);
        }


        // PUT api/events/{id} – atualização de evento
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(int id, [FromBody] UpdateEventDto dto)
        {
            var ev = await _context.Events.FindAsync(id);
            if (ev == null)
                return NotFound("Evento não encontrado.");

            ev.Name = dto.Name;
            ev.Description = dto.Description;
            ev.Date = dto.Date;
            ev.EndDate = dto.EndDate;
            ev.Location = dto.Location;
            ev.ContactPhone = dto.ContactPhone;
            ev.ContactEmail = dto.ContactEmail;

            _context.Events.Update(ev);
            await _context.SaveChangesAsync();

            return Ok(ev);
        }

        // DELETE api/events/{id} – exclusão de evento
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var ev = await _context.Events.FindAsync(id);
            if (ev == null)
                return NotFound("Evento não encontrado.");

            _context.Events.Remove(ev);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

}

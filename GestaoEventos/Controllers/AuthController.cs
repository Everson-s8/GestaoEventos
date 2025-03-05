using Microsoft.AspNetCore.Mvc;
using GestaoEventos.Data;
using Microsoft.EntityFrameworkCore;
using GestaoEventos.Models;
using GestaoEventos.DTO;

namespace GestaoEventos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
                return BadRequest("Email já está em uso.");

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = dto.Password, // Para fins de exemplo, a senha é armazenada "como está" (NÃO faça isso em produção)
                Role = dto.Role
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == dto.Email.ToLower() && u.PasswordHash == dto.Password);
            if (user == null)
                return Unauthorized("Credenciais inválidas.");

            // Se o usuário for funcionário (Role 2)
            if (user.Role == UserRole.Employee)
            {
                // Procura o registro correspondente na tabela EventStaffs
                var staff = await _context.EventStaffs
                    .FirstOrDefaultAsync(s => s.Email.ToLower() == user.Email.ToLower());

                if (staff == null)
                {
                    // Opcional: você pode criar o registro aqui ou retornar um erro
                    return BadRequest("Funcionário não encontrado. Verifique se o registro de EventStaff existe.");
                }

                return Ok(new
                {
                    user.Id,
                    user.Name,
                    user.Email,
                    user.Role,
                    eventId = staff.EventId
                });
            }

            // Para clientes e administradores
            return Ok(new
            {
                user.Id,
                user.Name,
                user.Email,
                user.Role
            });
        }


    }

}

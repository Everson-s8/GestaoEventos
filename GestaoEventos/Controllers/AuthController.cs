using Microsoft.AspNetCore.Mvc;
using GestaoEventos.Data;
using Microsoft.EntityFrameworkCore;
using GestaoEventos.Models;

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
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email && u.PasswordHash == dto.Password);
            if (user == null)
                return Unauthorized("Credenciais inválidas.");

            // Aqui, você poderia gerar um token JWT e retornar junto com os dados do usuário.
            return Ok(user);
        }
    }

    public class RegisterDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
    }

    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}

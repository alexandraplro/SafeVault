using Microsoft.AspNetCore.Mvc;
using SafeVault.Web.Security;
using SafeVault.Web.Services;

namespace SafeVault.Web.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _repo;
        private readonly TokenService _tokenService;

        public AuthController(IUserRepository repo, TokenService tokenService)
        {
            _repo = repo;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            string username;

            try
            {
                // Validate username safely
                username = request.CleanUsername;
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }

            // Query user from repository
            var user = _repo.GetUserByUsername(username);

            if (user == null)
                return Unauthorized("Invalid username or password");

            var storedHash = user["PasswordHash"]?.ToString();
            var role = user["Role"]?.ToString();

            if (storedHash == null || role == null)
                return Unauthorized("Invalid username or password");

            // Verify password
            if (!PasswordHasher.Verify(request.Password, storedHash))
                return Unauthorized("Invalid username or password");

            // Generate JWT
            var token = _tokenService.GenerateToken(username, role);

            return Ok(new { Token = token });
        }
    }

    public record LoginRequest(string Username, string Password)
    {
        public string CleanUsername => InputValidator.ValidateUsername(Username);
        public string CleanPassword => Password;
    }
}

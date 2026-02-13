using Microsoft.AspNetCore.Mvc;
using SafeVault.Web.Security;
using SafeVault.Web.Services;

namespace SafeVault.Web.Controllers
{
    [ApiController]
    [Route("auth")]
    public class RegisterController : ControllerBase
    {
        private readonly IUserRepository _repo;

        public RegisterController(IUserRepository repo)
        {
            _repo = repo;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            // Validate username
            string username;
            try
            {
                username = InputValidator.ValidateUsername(request.Username);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }

            // Check if user already exists
            var existing = _repo.GetUserByUsername(username);
            if (existing != null)
                return Conflict(new { error = "Username already exists." });

            // Hash password
            var passwordHash = PasswordHasher.Hash(request.Password);

            // Default role if none provided
            var role = string.IsNullOrWhiteSpace(request.Role)
                ? "user"
                : request.Role;

            // Insert user
            _repo.RegisterUser(username, request.Email, passwordHash, role);

            return Ok(new
            {
                message = "User registered successfully.",
                username,
                role
            });
        }
    }

    public record RegisterRequest(string Username, string Email, string Password, string Role);
}

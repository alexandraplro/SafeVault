using Microsoft.AspNetCore.Mvc;
using SafeVault.Web.Security;
using SafeVault.Web.Services;
using System.Text.Encodings.Web;

namespace SafeVault.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SubmitController : ControllerBase
    {
        private readonly IUserRepository _repo; 
        public SubmitController(IUserRepository repo)
        {
            _repo = repo;
        }

        [HttpPost]
        public IActionResult Submit([FromForm] string username, [FromForm] string email)
        {
            try
            {
                var safeUsername = InputValidator.ValidateUsername(username);
                var safeEmail = InputValidator.ValidateEmail(email);

                _repo.InsertUser(safeUsername, safeEmail);

                return Ok(new
                {
                    message = "User saved successfully.",
                    username = JavaScriptEncoder.Default.Encode(safeUsername),
                    email = JavaScriptEncoder.Default.Encode(safeEmail)
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}

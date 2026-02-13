using NUnit.Framework;
using SafeVault.Web.Security;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using SafeVault.Web.Services;
using Microsoft.AspNetCore.Mvc;
using SafeVault.Web.Controllers;

namespace SafeVault.Tests.AuthTests
{
    public class TestAuth
    {
        private TokenService _tokenService;

        [SetUp]
        public void Setup()
        {
            var inMemorySettings = new Dictionary<string, string?>
            {
                { "Jwt:Key", "THIS_IS_A_SECRET_KEY_CHANGE_IT_12345" },
                { "Jwt:Issuer", "SafeVault" },
                { "Jwt:Audience", "SafeVaultUsers" }
            };

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _tokenService = new TokenService(config);
        }

        [Test]
        public void AdminRole_ShouldBePresentInToken()
        {
            var token = _tokenService.GenerateToken("admin", "Admin");

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            var roleClaim = jwt.Claims.FirstOrDefault(c => c.Type == "role");

            Assert.That(roleClaim, Is.Not.Null);
            Assert.That(roleClaim!.Value, Is.EqualTo("Admin"));
        }

        [Test]
        public void UserRole_ShouldNotHaveAdminAccess()
        {
            var token = _tokenService.GenerateToken("bob", "User");

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            var roleClaim = jwt.Claims.FirstOrDefault(c => c.Type == "role");

            Assert.That(roleClaim, Is.Not.Null);
            Assert.That(roleClaim!.Value, Is.EqualTo("User"));
        }

        // ⭐ NEW TEST FOR ACTIVITY 3
        [Test]
        public void Login_ShouldReject_SQLInjectionAttempt()
        {
            var repo = new UserRepository(TestConfig.Config);
            var tokenService = new TokenService(TestConfig.Config);
            var controller = new AuthController(repo, tokenService);
        
            var request = new LoginRequest("'; DROP TABLE Users; --", "password");
        
            var result = controller.Login(request);
        
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }
        
    }
}

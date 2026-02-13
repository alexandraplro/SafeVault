using Microsoft.Extensions.Configuration;

namespace SafeVault.Tests
{
    public static class TestConfig
    {
        public static IConfiguration Config
        {
            get
            {
                var settings = new Dictionary<string, string?>
                {
                    // Connection string for UserRepository
                    { "ConnectionStrings:DefaultConnection", "Server=localhost;Database=SafeVaultTest;User Id=sa;Password=YourPassword123;Encrypt=False" },

                    // JWT settings for TokenService
                    { "Jwt:Key", "THIS_IS_A_SECRET_KEY_CHANGE_IT" },
                    { "Jwt:Issuer", "SafeVault" },
                    { "Jwt:Audience", "SafeVaultUsers" }
                };

                return new ConfigurationBuilder()
                    .AddInMemoryCollection(settings)
                    .Build();
            }
        }
    }
}

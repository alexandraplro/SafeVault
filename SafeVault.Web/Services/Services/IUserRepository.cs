namespace SafeVault.Web.Services;

public interface IUserRepository
{
    void InsertUser(string username, string email);
    Dictionary<string, object>? GetUserByUsername(string username);

    // NEW: Register user with password + role
    void RegisterUser(string username, string email, string passwordHash, string role);
}

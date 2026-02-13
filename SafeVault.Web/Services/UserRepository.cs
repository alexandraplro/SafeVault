using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace SafeVault.Web.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string not found.");
        }

        // ------------------------------------------------------------
        // INSERT USER (Assignment requirement: username + email only)
        // ------------------------------------------------------------
        public void InsertUser(string username, string email)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Users (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Username TEXT NOT NULL UNIQUE,
                    Email TEXT NOT NULL,
                    PasswordHash TEXT,
                    Role TEXT
                );

                INSERT INTO Users (Username, Email)
                VALUES ($username, $email);
            ";

            command.Parameters.AddWithValue("$username", username);
            command.Parameters.AddWithValue("$email", email);

            command.ExecuteNonQuery();
        }

        // ------------------------------------------------------------
        // GET USER BY USERNAME (Used by Login)
        // ------------------------------------------------------------
        public Dictionary<string, object>? GetUserByUsername(string username)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            // Ensure table exists
            var createCmd = connection.CreateCommand();
            createCmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS Users (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Username TEXT NOT NULL UNIQUE,
                    Email TEXT NOT NULL,
                    PasswordHash TEXT,
                    Role TEXT
                );
            ";
            createCmd.ExecuteNonQuery();

            // Query user
            var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT Username, Email, PasswordHash, Role
                FROM Users
                WHERE Username = $username
                LIMIT 1;
            ";

            command.Parameters.AddWithValue("$username", username);

            using var reader = command.ExecuteReader();

            if (!reader.Read())
                return null;

            return new Dictionary<string, object>
            {
                ["Username"] = reader["Username"],
                ["Email"] = reader["Email"],
                ["PasswordHash"] = reader["PasswordHash"],
                ["Role"] = reader["Role"]
            };
        }

        // ------------------------------------------------------------
        // REGISTER USER (username + email + passwordHash + role)
        // ------------------------------------------------------------
        public void RegisterUser(string username, string email, string passwordHash, string role)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            // Ensure table exists
            var createCmd = connection.CreateCommand();
            createCmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS Users (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Username TEXT NOT NULL UNIQUE,
                    Email TEXT NOT NULL,
                    PasswordHash TEXT NOT NULL,
                    Role TEXT NOT NULL
                );
            ";
            createCmd.ExecuteNonQuery();

            // Insert user
            var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Users (Username, Email, PasswordHash, Role)
                VALUES ($username, $email, $passwordHash, $role);
            ";

            command.Parameters.AddWithValue("$username", username);
            command.Parameters.AddWithValue("$email", email);
            command.Parameters.AddWithValue("$passwordHash", passwordHash);
            command.Parameters.AddWithValue("$role", role);

            command.ExecuteNonQuery();
        }
    }
}

using System;
using System.Text.RegularExpressions;
using System.Net;

namespace SafeVault.Web.Security
{
    public static class InputValidator
    {
        public static string ValidateUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username is required.");

            username = username.Trim();

            if (username.Length < 3 || username.Length > 50)
                throw new ArgumentException("Username must be between 3 and 50 characters.");

            var regex = new Regex("^[a-zA-Z0-9_.-]+$");
            if (!regex.IsMatch(username))
                throw new ArgumentException("Username contains invalid characters.");

            return username;
        }

        public static string ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required.");

            email = email.Trim();

            var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            if (!regex.IsMatch(email))
                throw new ArgumentException("Email format is invalid.");

            return email;
        }

        public static string EncodeForHtml(string input)
        {
            if (input == null) return string.Empty;

            return WebUtility.HtmlEncode(input);
        }
    }
}

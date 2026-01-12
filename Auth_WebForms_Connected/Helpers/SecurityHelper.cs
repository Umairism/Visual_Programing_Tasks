using System;
using System.Security.Cryptography;
using System.Text;

namespace Auth_WebForms_Connected.Helpers
{
    /// <summary>
    /// Security helper class for password hashing and validation
    /// </summary>
    public static class SecurityHelper
    {
        /// <summary>
        /// Hash password using SHA512
        /// </summary>
        public static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password));

            using (SHA512 sha512 = SHA512.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha512.ComputeHash(bytes);
                
                StringBuilder result = new StringBuilder();
                foreach (byte b in hash)
                {
                    result.Append(b.ToString("X2"));
                }
                
                return result.ToString();
            }
        }

        /// <summary>
        /// Verify password against hash
        /// </summary>
        public static bool VerifyPassword(string password, string hash)
        {
            string hashedPassword = HashPassword(password);
            return hashedPassword.Equals(hash, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Validate password strength
        /// </summary>
        public static bool IsPasswordStrong(string password)
        {
            if (string.IsNullOrEmpty(password) || password.Length < 8)
                return false;

            bool hasUpperCase = false;
            bool hasLowerCase = false;
            bool hasDigit = false;
            bool hasSpecialChar = false;

            foreach (char c in password)
            {
                if (char.IsUpper(c)) hasUpperCase = true;
                else if (char.IsLower(c)) hasLowerCase = true;
                else if (char.IsDigit(c)) hasDigit = true;
                else if (!char.IsLetterOrDigit(c)) hasSpecialChar = true;
            }

            return hasUpperCase && hasLowerCase && hasDigit && hasSpecialChar;
        }

        /// <summary>
        /// Get password strength message
        /// </summary>
        public static string GetPasswordStrengthMessage(string password)
        {
            if (string.IsNullOrEmpty(password))
                return "Password is required.";

            if (password.Length < 8)
                return "Password must be at least 8 characters long.";

            bool hasUpperCase = false;
            bool hasLowerCase = false;
            bool hasDigit = false;
            bool hasSpecialChar = false;

            foreach (char c in password)
            {
                if (char.IsUpper(c)) hasUpperCase = true;
                else if (char.IsLower(c)) hasLowerCase = true;
                else if (char.IsDigit(c)) hasDigit = true;
                else if (!char.IsLetterOrDigit(c)) hasSpecialChar = true;
            }

            if (!hasUpperCase)
                return "Password must contain at least one uppercase letter.";
            if (!hasLowerCase)
                return "Password must contain at least one lowercase letter.";
            if (!hasDigit)
                return "Password must contain at least one digit.";
            if (!hasSpecialChar)
                return "Password must contain at least one special character.";

            return "Password is strong.";
        }

        /// <summary>
        /// Validate email format
        /// </summary>
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}

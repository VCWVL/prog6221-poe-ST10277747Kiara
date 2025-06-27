using System;

namespace PartProg3
{
    public static class Authenticator
    {
        private static string? storedPassword = null;
        private static string? current2FACode = null;

        public static void SetPassword(string password)
        {
            storedPassword = password;
        }

        public static bool VerifyPassword(string input)
        {
            return storedPassword != null && input == storedPassword;
        }

        public static string Generate2FACode()
        {
            Random rand = new Random();
            current2FACode = rand.Next(100000, 999999).ToString();
            return current2FACode;
        }

        public static bool Verify2FACode(string inputCode)
        {
            return current2FACode != null && inputCode == current2FACode;
        }
    }
}

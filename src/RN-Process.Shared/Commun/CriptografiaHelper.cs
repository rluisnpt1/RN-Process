using System.Security.Cryptography;
using System.Text;

namespace RN_Process.Shared.Commun
{
    public static class CriptografiaHelper
    {
        public static byte[] PasswordCryptography(string password)
        {
            return Cryptography(password, "rnprosess-suluyds-swewgjgrfhjg-wedjgfnwjvn-85429");
        }

        public static byte[] Cryptography(string text, string salt)
        {
            while (salt.Length < 6) salt += salt + "Z";
            using (var sha = SHA512.Create())
            {
                salt = Encoding.UTF8.GetString(
                    sha.ComputeHash(Encoding.UTF8.GetBytes(salt.Substring(salt.Length - 5))));
                return sha.ComputeHash(Encoding.UTF8.GetBytes(text + salt));
            }
        }
    }
}
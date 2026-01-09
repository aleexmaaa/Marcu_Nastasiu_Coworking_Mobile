using System.Security.Cryptography;
using System.Text;

namespace Marcu_Nastasiu_Coworking_Mobile.Models
{
    public static class AuthHelpers
    {
        public static string HashPassword(string plainPassword)
        {
            plainPassword ??= "";

            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(plainPassword);
            var hash = sha.ComputeHash(bytes);

            var sb = new StringBuilder(hash.Length * 2);
            foreach (var b in hash)
                sb.Append(b.ToString("x2"));

            return sb.ToString();
        }
    }
}

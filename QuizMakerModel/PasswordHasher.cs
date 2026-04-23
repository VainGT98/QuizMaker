using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaker.QuizMakerModel
{
    public class PasswordHasher
    {
        public (string hash, string salt) HashPassword(string password)
        {
            byte[] saltBytes = RandomNumberGenerator.GetBytes(16);

            var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 100000, HashAlgorithmName.SHA256);
            byte[] hashBytes = pbkdf2.GetBytes(32);

            return (Convert.ToBase64String(hashBytes), Convert.ToBase64String(saltBytes));
        }

        public bool VerifyPassword(string password, string savedHash, string savedSalt)
        {
            byte[] saltBytes = Convert.FromBase64String(savedSalt);
            byte[] hashBytes = Convert.FromBase64String(savedHash);

            var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 100000, HashAlgorithmName.SHA256);
            byte[] testHash = pbkdf2.GetBytes(32);

            return CryptographicOperations.FixedTimeEquals(hashBytes, testHash);
        }
    }
}

using StudyFlow.Domain.Security.Cryptography;
using BC = BCrypt.Net;

namespace StudyFlow.Infrastructure.Security
{
    public class PasswordEncryption : IPasswordEncryption
    {
        public bool Decrypt(string password, string passwordHash)
        {
            return BC.BCrypt.Verify(password, passwordHash);
        }

        public string Encrypt(string password)
        {
            string passwordHash = BC.BCrypt.HashPassword(password);

            return passwordHash;
        }
    }
}

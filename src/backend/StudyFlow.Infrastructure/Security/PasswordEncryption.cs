using BC = BCrypt.Net;
using StudyFlow.Domain.Security.Cryptography;

namespace StudyFlow.Infrastructure.Security
{
    public class PasswordEncryption : IPasswordEncryption
    {
        public string Encrypt(string password)
        {
            string passwordHash = BC.BCrypt.HashPassword(password);

            return passwordHash;
        }
    }
}

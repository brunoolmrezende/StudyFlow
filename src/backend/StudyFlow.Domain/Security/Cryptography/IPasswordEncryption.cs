namespace StudyFlow.Domain.Security.Cryptography
{
    public interface IPasswordEncryption
    {
        public string Encrypt(string password);
        public bool Decrypt(string password, string passwordHash);
    }
}

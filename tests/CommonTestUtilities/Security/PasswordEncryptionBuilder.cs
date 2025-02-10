﻿using Moq;
using StudyFlow.Domain.Security.Cryptography;

namespace CommonTestUtilities.Security
{
    public class PasswordEncryptionBuilder
    {
        private readonly Mock<IPasswordEncryption> _mock;

        public PasswordEncryptionBuilder()
        {
            _mock = new Mock<IPasswordEncryption>();

            _mock.Setup(passwordEncrypter => passwordEncrypter.Encrypt(It.IsAny<string>())).Returns("!$%¨*sdabsad");
        }

        public IPasswordEncryption Build() => _mock.Object;
    }
}

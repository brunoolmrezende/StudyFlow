using StudyFlow.Communication.Requests;
using StudyFlow.Communication.Response;
using StudyFlow.Domain.Repositories.User;
using StudyFlow.Domain.Security.Cryptography;
using StudyFlow.Exceptions.ExceptionBase;

namespace StudyFlow.Application.UseCases.User.Login.DoLogin
{
    public class DoLoginUseCase : IDoLoginUseCase
    {
        private readonly IUserReadOnlyRepository _repository;
        private readonly IPasswordEncryption _encryption;

        public DoLoginUseCase(
            IUserReadOnlyRepository repository,
            IPasswordEncryption encryption)
        {
            _repository = repository;
            _encryption = encryption;
        }

        public async Task<ResponseRegisteredUserJson> Execute(RequestDoLoginJson request)
        {
            var user = await _repository.GetUserByEmail(request.Email) ?? throw new InvalidLoginException();

            var passwordMatch = _encryption.Decrypt(request.Password, user.Password);

            if (!passwordMatch)
            {
                throw new InvalidLoginException();
            }

            return new ResponseRegisteredUserJson
            {
                Name = user.Name,
            };
        }
    }
}

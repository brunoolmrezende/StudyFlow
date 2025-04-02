using StudyFlow.Communication.Requests;
using StudyFlow.Communication.Response;
using StudyFlow.Domain.Repositories;
using StudyFlow.Domain.Repositories.Token;
using StudyFlow.Domain.Repositories.User;
using StudyFlow.Domain.Security.Cryptography;
using StudyFlow.Domain.Security.Token;
using StudyFlow.Exceptions.ExceptionBase;

namespace StudyFlow.Application.UseCases.User.Login.DoLogin
{
    public class DoLoginUseCase : IDoLoginUseCase
    {
        private readonly IUserReadOnlyRepository _repository;
        private readonly IPasswordEncryption _encryption;
        private readonly IAccessTokenGenerator _accessTokenGenerator;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;
        private readonly ITokenRepository _tokenRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DoLoginUseCase(
            IUserReadOnlyRepository repository,
            IPasswordEncryption encryption,
            IAccessTokenGenerator accessTokenGenerator,
            IRefreshTokenGenerator refreshTokenGenerator,
            ITokenRepository tokenRepository,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _encryption = encryption;
            _accessTokenGenerator = accessTokenGenerator;
            _refreshTokenGenerator = refreshTokenGenerator;
            _tokenRepository = tokenRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseRegisteredUserJson> Execute(RequestDoLoginJson request)
        {
            var user = await _repository.GetUserByEmail(request.Email) ?? throw new InvalidLoginException();

            var passwordMatch = _encryption.Decrypt(request.Password, user.Password);

            if (!passwordMatch)
            {
                throw new InvalidLoginException();
            }

            var refreshToken = new Domain.Entities.RefreshToken
            {
                UserId = user.Id,
                Value = _refreshTokenGenerator.Generate()
            };

            await _tokenRepository.SaveNewRefreshToken(refreshToken);

            await _unitOfWork.Commit();

            return new ResponseRegisteredUserJson
            {
                Name = user.Name,
                Tokens = new ResponseTokenJson
                {
                    AccessToken = _accessTokenGenerator.GenerateToken(user.UserIdentifier),
                    RefreshToken = refreshToken.Value
                }
            };
        }
    }
}

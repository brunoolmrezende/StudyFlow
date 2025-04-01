using StudyFlow.Communication.Requests;
using StudyFlow.Communication.Response;
using StudyFlow.Domain.Repositories;
using StudyFlow.Domain.Repositories.Token;
using StudyFlow.Domain.Security.Token;
using StudyFlow.Domain.ValueObjects;
using StudyFlow.Exceptions.ExceptionBase;

namespace StudyFlow.Application.UseCases.Token.RefreshToken
{
    public class UseRefreshTokenUseCase : IUseRefreshTokenUseCase
    {
        private readonly ITokenRepository _tokenRepository;
        private readonly IAccessTokenGenerator _accessTokenGenerator;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;
        private readonly IUnitOfWork _unitOfWork;

        public UseRefreshTokenUseCase(
            ITokenRepository tokenRepository,
            IAccessTokenGenerator accessTokenGenerator,
            IRefreshTokenGenerator refreshTokenGenerator,
            IUnitOfWork unitOfWork)
        {
            _tokenRepository = tokenRepository;
            _accessTokenGenerator = accessTokenGenerator;
            _refreshTokenGenerator = refreshTokenGenerator;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseTokenJson> Execute(RequestNewTokenJson request)
        {
            var refreshToken = await _tokenRepository.GetToken(request.RefreshToken);

            if (refreshToken is null)
            {
                throw new RefreshTokenNotFoundException();
            }

            var tokenExpirationTime = refreshToken.CreatedAt.AddHours(StudyFlowRuleConstants.MAXIMUM_REFRESH_TOKEN_TIME_IN_HOURS);

            if (DateTime.Compare(tokenExpirationTime, DateTime.UtcNow) < 0)
            {
                throw new RefreshTokenExpiredException();
            }

            var newRefreshToken = new Domain.Entities.RefreshToken
            {
                Value = _refreshTokenGenerator.Generate(),
                UserId = refreshToken.UserId,
            };

            await _tokenRepository.SaveNewRefreshToken(newRefreshToken);

            await _unitOfWork.Commit();

            return new ResponseTokenJson
            {
                RefreshToken = newRefreshToken.Value,
                AccessToken = _accessTokenGenerator.GenerateToken(refreshToken.User.UserIdentifier)
            };
        }
    }
}

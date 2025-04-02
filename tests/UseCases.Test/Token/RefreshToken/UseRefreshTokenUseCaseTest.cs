using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Security;
using FluentAssertions;
using StudyFlow.Application.UseCases.Token.RefreshToken;
using StudyFlow.Communication.Requests;
using StudyFlow.Exceptions.ExceptionBase;
using StudyFlow.Exceptions;
using StudyFlow.Domain.ValueObjects;

namespace UseCases.Test.Token.RefreshToken
{
    public class UseRefreshTokenUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, _) = UserBuilder.Build();
            var refreshToken = RefreshTokenBuilder.Build(user);

            var useCase = CreateUseCase(refreshToken);

            var result = await useCase.Execute(new RequestNewTokenJson
            {
                RefreshToken = refreshToken.Value,
            });

            result.Should().NotBeNull();
            result.AccessToken.Should().NotBeNullOrEmpty();
            result.RefreshToken.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Error_Refresh_Token_Not_Found()
        {
            (var user, _) = UserBuilder.Build();
            var refreshToken = RefreshTokenBuilder.Build(user);

            var useCase = CreateUseCase();

            var act = async () => await useCase.Execute(new RequestNewTokenJson
            {
                RefreshToken = refreshToken.Value,
            });

            await act.Should().ThrowAsync<RefreshTokenNotFoundException>()
                .Where(e => e.GetErrorMessages().Count == 1 && e.GetErrorMessages().Contains(ResourceMessagesException.EXPIRED_SESSION));
        }

        [Fact]
        public async Task Error_Refresh_Token_Expired()
        {
            (var user, _) = UserBuilder.Build();
            var refreshToken = RefreshTokenBuilder.Build(user);
            refreshToken.CreatedAt = DateTime.UtcNow.AddHours(-StudyFlowRuleConstants.MAXIMUM_REFRESH_TOKEN_TIME_IN_HOURS);

            var useCase = CreateUseCase(refreshToken);

            var act = async () => await useCase.Execute(new RequestNewTokenJson
            {
                RefreshToken = refreshToken.Value,
            });

            await act.Should().ThrowAsync<RefreshTokenExpiredException>()
                .Where(e => e.GetErrorMessages().Count == 1 && e.GetErrorMessages().Contains(ResourceMessagesException.INVALID_SESSION));
        }

        private UseRefreshTokenUseCase CreateUseCase(StudyFlow.Domain.Entities.RefreshToken? refreshToken = null)
        {
            var tokenRepository = new TokenRepositoryBuilder().GetToken(refreshToken).Build();
            var refreshTokenGenerator = RefreshTokenGeneratorBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();
            var accessTokenGenerator = JwtTokenGeneratorBuilder.Build();

            return new UseRefreshTokenUseCase(tokenRepository, accessTokenGenerator, refreshTokenGenerator, unitOfWork);
        }
    }
}

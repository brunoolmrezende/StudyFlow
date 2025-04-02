using StudyFlow.Domain.Security.Token;
using StudyFlow.Infrastructure.Security.Token.Refresh;

namespace CommonTestUtilities.Security
{
    public class RefreshTokenGeneratorBuilder
    {
        public static IRefreshTokenGenerator Build() => new RefreshTokenGenerator();
    }
}

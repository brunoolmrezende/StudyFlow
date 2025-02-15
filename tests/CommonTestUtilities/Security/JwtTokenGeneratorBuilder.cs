using StudyFlow.Domain.Security.Token;
using StudyFlow.Infrastructure.Security.Token;

namespace CommonTestUtilities.Security
{
    public class JwtTokenGeneratorBuilder
    {
        public static IAccessTokenGenerator Build() => new AccessTokenGenerator(expirationTimeMinutes: 5, signInKey: "tttttttttttttttttttttttttttttttt");
    }
}

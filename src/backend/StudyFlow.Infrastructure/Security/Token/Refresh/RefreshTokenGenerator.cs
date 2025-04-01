using StudyFlow.Domain.Security.Token;

namespace StudyFlow.Infrastructure.Security.Token.Refresh
{
    public class RefreshTokenGenerator : IRefreshTokenGenerator
    {
        public string Generate()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }
    }
}

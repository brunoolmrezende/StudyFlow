namespace StudyFlow.Domain.Security.Token
{
    public interface IAccessTokenGenerator
    {
        public string GenerateToken(Guid userIdentifier);
    }
}

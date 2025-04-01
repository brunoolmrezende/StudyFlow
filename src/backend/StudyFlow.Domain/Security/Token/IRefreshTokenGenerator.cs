namespace StudyFlow.Domain.Security.Token
{
    public interface IRefreshTokenGenerator
    {
        public string Generate();
    }
}

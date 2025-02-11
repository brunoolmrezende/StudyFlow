namespace StudyFlow.Domain.Repositories.User
{
    public interface IUserReadOnlyRepository
    {
        Task<bool> IsEmailRegisteredAndActive(string email);
    }
}

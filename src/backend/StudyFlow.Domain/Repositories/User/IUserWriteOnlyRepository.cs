namespace StudyFlow.Domain.Repositories.User
{
    public interface IUserWriteOnlyRepository
    {
        Task Add(Domain.Entities.User user);
    }
}

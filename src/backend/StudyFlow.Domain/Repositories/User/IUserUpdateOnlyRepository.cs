namespace StudyFlow.Domain.Repositories.User
{
    public interface IUserUpdateOnlyRepository
    {
        Task<Domain.Entities.User> GetById(long id);
        void Update(Domain.Entities.User user);

    }
}

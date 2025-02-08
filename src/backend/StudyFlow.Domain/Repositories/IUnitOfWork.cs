namespace StudyFlow.Domain.Repositories
{
    public interface IUnitOfWork
    {
        Task Commit();
    }
}

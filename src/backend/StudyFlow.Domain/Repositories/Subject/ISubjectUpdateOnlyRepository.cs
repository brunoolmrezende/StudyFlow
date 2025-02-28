namespace StudyFlow.Domain.Repositories.Subject
{
    public interface ISubjectUpdateOnlyRepository
    {
        Task<Entities.Subject?> GetSubjectById(long id, Entities.User loggedUser);
        void Update(Entities.Subject subject);
    }
}

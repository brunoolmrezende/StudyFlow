namespace StudyFlow.Domain.Repositories.Subject
{
    public interface ISubjectWriteOnlyRepository
    {
        Task Add(Entities.Subject subject);
    }
}

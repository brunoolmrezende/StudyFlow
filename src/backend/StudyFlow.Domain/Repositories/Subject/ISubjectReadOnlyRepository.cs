namespace StudyFlow.Domain.Repositories.Subject
{
    public interface ISubjectReadOnlyRepository
    {
        Task<bool> IsSubjectCreatedAndActive(string name,Domain.Entities.User loggedUser);
    }
}

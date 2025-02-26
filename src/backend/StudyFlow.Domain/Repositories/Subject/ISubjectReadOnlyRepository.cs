namespace StudyFlow.Domain.Repositories.Subject
{
    public interface ISubjectReadOnlyRepository
    {
        Task<bool> IsSubjectCreatedAndActive(string name, Entities.User loggedUser);
        Task<IList<Entities.Subject>> GetAllSubjects(Entities.User loggedUser);
    }
}

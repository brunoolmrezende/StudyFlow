namespace StudyFlow.Domain.Repositories.Topic
{
    public interface ITopicReadOnlyRepository
    {
        Task<bool> IsTopicCreatedAndActive(string name, Entities.User loggedUser);
    }
}

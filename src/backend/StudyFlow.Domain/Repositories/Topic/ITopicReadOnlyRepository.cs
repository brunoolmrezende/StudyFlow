namespace StudyFlow.Domain.Repositories.Topic
{
    public interface ITopicReadOnlyRepository
    {
        Task<bool> IsTopicCreatedAndActive(string name, Entities.User loggedUser);
        Task<IList<Entities.Topic>> GetAllTopics(Entities.User loggedUser, bool? active);
    }
}

namespace StudyFlow.Domain.Repositories.Topic
{
    public interface ITopicReadOnlyRepository
    {
        Task<bool> IsTopicAlreadyCreated(string name, Entities.User loggedUser, long? topicId = null);
        Task<IList<Entities.Topic>> GetAllTopics(Entities.User loggedUser, bool? active);
        Task<Entities.Topic?> GetTopicById(long id, Entities.User loggedUser);
    }
}

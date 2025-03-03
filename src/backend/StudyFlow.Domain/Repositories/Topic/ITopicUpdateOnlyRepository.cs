namespace StudyFlow.Domain.Repositories.Topic
{
    public interface ITopicUpdateOnlyRepository
    {
        Task<Entities.Topic?> GetTopicById(long id, Entities.User loggedUser);
        void Update(Entities.Topic topic);
    }
}

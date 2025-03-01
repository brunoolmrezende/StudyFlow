namespace StudyFlow.Domain.Repositories.Topic
{
    public interface ITopicWriteOnlyRepository
    {
        Task Add(Entities.Topic topic);
    }
}

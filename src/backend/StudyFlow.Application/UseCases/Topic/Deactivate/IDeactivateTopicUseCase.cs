namespace StudyFlow.Application.UseCases.Topic.Deactivate
{
    public interface IDeactivateTopicUseCase
    {
        Task Execute(long id);
    }
}

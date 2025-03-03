using StudyFlow.Communication.Requests;

namespace StudyFlow.Application.UseCases.Topic.Update
{
    public interface IUpdateTopicUseCase
    {
        Task Execute(long id, RequestUpdateTopicJson request);
    }
}

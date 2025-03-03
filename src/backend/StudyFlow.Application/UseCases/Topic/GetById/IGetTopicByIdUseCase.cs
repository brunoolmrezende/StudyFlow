using StudyFlow.Communication.Response;

namespace StudyFlow.Application.UseCases.Topic.GetById
{
    public interface IGetTopicByIdUseCase
    {
        Task<ResponseTopicJson> Execute(long id);
    }
}

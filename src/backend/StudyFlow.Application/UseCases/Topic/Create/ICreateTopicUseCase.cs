using StudyFlow.Communication.Requests;
using StudyFlow.Communication.Response;

namespace StudyFlow.Application.UseCases.Topic.Create
{
    public interface ICreateTopicUseCase
    {
        Task<ResponseCreatedTopicJson> Execute(RequestCreateTopicJson request);
    }
}

using StudyFlow.Communication.Response;

namespace StudyFlow.Application.UseCases.Topic.GetAll
{
    public interface IGetAllTopicsUseCase
    {
        Task<ResponseTopicsJson> Execute(bool? active);
    }
}

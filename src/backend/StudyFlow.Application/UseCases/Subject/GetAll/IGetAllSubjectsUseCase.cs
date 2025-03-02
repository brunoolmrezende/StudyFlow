using StudyFlow.Communication.Response;

namespace StudyFlow.Application.UseCases.Subject.GetAll
{
    public interface IGetAllSubjectsUseCase
    {
        Task<ResponseSubjectsJson> Execute(bool? active);
    }
}

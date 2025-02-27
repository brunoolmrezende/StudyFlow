using StudyFlow.Communication.Response;

namespace StudyFlow.Application.UseCases.Subject.GetById
{
    public interface IGetSubjectByIdUseCase
    {
        Task<ResponseSubjectJson> Execute(long id);
    }
}

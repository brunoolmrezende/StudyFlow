using StudyFlow.Communication.Requests;
using StudyFlow.Communication.Response;

namespace StudyFlow.Application.UseCases.Subject.Create
{
    public interface ICreateSubjectUseCase
    {
        Task<ResponseCreatedSubjectJson> Execute(RequestCreateSubjectJson request);
    }
}

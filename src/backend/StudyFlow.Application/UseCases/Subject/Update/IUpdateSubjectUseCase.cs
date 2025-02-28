using StudyFlow.Communication.Requests;

namespace StudyFlow.Application.UseCases.Subject.Update
{
    public interface IUpdateSubjectUseCase
    {
        Task Execute(long id, RequestUpdateSubjectJson request);
    }
}

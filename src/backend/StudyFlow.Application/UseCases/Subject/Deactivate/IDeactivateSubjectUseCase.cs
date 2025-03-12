namespace StudyFlow.Application.UseCases.Subject.Deactivate
{
    public interface IDeactivateSubjectUseCase
    {
        Task Execute(long id);
    }
}

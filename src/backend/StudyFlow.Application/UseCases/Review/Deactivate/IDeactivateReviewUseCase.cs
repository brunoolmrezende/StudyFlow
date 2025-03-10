namespace StudyFlow.Application.UseCases.Review.Deactivate
{
    public interface IDeactivateReviewUseCase
    {
        Task Execute(long id);
    }
}

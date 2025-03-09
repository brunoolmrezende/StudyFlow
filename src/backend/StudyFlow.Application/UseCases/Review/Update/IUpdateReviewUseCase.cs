using StudyFlow.Communication.Requests;

namespace StudyFlow.Application.UseCases.Review.Update
{
    public interface IUpdateReviewUseCase
    {
        Task Execute(RequestUpdateReviewJson request, long reviewId);
    }
}

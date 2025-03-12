using StudyFlow.Communication.Response;

namespace StudyFlow.Application.UseCases.Review.GetAll
{
    public interface IGetAllReviewsUseCase
    {
        Task<ResponseReviewsJson> Execute(bool? active);
    }
}

using StudyFlow.Communication.Requests;
using StudyFlow.Communication.Response;

namespace StudyFlow.Application.UseCases.Review.Create
{
    public interface ICreateReviewUseCase
    {
        Task<ResponseCreatedReviewJson> Execute(RequestCreateReviewJson request);
    }
}

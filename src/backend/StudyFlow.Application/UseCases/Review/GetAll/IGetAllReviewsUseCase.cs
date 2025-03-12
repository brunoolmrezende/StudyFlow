using StudyFlow.Communication.Response;
using StudyFlow.Domain.Enums;

namespace StudyFlow.Application.UseCases.Review.GetAll
{
    public interface IGetAllReviewsUseCase
    {
        Task<ResponseReviewsJson> Execute(bool? active, IList<string>? status, IList<string>? difficulty);
    }
}

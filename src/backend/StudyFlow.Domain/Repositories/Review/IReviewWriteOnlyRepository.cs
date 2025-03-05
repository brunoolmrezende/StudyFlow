namespace StudyFlow.Domain.Repositories.Review
{
    public interface IReviewWriteOnlyRepository
    {
        Task Add(Entities.Review review);
    }
}

namespace StudyFlow.Domain.Repositories.Review
{
    public interface IReviewReadOnlyRepository
    {
        Task<Entities.Review?> GetReviewById(long id, Entities.User user);
    }
}

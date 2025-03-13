namespace StudyFlow.Domain.Repositories.Review
{
    public interface IReviewReadOnlyRepository
    {
        Task<IList<Entities.Review>> GetAllReviews(Entities.User loggedUser,  bool? active, IList<string>? status, IList<string>? difficulty);
        Task<Entities.Review?> GetReviewById(long id, Entities.User loggedUser);
    }
}

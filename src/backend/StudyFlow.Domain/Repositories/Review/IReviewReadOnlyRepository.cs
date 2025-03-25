namespace StudyFlow.Domain.Repositories.Review
{
    public interface IReviewReadOnlyRepository
    {
        Task<IList<Entities.Review>> GetAllReviews(Entities.User loggedUser,  bool? active, string? status, string? difficulty);
        Task<Entities.Review?> GetReviewById(long id, Entities.User loggedUser);
        Task<IList<Entities.Review>> GetReviewsForReminderAsync();
    }
}

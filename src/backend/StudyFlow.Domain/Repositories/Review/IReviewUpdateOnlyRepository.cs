namespace StudyFlow.Domain.Repositories.Review
{
    public interface IReviewUpdateOnlyRepository
    {
        Task<Entities.Review?> GetReviewById(long id, Entities.User loggedUser);
        void Update(Entities.Review review);
    }
}

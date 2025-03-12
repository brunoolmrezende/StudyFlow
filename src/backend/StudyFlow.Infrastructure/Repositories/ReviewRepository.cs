using Microsoft.EntityFrameworkCore;
using StudyFlow.Domain.Entities;
using StudyFlow.Domain.Repositories.Review;
using StudyFlow.Infrastructure.DataAccess;

namespace StudyFlow.Infrastructure.Repositories
{
    public class ReviewRepository : IReviewWriteOnlyRepository, IReviewReadOnlyRepository, IReviewUpdateOnlyRepository
    {
        private readonly StudyFlowDbContext _dbContext;

        public ReviewRepository(StudyFlowDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(Review review)
        {
            await _dbContext.Reviews.AddAsync(review);
        }

        async Task<Review?> IReviewReadOnlyRepository.GetReviewById(long id, User user)
        {
            return await _dbContext
                .Reviews
                .AsNoTracking()
                .Include(x => x.Topic)
                .FirstOrDefaultAsync(review => review.Id == id && review.UserId == user.Id);
        }

        async Task<Review?> IReviewUpdateOnlyRepository.GetReviewById(long id, User user)
        {
            return await _dbContext
                .Reviews
                .FirstOrDefaultAsync(review => review.Id == id && review.UserId == user.Id);
        }

        public void Update(Review review)
        {
            _dbContext.Reviews.Update(review);
        }

        public async Task<IList<Review>> GetAllReviews(User loggedUser, bool? active)
        {
            var query = _dbContext
                .Reviews
                .AsNoTracking()
                .Include(x => x.Topic)
                .Where(review => review.UserId == loggedUser.Id);

            if (active.HasValue)
            {
                query = query.Where(review => review.Active == active.Value);
            }

            return await query
                .OrderBy(review => review.ScheduledDate)
                .ToListAsync();        
        }
    }
}

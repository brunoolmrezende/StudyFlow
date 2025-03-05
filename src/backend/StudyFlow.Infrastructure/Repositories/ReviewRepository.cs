using Microsoft.EntityFrameworkCore;
using StudyFlow.Domain.Entities;
using StudyFlow.Domain.Repositories.Review;
using StudyFlow.Infrastructure.DataAccess;

namespace StudyFlow.Infrastructure.Repositories
{
    public class ReviewRepository : IReviewWriteOnlyRepository, IReviewReadOnlyRepository
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

        public async Task<Review?> GetReviewById(long id, User user)
        {
            return await _dbContext
                .Reviews
                .AsNoTracking()
                .Include(x => x.Topic)
                .FirstOrDefaultAsync(review => review.Id == id && review.UserId == user.Id);
        }
    }
}

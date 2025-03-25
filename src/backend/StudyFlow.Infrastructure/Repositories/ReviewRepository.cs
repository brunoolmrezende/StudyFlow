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

        async Task<Review?> IReviewReadOnlyRepository.GetReviewById(long id, User loggedUser)
        {
            return await _dbContext
                .Reviews
                .AsNoTracking()
                .Include(x => x.Topic)
                .FirstOrDefaultAsync(review => review.Id == id && review.UserId == loggedUser.Id);
        }

        async Task<Review?> IReviewUpdateOnlyRepository.GetReviewById(long id, User loggedUser)
        {
            return await _dbContext
                .Reviews
                .FirstOrDefaultAsync(review => review.Id == id && review.UserId == loggedUser.Id);
        }

        public void Update(Review review)
        {
            _dbContext.Reviews.Update(review);
        }

        public async Task<IList<Review>> GetAllReviews(User loggedUser, bool? active, string? status, string? difficulty)
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

            if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<Domain.Enums.ReviewStatus>(status, out var reviewStatus))
            {
                query = query.Where(review => review.Status == reviewStatus);
            }

            if (!string.IsNullOrWhiteSpace(difficulty) && Enum.TryParse<Domain.Enums.DifficultyLevel>(difficulty, out var reviewDifficulty))
            {
                query = query.Where(review => review.Difficulty == reviewDifficulty);
            }

            return await query
                .OrderBy(review => review.ScheduledDate)
                .ToListAsync();
        }

        public async Task<IList<Review>> GetReviewsForReminderAsync()
        {
            var now = DateTime.UtcNow;
            var notificationLeadTime = TimeSpan.FromHours(12);

            var reminderTime = now.Add(notificationLeadTime);

            var query = _dbContext
                .Reviews
                .AsNoTracking()
                .Include(x => x.Topic)
                .Include(x => x.User)
                .Where(review => review.ScheduledDate >= now && review.ScheduledDate <= reminderTime && review.Status == Domain.Enums.ReviewStatus.Pending);

            return await query.ToListAsync();
        }
    }
}

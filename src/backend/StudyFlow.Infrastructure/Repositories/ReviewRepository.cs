using Microsoft.EntityFrameworkCore;
using StudyFlow.Domain.Entities;
using StudyFlow.Domain.Enums;
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

        public async Task<IList<Review>> GetAllReviews(User loggedUser, bool? active, IList<string>? status, IList<string>? difficulty)
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

            if (difficulty is not null && difficulty.Any())
            {
                var difficultyList = difficulty
                    .Select(difficulty => Enum.TryParse<DifficultyLevel>(difficulty.Trim(), true, out var parsedDifficulty) ? parsedDifficulty : (DifficultyLevel?)null)
                    .Where(difficulty => difficulty.HasValue)
                    .Select(difficulty => difficulty.Value)
                    .ToList();

                if (difficultyList.Count == 1)
                {
                    query = query.Where(review => review.Difficulty == difficultyList.First());
                }
                else
                {
                    var difficultyQuery = difficultyList.AsQueryable();

                    query = query.Where(review => difficultyQuery.Contains(review.Difficulty));
                }
            }

            if (status is not null && status.Any())
            {
                var statusList = status
                    .Select(status => Enum.TryParse<ReviewStatus>(status.Trim(), true, out var parsedStatus) ? parsedStatus : (ReviewStatus?)null)
                    .Where(status => status.HasValue)
                    .Select(status => status.Value)
                    .ToList();

                if (statusList.Count == 1)
                {
                    query = query.Where(review => review.Status == statusList.First());
                }
                else
                {
                    var statusQuery = statusList.AsQueryable();

                    query = query.Where(review => statusQuery.Contains(review.Status));
                }
            }

            return await query
                .OrderBy(review => review.ScheduledDate)
                .ToListAsync();        
        }
    }
}

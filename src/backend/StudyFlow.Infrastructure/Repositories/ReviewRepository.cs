using System.Linq.Expressions;
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

            ApplyEnumFilter(ref query, difficulty, d => d.Difficulty);
            ApplyEnumFilter(ref query, status, s => s.Status);
            
            return await query
                .OrderBy(review => review.ScheduledDate)
                .ToListAsync();        
        }

        private void ApplyEnumFilter<TEnum>(
            ref IQueryable<Review> query,
            IList<string>? values,
            Expression<Func<Review, TEnum>> propertySelector)
            where TEnum : struct, Enum
        {
            if (values is null || !values.Any()) return;

            var enumList = values
                 .Select(value => Enum.TryParse<TEnum>(value.Trim(), true, out var parsedEnum) ? parsedEnum : (TEnum?)null)
                 .Where(e => e.HasValue)
                 .Select(e => e.Value)
                 .ToList();

            if (enumList.Count == 0) return;

            query = enumList.Count == 1
                ? query.Where(review => propertySelector.Compile()(review).Equals(enumList.First()))
                : query.Where(review => enumList.Contains(propertySelector.Compile()(review)));
        }
    }
}

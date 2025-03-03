using Microsoft.EntityFrameworkCore;
using StudyFlow.Domain.Entities;
using StudyFlow.Domain.Repositories.Topic;
using StudyFlow.Infrastructure.DataAccess;

namespace StudyFlow.Infrastructure.Repositories
{
    public class TopicRepository : ITopicWriteOnlyRepository, ITopicReadOnlyRepository
    {
        private readonly StudyFlowDbContext _dbContext;

        public TopicRepository(StudyFlowDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(Topic topic)
        {
            await _dbContext.Topics.AddAsync(topic);
        }

        public async Task<IList<Topic>> GetAllTopics(User loggedUser, bool? active)
        {
            var query = _dbContext
                .Topics
                .AsNoTracking()
                .Where(topic => topic.UserId == loggedUser.Id);

            if (active.HasValue)
            {
                query = query.Where(topic => topic.Active == active.Value);
            }

            return await query
                .OrderBy(topic => topic.Name)
                .ToListAsync();
        }

        public async Task<Topic?> GetTopicById(long id, User loggedUser)
        {
            return await _dbContext
                .Topics
                .AsNoTracking()
                .FirstOrDefaultAsync(topic => topic.Id == id && topic.UserId == loggedUser.Id);
        }

        public async Task<bool> IsTopicCreatedAndActive(string name, User loggedUser)
        {
            return await _dbContext
                .Topics
                .AnyAsync(topic => topic.Name.ToLower() == name.ToLower() && topic.UserId == loggedUser.Id && topic.Active);
        }
    }
}

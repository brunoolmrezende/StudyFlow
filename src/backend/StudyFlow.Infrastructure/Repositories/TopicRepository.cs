using Microsoft.EntityFrameworkCore;
using StudyFlow.Domain.Entities;
using StudyFlow.Domain.Repositories.Topic;
using StudyFlow.Infrastructure.DataAccess;

namespace StudyFlow.Infrastructure.Repositories
{
    public class TopicRepository : ITopicWriteOnlyRepository, ITopicReadOnlyRepository, ITopicUpdateOnlyRepository
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

        async Task<Topic?> ITopicReadOnlyRepository.GetTopicById(long id, User loggedUser)
        {
            return await _dbContext
                .Topics
                .AsNoTracking()
                .FirstOrDefaultAsync(topic => topic.Id == id && topic.UserId == loggedUser.Id);
        }

        public async Task<bool> IsTopicAlreadyCreated(string name, User loggedUser, long? topicId = null)
        {
            return await _dbContext
                .Topics
                .AnyAsync(topic => topic.Name.ToLower() == name.ToLower() 
                                   && topic.UserId == loggedUser.Id 
                                   && (!topicId.HasValue || topic.Id != topicId));
        }

        public void Update(Topic topic)
        {
            _dbContext.Topics.Update(topic);
        }

        public async Task<Topic?> GetTopicById(long id, User loggedUser)
        {
            return await _dbContext
                .Topics
                .FirstOrDefaultAsync(topic => topic.Id == id && topic.UserId == loggedUser.Id);
        }
    }
}

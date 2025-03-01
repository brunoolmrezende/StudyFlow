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

        public async Task<bool> IsTopicCreatedAndActive(string name, User loggedUser)
        {
            return await _dbContext
                .Topics
                .AnyAsync(topic => topic.Name.ToLower() == name.ToLower() && topic.UserId == loggedUser.Id && topic.Active);
        }
    }
}

using StudyFlow.Domain.Repositories;

namespace StudyFlow.Infrastructure.DataAccess
{
    public class UnitOfWork(StudyFlowDbContext dbContext) : IUnitOfWork
    {
        private readonly StudyFlowDbContext _dbContext = dbContext;

        public async Task Commit() => await _dbContext.SaveChangesAsync();      
    }
}

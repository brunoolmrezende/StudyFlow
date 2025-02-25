using Microsoft.EntityFrameworkCore;
using StudyFlow.Domain.Entities;
using StudyFlow.Domain.Repositories.Subject;
using StudyFlow.Infrastructure.DataAccess;

namespace StudyFlow.Infrastructure.Repositories
{
    public class SubjectRepository : ISubjectWriteOnlyRepository, ISubjectReadOnlyRepository
    {
        private readonly StudyFlowDbContext _dbContext;

        public SubjectRepository(StudyFlowDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(Subject subject)
        {
            await _dbContext.Subjects.AddAsync(subject);
        }

        public async Task<bool> IsSubjectCreatedAndActive(string name, Domain.Entities.User loggedUser)
        {
           return await _dbContext
                .Subjects
                .AnyAsync(subject => subject.Name.ToLower() == name.ToLower() && subject.UserId == loggedUser.Id && subject.Active);
        }
    }
}

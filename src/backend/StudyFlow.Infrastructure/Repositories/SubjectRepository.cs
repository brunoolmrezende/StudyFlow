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

        public async Task<IList<Subject>> GetAllSubjects(User loggedUser)
        {
            return await _dbContext
                .Subjects
                .AsNoTracking()
                .Where(subject => subject.UserId == loggedUser.Id && subject.Active)
                .OrderBy(subject => subject.Name)
                .ToListAsync();
        }

        public async Task<Subject?> GetSubjectById(long id, User loggedUser)
        {
            return await _dbContext
                .Subjects
                .AsNoTracking()
                .FirstOrDefaultAsync(subject => subject.Id == id && subject.UserId == loggedUser.Id && subject.Active);
        }

        public async Task<bool> IsSubjectCreatedAndActive(string name, User loggedUser)
        {
           return await _dbContext
                .Subjects
                .AnyAsync(subject => subject.Name.ToLower() == name.ToLower() && subject.UserId == loggedUser.Id && subject.Active);
        }
    }
}

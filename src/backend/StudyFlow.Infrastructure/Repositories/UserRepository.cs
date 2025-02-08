using Microsoft.EntityFrameworkCore;
using StudyFlow.Domain.Entities;
using StudyFlow.Domain.Repositories.User;
using StudyFlow.Infrastructure.DataAccess;

namespace StudyFlow.Infrastructure.Repositories
{
    public class UserRepository(StudyFlowDbContext dbContext) : IUserReadOnlyRepository, IUserWriteOnlyRepository
    {
        private readonly StudyFlowDbContext _dbContext = dbContext;

        public async Task Add(User user)
        {
            await _dbContext.Users.AddAsync(user);
        }

        public async Task<bool> ExistActiveUserWithEmail(string email)
        {
            return await _dbContext.Users.AnyAsync(user => user.Email == email);
        }
    }
}

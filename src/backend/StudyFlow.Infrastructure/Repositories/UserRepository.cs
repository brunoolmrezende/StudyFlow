using Microsoft.EntityFrameworkCore;
using StudyFlow.Domain.Entities;
using StudyFlow.Domain.Repositories.User;
using StudyFlow.Infrastructure.DataAccess;

namespace StudyFlow.Infrastructure.Repositories
{
    public class UserRepository : IUserReadOnlyRepository, IUserWriteOnlyRepository
    {
        private readonly StudyFlowDbContext _dbContext;

        public UserRepository(StudyFlowDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(User user)
        {
            await _dbContext.Users.AddAsync(user);
        }

        public async Task<bool> IsEmailRegisteredAndActive(string email)
        {
            return await _dbContext
                .Users
                .AnyAsync(user => user.Email == email && user.Active);
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _dbContext
                .Users
                .AsNoTracking()
                .FirstOrDefaultAsync(user => user.Email == email && user.Active);
        }
    }
}

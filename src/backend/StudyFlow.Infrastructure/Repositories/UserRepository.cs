using Microsoft.EntityFrameworkCore;
using StudyFlow.Domain.Entities;
using StudyFlow.Domain.Repositories.User;
using StudyFlow.Infrastructure.DataAccess;

namespace StudyFlow.Infrastructure.Repositories
{
    public class UserRepository : IUserReadOnlyRepository, IUserWriteOnlyRepository, IUserUpdateOnlyRepository
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

        public async Task<bool> ExistActiveUserWithUserIdentifier(Guid userIdentifier)
        {
            return await _dbContext
                .Users
                .AnyAsync(user => user.UserIdentifier == userIdentifier && user.Active);
        }

        public async Task<User> GetById(long id)
        {
            return await _dbContext
                .Users
                .FirstAsync(user => user.Id == id);
        }

        public void Update(User user)
        {
            _dbContext.Users.Update(user);
        }
    }
}

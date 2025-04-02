using Microsoft.EntityFrameworkCore;
using StudyFlow.Domain.Entities;
using StudyFlow.Domain.Repositories.Token;
using StudyFlow.Infrastructure.DataAccess;

namespace StudyFlow.Infrastructure.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly StudyFlowDbContext _dbContext;

        public TokenRepository(StudyFlowDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<RefreshToken?> GetToken(string refreshToken)
        {
            return await _dbContext
                .RefreshTokens
                .AsNoTracking()
                .Include(token => token.User)
                .FirstOrDefaultAsync(token => token.Value == refreshToken);
        }

        public async Task SaveNewRefreshToken(RefreshToken refreshToken)
        {
            var tokens = _dbContext.RefreshTokens.Where(token => token.UserId == refreshToken.UserId);

            _dbContext.RefreshTokens.RemoveRange(tokens);

            await _dbContext.RefreshTokens.AddAsync(refreshToken);
        }
    }
}

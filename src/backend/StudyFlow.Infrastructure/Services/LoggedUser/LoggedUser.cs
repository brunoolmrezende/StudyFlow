using Microsoft.EntityFrameworkCore;
using StudyFlow.Domain.Entities;
using StudyFlow.Domain.Security.Token;
using StudyFlow.Domain.Services.LoggedUser;
using StudyFlow.Infrastructure.DataAccess;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace StudyFlow.Infrastructure.Services.LoggedUser
{
    public class LoggedUser : ILoggedUser
    {
        private readonly ITokenProvider _tokenProvider;
        private readonly StudyFlowDbContext _dbContext;

        public LoggedUser(ITokenProvider tokenProvider, StudyFlowDbContext dbContext)
        {
            _tokenProvider = tokenProvider;
            _dbContext = dbContext;
        }

        public async Task<User> GetLoggedUser()
        {
            var token = _tokenProvider.GetTokenValue();

            var tokenHandler = new JwtSecurityTokenHandler();

            var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

            var identifier = jwtSecurityToken.Claims.First(c => c.Type == ClaimTypes.Sid).Value;

            var userIdentifier = Guid.Parse(identifier);

            return await _dbContext
                .Users
                .AsNoTracking()
                .FirstAsync(user => user.UserIdentifier == userIdentifier && user.Active);
        }
    }
}

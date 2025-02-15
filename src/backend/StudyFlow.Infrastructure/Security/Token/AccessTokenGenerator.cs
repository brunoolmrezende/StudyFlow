using Microsoft.IdentityModel.Tokens;
using StudyFlow.Domain.Security.Token;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudyFlow.Infrastructure.Security.Token
{
    public class AccessTokenGenerator : IAccessTokenGenerator
    {
        private readonly int _expirationTimeMinutes;
        private readonly string _securityKey;

        public AccessTokenGenerator(int expirationTimeMinutes, string signInKey)
        {
            _expirationTimeMinutes = expirationTimeMinutes;
            _securityKey = signInKey;
        }

        public string GenerateToken(Guid userIdentifier)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Sid, userIdentifier.ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_expirationTimeMinutes),
                SigningCredentials = new SigningCredentials(SecurityKey(), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(securityToken);
        }

        private SymmetricSecurityKey SecurityKey()
        {
            var bytes = Encoding.UTF8.GetBytes(_securityKey);

            return new SymmetricSecurityKey(bytes);
        }
    }
}

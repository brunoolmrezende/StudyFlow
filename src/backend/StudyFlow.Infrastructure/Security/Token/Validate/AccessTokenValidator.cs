using Microsoft.IdentityModel.Tokens;
using StudyFlow.Domain.Security.Token;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace StudyFlow.Infrastructure.Security.Token.Validate
{
    public class AccessTokenValidator : JwtTokenHandler, IAccessTokenValidator
    {
        private readonly string _signInKey;

        public AccessTokenValidator(string signInKey)
        {
            _signInKey = signInKey;
        }

        public Guid ValidateAndGetUserIdentifier(string token)
        {
            var validationParameter = new TokenValidationParameters
            {
                ClockSkew = new TimeSpan(0),
                ValidateAudience = false,
                ValidateIssuer = false,
                IssuerSigningKey = SecurityKey(_signInKey),
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var principal = tokenHandler.ValidateToken(token, validationParameter, out _);

            var userIdentifier = principal.Claims.First(c => c.Type == ClaimTypes.Sid).Value;

            return Guid.Parse(userIdentifier);
        }
    }
}

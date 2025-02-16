using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace StudyFlow.Infrastructure.Security.Token
{
    public abstract class JwtTokenHandler
    {
        protected static SymmetricSecurityKey SecurityKey(string securityKey)
        {
            var bytes = Encoding.UTF8.GetBytes(securityKey);

            return new SymmetricSecurityKey(bytes);
        }
    }
}

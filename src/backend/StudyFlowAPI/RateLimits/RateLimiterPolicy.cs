using Microsoft.AspNetCore.RateLimiting;
using StudyFlow.Communication.Response;
using StudyFlow.Exceptions;
using StudyFlow.Infrastructure.Extensions;
using System.Net;
using System.Threading.RateLimiting;

namespace StudyFlow.API.RateLimits
{
    public class RateLimiterPolicy : IRateLimiterPolicy<string>
    {
        public Func<OnRejectedContext, CancellationToken, ValueTask>? OnRejected => async (context, cancellationToken) =>
        {
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
            await context.HttpContext.Response.WriteAsJsonAsync(new ResponseErrorJson(ResourceMessagesException.TOO_MANY_REQUESTS), cancellationToken);
        };

        public RateLimitPartition<string> GetPartition(HttpContext httpContext)
        {
            var configuration = httpContext.RequestServices.GetRequiredService<IConfiguration>();

            bool isTestEnvironment = configuration.IsUnitTestEnviroment();

            string clientIp = isTestEnvironment 
                ? "127.0.0.1" 
                : httpContext.Connection.RemoteIpAddress!.ToString();

            return RateLimitPartition.GetFixedWindowLimiter(clientIp, _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 4,
                QueueLimit = 0,
                Window = TimeSpan.FromSeconds(12)
            });
        }
    }
}

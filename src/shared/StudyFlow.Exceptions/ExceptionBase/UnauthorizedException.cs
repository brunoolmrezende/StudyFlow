using System.Net;

namespace StudyFlow.Exceptions.ExceptionBase
{
    public class UnauthorizedException : StudyFlowException
    {
        public UnauthorizedException(string message) : base(message)
        {
        }

        public override IList<string> GetErrorMessages() => [Message];

        public override HttpStatusCode GetHttpStatusCode() => HttpStatusCode.Unauthorized;
    }
}

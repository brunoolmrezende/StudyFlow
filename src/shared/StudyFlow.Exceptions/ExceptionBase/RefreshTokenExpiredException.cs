using System.Net;

namespace StudyFlow.Exceptions.ExceptionBase
{
    public class RefreshTokenExpiredException : StudyFlowException
    {
        public RefreshTokenExpiredException() : base(ResourceMessagesException.INVALID_SESSION)
        {
        }

        public override IList<string> GetErrorMessages() => [Message];

        public override HttpStatusCode GetHttpStatusCode() => HttpStatusCode.Forbidden;
    }
}

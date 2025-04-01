using System.Net;

namespace StudyFlow.Exceptions.ExceptionBase
{
    public class RefreshTokenNotFoundException : StudyFlowException
    {
        public RefreshTokenNotFoundException() : base(ResourceMessagesException.EXPIRED_SESSION)
        {
        }

        public override IList<string> GetErrorMessages() => [Message];

        public override HttpStatusCode GetHttpStatusCode() => HttpStatusCode.Unauthorized;
    }
}

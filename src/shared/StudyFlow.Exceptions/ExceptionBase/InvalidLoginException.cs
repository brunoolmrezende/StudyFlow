using System.Net;

namespace StudyFlow.Exceptions.ExceptionBase
{
    public class InvalidLoginException : StudyFlowException
    {
        public InvalidLoginException() : base(ResourceMessagesException.INVALID_EMAIL_OR_PASSWORD) { }

        public override IList<string> GetErrorMessages() => [Message];

        public override HttpStatusCode GetHttpStatusCode() => HttpStatusCode.Unauthorized;
    }
}

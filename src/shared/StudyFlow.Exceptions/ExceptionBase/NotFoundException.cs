using System.Net;

namespace StudyFlow.Exceptions.ExceptionBase
{
    public class NotFoundException : StudyFlowException
    {
        public NotFoundException(string message) : base(message)
        {
        }

        public override IList<string> GetErrorMessages() => [Message];

        public override HttpStatusCode GetHttpStatusCode() => HttpStatusCode.NotFound;
    }
}

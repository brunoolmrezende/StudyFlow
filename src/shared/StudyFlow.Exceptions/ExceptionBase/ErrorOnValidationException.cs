using System.Net;

namespace StudyFlow.Exceptions.ExceptionBase
{
    public class ErrorOnValidationException : StudyFlowException
    {
        private readonly IList<string> _errorMessages;

        public ErrorOnValidationException(IList<string> errors) : base(string.Empty) 
        { 
            _errorMessages = errors;
        }

        public override IList<string> GetErrorMessages() => _errorMessages;
        public override HttpStatusCode GetHttpStatusCode() => HttpStatusCode.BadRequest;
    }
}

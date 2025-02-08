using System.Net;

namespace StudyFlow.Exceptions.ExceptionBase
{
    public abstract class StudyFlowException(string message) : SystemException(message)
    {
        public abstract IList<string> GetErrorMessages();
        public abstract HttpStatusCode GetHttpStatusCode();
    }
}

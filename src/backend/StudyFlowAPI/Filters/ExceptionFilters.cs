using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using StudyFlow.Communication.Response;
using StudyFlow.Exceptions;
using StudyFlow.Exceptions.ExceptionBase;

namespace StudyFlow.API.Filters
{
    public class ExceptionFilters : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is StudyFlowException studyFlowException)
            {
                HandleProjectExceptions(context, studyFlowException);
            }
            
            else
            {
                ThrowUnknownException(context);
            }
        }

        private static void HandleProjectExceptions(ExceptionContext context, StudyFlowException studyFlowException)
        {
            context.HttpContext.Response.StatusCode = (int)studyFlowException.GetHttpStatusCode();
            context.Result = new ObjectResult(new ResponseErrorJson(studyFlowException.GetErrorMessages()));
        }

        private static void ThrowUnknownException(ExceptionContext context)
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Result = new ObjectResult(new ResponseErrorJson(ResourceMessagesException.UNKNOWN_ERROR));
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using StudyFlow.API.Attribute;
using StudyFlow.Application.UseCases.Subject.Create;
using StudyFlow.Communication.Requests;
using StudyFlow.Communication.Response;
using StudyFlow.Exceptions.ExceptionBase;

namespace StudyFlow.API.Controllers
{
    [AuthenticatedUser]
    public class SubjectController : StudyFlowBaseController
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseCreatedSubjectJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorOnValidationException), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(
            [FromServices] ICreateSubjectUseCase useCase,
            [FromBody] RequestCreateSubjectJson request)
        {
            var response = await useCase.Execute(request);

            return Created(string.Empty, response);
        }

    }
}

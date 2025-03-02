using Microsoft.AspNetCore.Mvc;
using StudyFlow.API.Attribute;
using StudyFlow.API.Binders;
using StudyFlow.Application.UseCases.Subject.Create;
using StudyFlow.Application.UseCases.Subject.GetAll;
using StudyFlow.Application.UseCases.Subject.GetById;
using StudyFlow.Application.UseCases.Subject.Update;
using StudyFlow.Communication.Requests;
using StudyFlow.Communication.Response;

namespace StudyFlow.API.Controllers
{
    [AuthenticatedUser]
    public class SubjectController : StudyFlowBaseController
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseCreatedSubjectJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(
            [FromServices] ICreateSubjectUseCase useCase,
            [FromBody] RequestCreateSubjectJson request)
        {
            var response = await useCase.Execute(request);

            return Created(string.Empty, response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ResponseSubjectsJson), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAll(
            [FromServices] IGetAllSubjectsUseCase useCase,
            [FromQuery] bool? active)
        {
            var response = await useCase.Execute(active);

            if (response.Subjects.Any())
            {
                return Ok(response);
            }

            return NoContent();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseSubjectJson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(
            [FromServices] IGetSubjectByIdUseCase useCase,
            [FromRoute][ModelBinder(typeof(StudyFlowBinder))] long id)
        {
            var response = await useCase.Execute(id);

            return Ok(response);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(
            [FromServices] IUpdateSubjectUseCase useCase,
            [FromRoute][ModelBinder(typeof(StudyFlowBinder))] long id,
            [FromBody] RequestUpdateSubjectJson request)
        {
            await useCase.Execute(id, request);

            return NoContent();
        }

    }
}

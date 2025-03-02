using Microsoft.AspNetCore.Mvc;
using StudyFlow.API.Attribute;
using StudyFlow.Application.UseCases.Topic.Create;
using StudyFlow.Application.UseCases.Topic.GetAll;
using StudyFlow.Communication.Requests;
using StudyFlow.Communication.Response;

namespace StudyFlow.API.Controllers
{
    [AuthenticatedUser]
    public class TopicController : StudyFlowBaseController
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseCreatedTopicJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(
            [FromServices] ICreateTopicUseCase useCase,
            [FromBody] RequestCreateTopicJson request)
        {
            var response = await useCase.Execute(request);

            return Created(string.Empty, response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ResponseTopicsJson), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAll(
            [FromServices] IGetAllTopicsUseCase useCase,
            [FromQuery] bool? active = true)
        {
            var response = await useCase.Execute(active);

            if (response.Topics.Any())
            {
                return Ok(response);
            }

            return NoContent();
        }   
    }
}

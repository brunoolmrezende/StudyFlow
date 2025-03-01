using Microsoft.AspNetCore.Mvc;
using StudyFlow.API.Attribute;
using StudyFlow.Application.UseCases.Topic;
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
    }
}

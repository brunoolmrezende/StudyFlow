using Microsoft.AspNetCore.Mvc;
using StudyFlow.API.Attribute;
using StudyFlow.API.Binders;
using StudyFlow.Application.UseCases.Review.Create;
using StudyFlow.Application.UseCases.Review.Deactivate;
using StudyFlow.Application.UseCases.Review.GetAll;
using StudyFlow.Application.UseCases.Review.Update;
using StudyFlow.Communication.Requests;
using StudyFlow.Communication.Response;
using StudyFlow.Domain.Enums;

namespace StudyFlow.API.Controllers
{
    [AuthenticatedUser]
    public class ReviewController : StudyFlowBaseController
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(
            [FromServices] ICreateReviewUseCase useCase,
            [FromBody] RequestCreateReviewJson request)
        {
            var response = await useCase.Execute(request);

            return Created(string.Empty, response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ResponseTopicsJson), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAll(
            [FromServices] IGetAllReviewsUseCase useCase,
            [FromQuery] bool? active, 
            [FromQuery] IList<string>? status,
            [FromQuery] IList<string>? difficulty)
        {
            var response = await useCase.Execute(active, status, difficulty);

            if (response.Reviews.Any())
            {
                return Ok(response);
            }

            return NoContent();
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(
            [FromServices] IUpdateReviewUseCase useCase,
            [FromRoute][ModelBinder(typeof(StudyFlowBinder))] long id,
            [FromBody] RequestUpdateReviewJson request)
        {
            await useCase.Execute(request, id);

            return NoContent();
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Deactivate(
            [FromServices] IDeactivateReviewUseCase useCase,
            [FromRoute][ModelBinder(typeof(StudyFlowBinder))] long id)
        {
            await useCase.Execute(id);

            return NoContent();
        }



    }
}

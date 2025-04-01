using Microsoft.AspNetCore.Mvc;
using StudyFlow.Application.UseCases.Token.RefreshToken;
using StudyFlow.Communication.Requests;
using StudyFlow.Communication.Response;

namespace StudyFlow.API.Controllers
{
    public class TokenController : StudyFlowBaseController
    {
        [HttpPost("refresh-token")]
        [ProducesResponseType(typeof(ResponseTokenJson), StatusCodes.Status200OK)]
        public async Task<IActionResult> RefreshToken(
            [FromServices] IUseRefreshTokenUseCase useCase,
            [FromBody] RequestNewTokenJson request)
        {
            var response = await useCase.Execute(request);

            return Ok(response);
        }
    }
}

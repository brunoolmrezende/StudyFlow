using Microsoft.AspNetCore.Mvc;
using StudyFlow.Application.UseCases.User.Login.DoLogin;
using StudyFlow.Communication.Requests;
using StudyFlow.Communication.Response;

namespace StudyFlow.API.Controllers
{
    public class LoginController : StudyFlowBaseController
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login(
            [FromServices] IDoLoginUseCase useCase,
            [FromBody] RequestDoLoginJson request)
        {
            var response = await useCase.Execute(request);

            return Ok(response);
        }
    }
}

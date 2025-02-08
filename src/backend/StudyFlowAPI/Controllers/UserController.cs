using Microsoft.AspNetCore.Mvc;
using StudyFlow.Communication.Requests;
using StudyFlow.Communication.Response;

namespace StudyFlow.API.Controllers
{
    public class UserController : StudyFlowBaseController
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status201Created)]
        public IActionResult Register([FromBody] RequestRegisterUserJson request)
        {
            return Created();
        }

    }
}

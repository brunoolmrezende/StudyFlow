using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace StudyFlow.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [EnableRateLimiting("RateLimiterPolicy")]
    public class StudyFlowBaseController : ControllerBase
    {
    }
}

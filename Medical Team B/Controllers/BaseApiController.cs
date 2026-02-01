using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Medical_Team_B.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        protected string UserId => User.FindFirstValue("uid")!;
    }
}

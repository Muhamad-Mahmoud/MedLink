using Medical_Team_B.Errors;
using Microsoft.AspNetCore.Mvc;

namespace Medical_Team_B.Controllers
{
    [Route("errors/{code}")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorsController : ControllerBase
    {
        public IActionResult Error(int code)
        {
            return NotFound(new ApiResponse(code));
        }
    }
}

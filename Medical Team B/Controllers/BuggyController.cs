using Medical_Team_B.Errors;
using MedLink.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Mvc;

namespace Medical_Team_B.Controllers
{
    public class BuggyController : BaseApiController
    {
        private readonly ApplicationDbContext _context;

        public BuggyController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet("notfound")]
        public ActionResult GetNotFound()
        {
            return NotFound(new ApiResponse(404));
        }

        //[HttpGet("servererror")]
        //public ActionResult GetServerError()
        //{
        //    var users = _context.Users.Find(100);
        //    var usersToReturn = users.ToString();
        //    return Ok(usersToReturn);
        //}

        [HttpGet("badrequest")]
        public ActionResult GetBadRequest()
        {
            return BadRequest(new ApiResponse(400));
        }

        [HttpGet("badrequest/{id}")]
        public ActionResult GetBadRequest(int id)
        {
            return Ok();
        }



    }
}

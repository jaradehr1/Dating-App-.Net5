using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BuggyController : BaseAPIController
    {
        private readonly DataContext context;
        public BuggyController(DataContext context)
        {
            this.context = context;
        }

        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetSecret() 
        {
            return "secret text";    
        }

        [HttpGet("not-found")]
        public ActionResult<AppUser> GetNotFound() 
        {
            var x = this.context.Users.Find(-1);
            if (x == null)
            {
                return NotFound();
            }
            return Ok(x);
        }

        [HttpGet("server-error")]
        public ActionResult<string> GetServerError() 
        {
            var x = this.context.Users.Find(-1);
            var y = x.ToString();
            return y;
        }

        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest() 
        {
            return BadRequest("This was not a good request"); 
        }
    }
}
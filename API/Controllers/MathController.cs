using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MathController : ControllerBase
    {
        [HttpGet]
        public ActionResult GetSolution()
        {
            return Ok();
        }
    }
}

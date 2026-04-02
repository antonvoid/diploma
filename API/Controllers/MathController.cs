using Math.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MathController : ControllerBase
    {
        private IMathService _service;
        public MathController(IMathService service)
        {
            _service = service;
        }
        [HttpGet]
        public ActionResult GetSolution()
        {
            var x = _service.Compute(10);
            return Ok(x);
        }
    }
}

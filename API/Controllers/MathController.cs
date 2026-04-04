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
        [HttpGet("tets")]
        public ActionResult GetSolution()
        {
            var x = _service.Compute(10);
            return Ok(x);
        }
        [HttpPost("exponential")]
        public ActionResult<ExpOdeResponse> SolveExponential([FromBody] ExpOdeRequest req)
        {
            var (t, y) = _service.SolveExponentialGrowthDecay_RK4(req.a, req.y0, req.t0, req.t1, req.n);
            return Ok(new ExpOdeResponse(t, y));
        }

        [HttpPost("sho")]
        public ActionResult<ShoResponse> SolveSho([FromBody] ShoRequest req)
        {
            var (t, x, v) = _service.SolveSimpleHarmonicOscillator_RK4(req.omega, req.x0, req.v0, req.t0, req.t1, req.n);
            return Ok(new ShoResponse(t, x, v));
        }

        [HttpPost("pendulum")]
        public ActionResult<PendulumResponse> SolvePendulum([FromBody] PendulumRequest req)
        {
            var (t, theta, omega) = _service.SolveNonlinearPendulum_RK4(req.g, req.length, req.theta0, req.omega0, req.t0, req.t1, req.n);
            return Ok(new PendulumResponse(t, theta, omega));
        }
    }
    public sealed record ExpOdeRequest(double a, double y0, double t0, double t1, int n);
    public sealed record ExpOdeResponse(double[] t, double[] y);

    public sealed record ShoRequest(double omega, double x0, double v0, double t0, double t1, int n);
    public sealed record ShoResponse(double[] t, double[] x, double[] v);

    public sealed record PendulumRequest(double g, double length, double theta0, double omega0, double t0, double t1, int n);
    public sealed record PendulumResponse(double[] t, double[] theta, double[] omega);

}

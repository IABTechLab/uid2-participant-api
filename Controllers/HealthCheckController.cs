using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace uid2_participant_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        [HttpGet(Name = "HealthCheck")]
        public string Get()
        {
            return "Healthy";
        }
    }
}

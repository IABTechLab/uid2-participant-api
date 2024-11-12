using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UID.Participant.Api.Controllers
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

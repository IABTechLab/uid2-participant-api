using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace uid2_participant_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SitesController : ControllerBase
    {
        private readonly ParticipantApiContext participantApiContext;

        public SitesController(ParticipantApiContext participantApiContext)
        {
            this.participantApiContext = participantApiContext;
        }

        // GET: api/<SitesController>
        [HttpGet]
        public IEnumerable<Site> Get()
        {
            return new Site[] { new Site(), new Site() };
        }

        // GET api/<SitesController>/5
        [HttpGet("{id}")]
        [ProducesResponseType<Site>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async ValueTask<IActionResult> Get(int id)
        {
            var site = await this.participantApiContext.Sites.FirstOrDefaultAsync(s => s.Id == id);
            return site == null ? NotFound() : Ok(site);
        }

        // POST api/<SitesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<SitesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<SitesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

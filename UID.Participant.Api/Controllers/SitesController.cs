using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace uid2_participant_api.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion(1.0)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class SitesController : ControllerBase
    {
        private readonly ILogger<SitesController> logger;
        private readonly ParticipantApiContext participantApiContext;

        public SitesController(ILogger<SitesController> logger, ParticipantApiContext participantApiContext)
        {
            this.logger = logger;
            this.participantApiContext = participantApiContext;
        }

        // GET: api/Sites
        [HttpGet]
        [ProducesResponseType<IEnumerable<Site>>(StatusCodes.Status200OK)]
        public async ValueTask<IActionResult> Get()
        {
            var sites = await this.participantApiContext.Sites
                .AsNoTracking()
                .ToListAsync();
            return Ok(sites);
        }

        // GET api/Sites/5
        [HttpGet("{id}")]
        [ProducesResponseType<Site>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async ValueTask<IActionResult> Get(int id)
        {
            var site = await this.participantApiContext.Sites
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);
            return site == null ? NotFound() : Ok(site);
        }

        // POST api/Sites
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async ValueTask<IActionResult> Post([FromBody] Site value)
        {
            try
            {
                await this.participantApiContext.AddAsync(value);
                return Ok();
            }
            catch (Exception ex)
            {
                this.logger.LogDebug(ex, "Error adding Site.");
                return BadRequest();
            }
        }

        // PUT api/Sites/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async ValueTask<IActionResult> Put(int id, [FromBody] Site value)
        {
            var site = await this.participantApiContext.Sites.FirstOrDefaultAsync(s => s.Id == id);
            if (site != null)
            {
                site.Name = value.Name;
                site.Enabled = value.Enabled;
                site.Visible = value.Visible;
                await this.participantApiContext.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        // DELETE api/Sites/5
        /*[HttpDelete("{id}")]
        public void Delete(int id)
        {
        }*/
    }
}

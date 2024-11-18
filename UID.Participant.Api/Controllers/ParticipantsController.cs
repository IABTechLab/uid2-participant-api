﻿using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using UID.Participant.Api.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UID.Participant.Api.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion(1.0)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class ParticipantsController(ILogger<ParticipantsController> logger, ParticipantApiContext participantApiContext) : ControllerBase
    {
        private readonly ILogger<ParticipantsController> logger = logger;
        private readonly ParticipantApiContext participantApiContext = participantApiContext;

        // GET: api/Participants
        [HttpGet]
        [ProducesResponseType<IEnumerable<Models.Participant>>(StatusCodes.Status200OK)]
        public async ValueTask<IActionResult> Get()
        {
            var participants = await this.participantApiContext.Participants
                .AsNoTracking()
                .ToListAsync();
            return this.Ok(participants);
        }

        // GET api/Participants/5
        [HttpGet("{id}")]
        [ProducesResponseType<Models.Participant>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async ValueTask<IActionResult> Get(int id)
        {
            var participant = await this.participantApiContext.Participants
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);
            return participant == null ? this.NotFound() : this.Ok(participant);
        }

        // POST api/Participants
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async ValueTask<IActionResult> Post([FromBody] Models.Participant value)
        {
            try
            {
                var validClientTypes = await this.ValidateClientTypes(value.ClientTypes);
                if (!validClientTypes.valid)
                {
                    return this.BadRequest(this.ModelState);
                }

                value.ClientTypes = validClientTypes.clientTypes;
                await this.participantApiContext.AddAsync(value);
                await this.participantApiContext.SaveChangesAsync();
                return this.Ok(value);
            }
            catch (Exception ex)
            {
                this.logger.LogDebug(ex, "Error adding Participant.");
                return this.BadRequest();
            }
        }

        // PUT api/Participants/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async ValueTask<IActionResult> Put(int id, [FromBody] Models.Participant value)
        {
            var participant = await this.participantApiContext.Participants.FirstOrDefaultAsync(s => s.Id == id);
            var validClientTypes = await this.ValidateClientTypes(value.ClientTypes);

            if (participant is null)
            {
                return this.NotFound();
            }

            if (!validClientTypes.valid)
            {
                return this.BadRequest(this.ModelState);
            }

            if (participant != null)
            {
                participant.Name = value.Name;
                participant.Enabled = value.Enabled;
                participant.Visible = value.Visible;
                participant.ClientTypes = validClientTypes.clientTypes;
                await this.participantApiContext.SaveChangesAsync();
                return this.Ok(participant);
            }
            else
            {
                return this.NotFound();
            }
        }

        // DELETE api/Participants/5
        /*[HttpDelete("{id}")]
        public void Delete(int id)
        {
        }*/

        private async ValueTask<(bool valid, ICollection<ClientType> clientTypes)> ValidateClientTypes(ICollection<ClientType> clientTypes)
        {
            // get all the client types in the database that match the given ones
            var dbClientTypes = await this.participantApiContext.ClientTypes.Where(ct => clientTypes.Contains(ct)).ToListAsync();

            // remove all the valid ones
            var invalidClientTypes = clientTypes.Except(dbClientTypes);
            if (invalidClientTypes.Any())
            {
                this.ModelState.AddModelError("Invalid ClientTypes", string.Join(",", invalidClientTypes.Select(ct => ct.ToString())));
                return (valid: false, clientTypes: new List<ClientType>(invalidClientTypes));
            }

            return (valid: true, clientTypes: dbClientTypes);
        }
    }
}

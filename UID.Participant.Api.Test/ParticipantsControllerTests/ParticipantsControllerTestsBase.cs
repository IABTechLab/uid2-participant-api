using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UID.Participant.Api.Controllers;

namespace UID.Participant.Api.Test.ParticipantsControllerTests
{
    public abstract class ParticipantsControllerTestsBase : ControllerTestsBase
    {
        protected readonly ILogger<ParticipantsController> loggerMock;
        protected readonly ParticipantsController sut;

        protected ParticipantsControllerTestsBase()
        {
            this.loggerMock = Substitute.For<ILogger<ParticipantsController>>();
            this.sut = new ParticipantsController(this.loggerMock, this.ReadParticipantContext);
        }
    }
}

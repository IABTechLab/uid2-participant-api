using Microsoft.Extensions.Logging;
using NSubstitute;
using UID.Participant.Api.Controllers;

namespace UID.Participant.Api.Test.ControllerTests.Participant
{
    public abstract class TestsBase : ControllerTestsBase
    {
        protected readonly ILogger<ParticipantsController> loggerMock;
        protected readonly ParticipantsController sut;

        protected TestsBase()
        {
            this.loggerMock = Substitute.For<ILogger<ParticipantsController>>();
            this.sut = new ParticipantsController(this.loggerMock, this.ReadParticipantContext);
        }
    }
}

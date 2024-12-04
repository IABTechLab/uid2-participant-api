using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using System.ComponentModel.DataAnnotations;
using UID.Participant.Api.Controllers;
using UID.Participant.Api.Models;
using UID.Participant.Api.Validation;

namespace UID.Participant.Api.Test.ControllerTests.Participant
{
    public class PostTests : TestsBase
    {
        [Fact]
        public async Task PostSavesParticipant()
        {
            var participant = this.Fixture
                .Build<Models.Participant>()
                .With(p => p.ClientTypes, [])
                .Create();

            var serviceProviderMock = Substitute.For<IServiceProvider>();
            serviceProviderMock.GetService(typeof(ParticipantApiContext)).Returns(this.ReadParticipantContext);

            var context = new ValidationContext(participant.ClientTypes, serviceProviderMock, null);
            var validator = new ValidateClientTypesAttribute();

            validator.IsValidInternal(participant.ClientTypes, context);

            var result = await this.sut.Post(participant);
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>().Subject.Value.Should().BeOfType<Models.Participant>().And.BeEquivalentTo(participant);
        }

        [Fact]
        public async Task PostReturnsBadRequestParticipantWithNewClientTypes()
        {
            var dummyClientType = new ClientType { Id = 5, Name = "Does not exist" };
            var participant = this.Fixture
                .Build<Models.Participant>()
                .With(p => p.ClientTypes, new List<ClientType>([dummyClientType]))
                .Create();

            this.sut.ModelState.AddModelError("Invalid ClientTypes", "error");
            var result = await this.sut.Post(participant);
            result.Should().NotBeNull();
            var errorResult = result.Should().BeOfType<BadRequestObjectResult>().Subject.Value.Should().BeOfType<SerializableError>().Subject;
            var keyPair = errorResult.First().Should().BeOfType<KeyValuePair<string, object>>().Subject;
            keyPair.Key.Should().Be("Invalid ClientTypes");
            keyPair.Value.Should().BeOfType<string[]>().Subject[0].Should().Be(dummyClientType.ToString());
        }
    }
}

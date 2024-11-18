using AutoFixture;
using AutoFixture.Kernel;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using UID.Participant.Api.Models;

namespace UID.Participant.Api.Test.ParticipantsControllerTests
{
    public class ParticipantsControllerPostTests : ParticipantsControllerTestsBase
    {
        [Theory]
        [InlineData(new int[0] )]
        [InlineData(new[] { 1 })]
        [InlineData(new[] { 1, 2 })]
        [InlineData(new[] { 1, 2, 3 })]
        [InlineData(new[] { 1, 2, 3, 4 })]
        public async Task PostSavesParticipantWithValidClientType(int[] clientTypeIds)
        {
            var participant = this.Fixture
                .Build<Models.Participant>()
                .With(p => p.ClientTypes, new List<ClientType>(this.KnownClientTypes.Where(ct => clientTypeIds.Contains(ct.Id))))
                .Create();

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

            var result = await this.sut.Post(participant);
            result.Should().NotBeNull();
            var errorResult = result.Should().BeOfType<BadRequestObjectResult>().Subject.Value.Should().BeOfType<SerializableError>().Subject;
            var keyPair = errorResult.First().Should().BeOfType<KeyValuePair<string, object>>().Subject;
            keyPair.Key.Should().Be("Invalid ClientTypes");
            keyPair.Value.Should().BeOfType<string[]>().Subject[0].Should().Be(dummyClientType.ToString());
        }
    }
}

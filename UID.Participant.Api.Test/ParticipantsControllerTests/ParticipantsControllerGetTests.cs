using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using UID.Participant.Api.Models;

namespace UID.Participant.Api.Test.ParticipantsControllerTests
{
    public class ParticipantControllerGetTests : ParticipantsControllerTestsBase
    {
        // xUnit create a new instance of the class for each test

        [Fact]
        public async Task ReturnsAllParticipants()
        {
            // arrange
            var participants = this.Fixture
                .Build<Models.Participant>()
                .With(p => p.ClientTypes, [this.KnownClientTypes[2]])
                .CreateMany();
            this.WriteParticipantContext.Participants.AddRange(participants);
            this.WriteParticipantContext.SaveChanges();

            // Act
            var result = await this.sut.Get();

            // Assert
            result.Should().NotBeNull();
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.StatusCode.Should().Be(200);
            var actualParticipants = okResult.Value.Should().BeOfType<List<Models.Participant>>().Subject;
            actualParticipants.Should().NotBeEmpty()
                .And.HaveCount(participants.Count());
            actualParticipants.Should().BeEquivalentTo(participants);
        }

        [Fact]
        public async Task ReturnsEmptyList()
        {
            // arrange

            // act
            var result = await this.sut.Get();

            // assert
            result.Should().NotBeNull();
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeOfType<List<Models.Participant>>().Subject.Should().BeEmpty();
        }

        [Fact]
        public async Task ByIdReturnsParticipant()
        {
            // arrange
            var participants = this.Fixture
                .Build<Models.Participant>()
                .With(p => p.ClientTypes, [this.KnownClientTypes[3]])
                .CreateMany();
            this.WriteParticipantContext.Participants.AddRange(participants);
            this.WriteParticipantContext.SaveChanges();

            // act
            foreach (var participant in participants)
            {
                var result = await this.sut.Get(participant.Id);

                // assert
                result.Should().NotBeNull();
                var okResult = result.Should()
                    .BeOfType<OkObjectResult>().Subject;
                okResult.StatusCode.Should().Be(200);
                var actualParticipant = okResult.Value
                    .Should().BeOfType<Models.Participant>().Subject;
                actualParticipant.Should().BeEquivalentTo(participant);
                actualParticipant.ClientTypes.Should().BeEquivalentTo(participant.ClientTypes);
            }
        }

        [Fact]
        public async Task ByIdReturnsNotFound()
        {
            // arrange
            var participants = this.Fixture
                .Build<Models.Participant>()
                .With(p => p.ClientTypes, [this.KnownClientTypes[2]])
                .CreateMany();
            this.WriteParticipantContext.Participants.AddRange(participants);
            this.WriteParticipantContext.SaveChanges();

            // act
            var result = await this.sut.Get(int.MaxValue);
            result.Should().NotBeNull();
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task ByIdReturnsExpectedClientTypes()
        {
            var participant = this.Fixture
                .Build<Models.Participant>()
                .With(p => p.ClientTypes, [this.KnownClientTypes.First()])
                .Create();
            this.WriteParticipantContext.Participants.Add(participant);
            this.WriteParticipantContext.SaveChanges();

            // add a client type that should not exist on the returned one
            var newClientType = new ClientType { Id = 500, Name = "New Client Type" };
            participant.ClientTypes.Add(newClientType);

            var result = await this.sut.Get(participant.Id);
            var actualParticipant = result.Should().BeOfType<OkObjectResult>().Subject.Value.Should().BeOfType<Models.Participant>().Subject;

            actualParticipant.ClientTypes.Should().NotContain(newClientType);
        }
    }
}
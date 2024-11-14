using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using UID.Participant.Api.Controllers;
using UID.Participant.Api.Models;

namespace UID.Participant.Api.Test
{
    public class ParticipantControllerGetTests : IDisposable
    {
        private readonly Fixture fixture;
        private readonly SqliteConnection sqLiteConnection;
        private readonly ParticipantApiContext participantContext;
        private readonly ILogger<ParticipantsController> loggerMock;
        private readonly ParticipantsController sut;

        // xUnit create a new instance of the class for each test
        public ParticipantControllerGetTests()
        {
            this.fixture = new Fixture();
            this.sqLiteConnection = new SqliteConnection("Filename=:memory:"); 
            this.sqLiteConnection.Open();
            this.participantContext = new ParticipantApiContext(new DbContextOptionsBuilder<ParticipantApiContext>().UseSqlite(this.sqLiteConnection).Options);
            this.participantContext.Database.EnsureCreated();

            this.loggerMock = Substitute.For<ILogger<ParticipantsController>>();

            // separate the read and write contexts, so only changes that happen through .SaveChanges are returned in the queiries
            var readContext = new ParticipantApiContext(new DbContextOptionsBuilder<ParticipantApiContext>().UseSqlite(this.sqLiteConnection).Options);
            this.sut = new ParticipantsController(this.loggerMock, readContext);
        }

        [Fact]
        public async Task ReturnsAllParticipants()
        {
            // arrange
            var participants = this.fixture.CreateMany<Models.Participant>();
            this.participantContext.Participants.AddRange(participants);
            this.participantContext.SaveChanges();

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
            var participants = this.fixture.CreateMany<Models.Participant>();
            this.participantContext.Participants.AddRange(participants);
            this.participantContext.SaveChanges();

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
            var participants = this.fixture.CreateMany<Models.Participant>();
            this.participantContext.Participants.AddRange(participants);
            this.participantContext.SaveChanges();

            // act
            var result = await this.sut.Get(int.MaxValue);
            result.Should().NotBeNull();
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task ByIdReturnsExpectedClientTypes()
        {
            var participant = this.fixture.Create<Models.Participant>();
            this.participantContext.Participants.Add(participant);
            this.participantContext.SaveChanges();

            // add a client type that should not exist on the returned one
            var newClientType = new ClientType { Id = 500, Name = "New Client Type" };
            participant.ClientTypes.Add(newClientType);

            var result = await this.sut.Get(participant.Id);
            var actualParticipant = result.Should().BeOfType<OkObjectResult>().Subject.Value.Should().BeOfType<Models.Participant>().Subject;

            actualParticipant.ClientTypes.Should().NotContain(newClientType);
        }

        public void Dispose()
        {
            if (this.sqLiteConnection != null)
            {
                this.sqLiteConnection.Close();
                this.sqLiteConnection.Dispose();
            }

            GC.SuppressFinalize(this);
        }
    }
}
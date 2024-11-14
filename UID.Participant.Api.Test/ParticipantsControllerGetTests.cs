using AutoFixture;
using EntityFrameworkCore.Testing.NSubstitute;
using EntityFrameworkCore.Testing.NSubstitute.Helpers;
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
        private Fixture fixture;
        private SqliteConnection sqLiteConnection;
        private ParticipantApiContext mockedContext;
        private ILogger<ParticipantsController> loggerMock;
        private ParticipantsController sut;

        // xUnit create a new instance of the class for each test
        public ParticipantControllerGetTests()
        {
            this.fixture = new Fixture();
            this.sqLiteConnection = new SqliteConnection("Filename=:memory:");
            sqLiteConnection.Open();
            var dbContextToMock = new ParticipantApiContext(new DbContextOptionsBuilder<ParticipantApiContext>().UseSqlite(this.sqLiteConnection).Options);
            dbContextToMock.Database.EnsureCreated();
            this.mockedContext = new MockedDbContextBuilder<ParticipantApiContext>().UseDbContext(dbContextToMock).MockedDbContext;

            //this.mockedContext = Create.MockedDbContextFor<ParticipantApiContext>();
            this.loggerMock = Substitute.For<ILogger<ParticipantsController>>();
            this.sut = new ParticipantsController(loggerMock, mockedContext);
        }

        

        [Fact]
        public async Task ReturnsAllParticipants()
        {
            // arrange
            var participants = fixture.CreateMany<Models.Participant>();
            mockedContext.Participants.AddRange(participants);
            mockedContext.SaveChanges();

            // Act
            var result = await sut.Get();

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
            var result = await sut.Get();

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
            this.mockedContext.Participants.AddRange(participants);
            this.mockedContext.SaveChanges();

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
                // actualParticipant
            }
        }

        [Fact]
        public async Task ByIdReturnsNotFound()
        {
            // arrange
            var participants = this.fixture.CreateMany<Models.Participant>();
            this.mockedContext.Participants.AddRange(participants);
            this.mockedContext.SaveChanges();

            // act
            var result = await this.sut.Get(int.MaxValue);
            result.Should().NotBeNull();
            var notFound = result.Should().BeOfType<NotFoundResult>();
        }

        public void Dispose()
        {
            if (this.sqLiteConnection != null)
            {
                this.sqLiteConnection.Close();
            }
        }
    }
}
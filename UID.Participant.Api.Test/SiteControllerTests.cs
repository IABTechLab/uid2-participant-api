using AutoFixture;
using EntityFrameworkCore.Testing.NSubstitute;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using uid2_participant_api;
using uid2_participant_api.Controllers;

namespace UID2.Participant.Api.Test
{
    public class SiteControllerTests
    {
        [Fact]
        public async Task GetSitesReturnsSites()
        {
            Fixture fixture = new Fixture();

            //var sites = fixture.CreateMany<Site>();
            var mockedContext = Create.MockedDbContextFor<ParticipantApiContext>();

            mockedContext.Set<Site>().Add(new Site());

            var loggerMock = Substitute.For<ILogger<SitesController>>();

            // Act
            var sut = new SitesController(loggerMock, mockedContext);
            var actualSites = await sut.Get();

            // Assert
            actualSites.Should().NotBeNull();
        }
    }
}
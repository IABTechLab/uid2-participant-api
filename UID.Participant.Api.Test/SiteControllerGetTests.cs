using AutoFixture;
using EntityFrameworkCore.Testing.NSubstitute;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using UID.Participant.Api.Controllers;
using UID.Participant.Api.Models;

namespace UID2.Participant.Api.Test
{
    public class SiteControllerGetTests
    {
        private Fixture fixture;
        private ParticipantApiContext mockedContext;
        private ILogger<SitesController> loggerMock;
        private SitesController sut;

        // xUnit create a new instance of the class for each test
        public SiteControllerGetTests()
        {
            this.fixture = new Fixture();
            this.mockedContext = Create.MockedDbContextFor<ParticipantApiContext>();
            this.loggerMock = Substitute.For<ILogger<SitesController>>();
            this.sut = new SitesController(loggerMock, mockedContext);
        }

        [Fact]
        public async Task GetSitesReturnsAllSites()
        {
            // arrange
            var sites = fixture.CreateMany<Site>();
            mockedContext.Sites.AddRange(sites);
            mockedContext.SaveChanges();

            // Act
            var result = await sut.Get();

            // Assert
            result.Should().NotBeNull();
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.StatusCode.Should().Be(200);
            var actualSites = okResult.Value.Should().BeOfType<List<Site>>().Subject;
            actualSites.Should().NotBeEmpty()
                .And.HaveCount(sites.Count());
            actualSites.Should().BeEquivalentTo(sites);
        }

        [Fact]
        public async Task GetSitesReturnsEmptyList()
        {
            // arrange

            // act
            var result = await sut.Get();

            // assert
            result.Should().NotBeNull();
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeOfType<List<Site>>().Subject.Should().BeEmpty();
        }

        [Fact]
        public async Task GetSiteByIdReturnsSite()
        {
            // arrange
            var sites = this.fixture.CreateMany<Site>();
            this.mockedContext.Sites.AddRange(sites);
            this.mockedContext.SaveChanges();

            // act
            foreach (var site in sites)
            {
                var result = await this.sut.Get(site.Id);

                // assert
                result.Should().NotBeNull();
                var okResult = result.Should()
                    .BeOfType<OkObjectResult>().Subject;
                okResult.StatusCode.Should().Be(200);
                okResult.Value
                    .Should().BeOfType<Site>().Subject
                    .Should().BeEquivalentTo(site);
            }
        }
    }
}
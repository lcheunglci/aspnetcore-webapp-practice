using FluentAssertions;
using GigHub.Core.Models;
using GigHub.Persistence;
using GigHub.Repositories;
using GigHub.Tests.Extensions;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace GigHub.Tests.Persistence.Repositories
{

    public class GigRepositoryTests
    {
        private GigRepository _repository;
        private Mock<DbSet<Gig>> _mockGigs;

        public GigRepositoryTests()
        {
            _mockGigs = new Mock<DbSet<Gig>>();


            var mockContext = new Mock<IApplicationDbContext>();
            mockContext.SetupGet(c => c.Gigs).Returns(_mockGigs.Object);


            _repository = new GigRepository(mockContext.Object);
        }


        [Fact]
        public void GetUpcomingGigsByArtist_GigIsInThePast_ShouldNotBeReturned()
        {
            var gig = new Gig()
            {
                DateTime = DateTime.Now.AddDays(-1),
                ArtistId = "1"
            };

            _mockGigs.SetSource(new[] { gig });

            var gigs = _repository.GetUpcomingGigsByArtist("1");

            gigs.Should().BeEmpty();
        }

        [Fact]
        public void GetUpcomingGigsByArtist_GigIsCanceled_ShouldNotBeReturned()
        {
            var gig = new Gig()
            {
                DateTime = DateTime.Now.AddDays(1),
                ArtistId = "1"
            };
            gig.Cancel();

            _mockGigs.SetSource(new[] { gig });

            var gigs = _repository.GetUpcomingGigsByArtist("1");

            gigs.Should().BeEmpty();
        }

    }
}

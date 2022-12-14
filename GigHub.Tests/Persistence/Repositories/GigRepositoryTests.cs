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
        private Mock<DbSet<Attendance>> _mockAttendances;

        public GigRepositoryTests()
        {
            _mockGigs = new Mock<DbSet<Gig>>();
            _mockAttendances = new Mock<DbSet<Attendance>>();


            var mockContext = new Mock<IApplicationDbContext>();
            mockContext.SetupGet(c => c.Gigs).Returns(_mockGigs.Object);
            mockContext.SetupGet(c => c.Attendances).Returns(_mockAttendances.Object);



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


        [Fact]
        public void GetUpcomingGigsByArtist_GigIsForADifferentArtist_ShouldNotBeReturned()
        {
            var gig = new Gig()
            {
                DateTime = DateTime.Now.AddDays(1),
                ArtistId = "1"
            };

            _mockGigs.SetSource(new[] { gig });

            var gigs = _repository.GetUpcomingGigsByArtist(gig.ArtistId + "-");

            gigs.Should().BeEmpty();
        }

        [Fact]
        public void GetUpcomingGigsByArtist_GigIsForTheGivenArtistAndIsTheFuture_ShouldBeReturned()
        {
            var gig = new Gig()
            {
                DateTime = DateTime.Now.AddDays(1),
                ArtistId = "1"
            };

            _mockGigs.SetSource(new[] { gig });

            var gigs = _repository.GetUpcomingGigsByArtist(gig.ArtistId);

            gigs.Should().Contain(gig);
        }

        // This test caught a bug in GetGigsUserAttending() method. 
        // It used to return gigs from the past. 
        [Fact]
        public void GetGigsUserAttending_GigIsInThePast_ShouldNotBeReturned()
        {
            var gig = new Gig() { DateTime = DateTime.Now.AddDays(-1) };
            var attendance = new Attendance { Gig = gig, AttendeeId = "1" };

            _mockAttendances.SetSource(new[] { attendance });

            var gigs = _repository.GetGigsUserAttending(attendance.AttendeeId);

            gigs.Should().BeEmpty();
        }

        [Fact]
        public void GetGigsUserAttending_AttendanceForADifferentUser_ShouldNotBeReturned()
        {
            var gig = new Gig() { DateTime = DateTime.Now.AddDays(1) };
            var attendance = new Attendance { Gig = gig, AttendeeId = "1" };

            _mockAttendances.SetSource(new[] { attendance });

            var gigs = _repository.GetGigsUserAttending(attendance.AttendeeId + "-");

            gigs.Should().BeEmpty();
        }

        [Fact]
        public void GetGigsUserAttending_UpcomingGigUserAttending_ShouldBeReturned()
        {
            var gig = new Gig() { DateTime = DateTime.Now.AddDays(1) };
            var attendance = new Attendance { Gig = gig, AttendeeId = "1" };

            _mockAttendances.SetSource(new[] { attendance });

            var gigs = _repository.GetGigsUserAttending(attendance.AttendeeId);

            gigs.Should().Contain(gig);
        }

    }
}

using FluentAssertions;
using GigHub.Controllers;
using GigHub.Core.Models;
using GigHub.IntegrationTests.Extensions;
using GigHub.Persistence;
using GigHub.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace GigHub.IntegrationTests.Controllers
{
    public class GigsControllerTests : TestBase
    {
        private GigsController _controller;
        private ApplicationDbContext _context;

        public GigsControllerTests()
        {
            _context = new ApplicationDbContext();
            IUserStore<ApplicationUser> store = new UserStore<ApplicationUser>(_context);
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(store, null, new PasswordHasher<ApplicationUser>(), null, null, null, null, null, null);

            _controller = new GigsController(
                userManager,
                new UnitOfWork(_context));

        }


        public override void Dispose()
        {
            _context.Dispose();
            base.Dispose();
        }

        [Fact]
        public void Mine_WhenCalled_ShouldReturnUpcomingGigs()
        {
            // Arrange
            var user = _context.Users.First();
            _controller.MockCurrentUser(user.Id, user.UserName);

            var genre = _context.Genres.First();
            var gig = new Gig { Artist = (ApplicationUser)user, DateTime = DateTime.Now.AddDays(1), Genre = genre, Venue = "-" };

            _context.Gigs.Add(gig);
            _context.SaveChanges();

            // Act
            var result = _controller.Mine();


            // Assert
            (result.ViewData.Model as IEnumerable<Gig>).Should().HaveCount(1);

        }

        [Fact]
        public void Update_WhenCalled_ShouldUpdateTheGivenGigs()
        {
            // Arrange
            var user = _context.Users.First();
            _controller.MockCurrentUser(user.Id, user.UserName);

            var genre = _context.Genres.Single(g => g.Id == 1);
            var gig = new Gig { Artist = (ApplicationUser)user, DateTime = DateTime.Now.AddDays(1), Genre = genre, Venue = "-" };

            _context.Gigs.Add(gig);
            _context.SaveChanges();

            // Act
            var result = _controller.Update(new GigFormViewModel
            {
                Id = gig.Id,
                Date = DateTime.Today.AddMonths(1).ToString("d MMM yyyy"),
                Time = "20:00",
                Venue = "Venue",
                Genre = 2
            });


            // Assert
            _context.Entry(gig).Reload();

            gig.DateTime.Should().Be(DateTime.Today.AddMonths(1).AddHours(20));
            gig.Venue.Should().Be("Venue");
            gig.GenreId.Should().Be(2);
        }
    }
}

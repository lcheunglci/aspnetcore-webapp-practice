using FluentAssertions;
using GigHub.Controllers;
using GigHub.Core.Models;
using GigHub.IntegrationTests.Extensions;
using GigHub.Persistence;
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

            (result.ViewData.Model as IEnumerable<Gig>).Should().HaveCount(1);

        }
    }
}

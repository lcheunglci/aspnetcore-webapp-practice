using GigHub.Data;
using GigHub.Models;
using GigHub.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GigHub.Controllers
{
    public class GigsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public GigsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        [Authorize]
        public IActionResult Mine()
        {
            var userId = _userManager.GetUserId(User);
            var gigs = _context.Gigs
                .Where(g =>
                    g.ArtistId == userId &&
                    g.DateTime > DateTime.Now &&
                    !g.IsCanceled)
                .Include(g => g.Genre)
                .ToList();

            return View(gigs);
        }


        [Authorize]
        public IActionResult Attending()
        {
            var userId = _userManager.GetUserId(User);
            var gigs = _context.Attendances
                .Include(a => a.Gig)
                .Include(g => g.Gig.Genre)
                .Include(g => g.Gig.Artist)
                .Where(a => a.AttendeeId == userId)
                .Select(a => a.Gig)
                .ToList();

            var viewModel = new GigsViewModel
            {
                UpcomingGigs = gigs,
                ShowActions = User.Identity.IsAuthenticated,
                Heading = "Gigs I'm Attending"
            };

            return View("Gigs", viewModel);
        }

        [Authorize]
        public IActionResult Create()
        {
            var viewModel = new GigFormViewModel()
            {
                Heading = "Add a Gig",
                Genres = _context.Genres.ToList(),
            };
            return View("GigForm", viewModel);
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            var userId = _userManager.GetUserId(User);
            var gig = _context.Gigs.Single(g => (g.Id == id) && (g.ArtistId == userId));
            var viewModel = new GigFormViewModel()
            {
                Heading = "Edit a Gig",
                Id = gig.Id,
                Genres = _context.Genres.ToList(),
                Date = gig.DateTime.ToString("d MMM yyyy"),
                Time = gig.DateTime.ToString("HH:mm"),
                Genre = gig.GenreId,
                Venue = gig.Venue,
            };

            return View("GigForm", viewModel);
        }

        [HttpPost]
        public IActionResult Search(GigsViewModel viewModel)
        {
            return RedirectToAction("Index", "Home", new { query = viewModel.SearchTerm });
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(GigFormViewModel viewModel)
        {
            ModelState.Remove("Genres");

            if (!ModelState.IsValid)
            {
                viewModel.Genres = _context.Genres.ToList();
                return View("GigForm", viewModel);
            }

            var genre = _context.Genres.Single(g => g.Id == viewModel.Genre);

            // TODO: add validation for datetime
            var gig = new Gig
            {
                ArtistId = _userManager.GetUserId(User),
                DateTime = viewModel.GetDateTime(),
                GenreId = viewModel.Genre,
                Venue = viewModel.Venue
            };

            _context.Gigs.Add(gig);
            _context.SaveChanges();

            return RedirectToAction("Mine", "Gigs");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(GigFormViewModel viewModel)
        {
            ModelState.Remove("Genres");

            if (!ModelState.IsValid)
            {
                viewModel.Genres = _context.Genres.ToList();
                return View("GigForm", viewModel);
            }

            var genre = _context.Genres
                .Single(g => g.Id == viewModel.Genre);

            var userId = _userManager.GetUserId(User);

            // TODO: add validation for datetime
            var gig = _context.Gigs
                .Include(g => g.Attendances.Select(a => a.Attendee))
                .Single(g => (g.Id == viewModel.Id) && (g.ArtistId == userId));

            gig.Modify(viewModel.Venue, viewModel.GetDateTime(), viewModel.Genre);

            _context.SaveChanges();

            return RedirectToAction("Mine", "Gigs");
        }

        public IActionResult Details(int id)
        {
            var gig = _context.Gigs
                .Include(g => g.Artist)
                .Include(g => g.Genre)
                .SingleOrDefault(g => g.Id == id);

            if (gig == null)
            {
                return NotFound();
            }

            var viewModel = new GigDetailsViewModel { Gig = gig };

            if (User.Identity.IsAuthenticated)
            {
                var userId = _userManager.GetUserId(User);

                viewModel.IsAttending = _context.Attendances
                    .Any(a => a.GigId == gig.Id && a.AttendeeId == userId);

                viewModel.IsFollowing = _context.Followings
                    .Any(f => f.FolloweeId == gig.ArtistId && f.FollowerId == userId);
            }

            return View("Details", viewModel);
        }

    }
}

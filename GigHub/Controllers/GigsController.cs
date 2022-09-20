using GigHub.Data;
using GigHub.Models;
using GigHub.Repositories;
using GigHub.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GigHub.Controllers
{
    public class GigsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly GigRepository _gigRepository;
        private readonly AttendanceRepositroy _attendanceRepository;
        private readonly FollowingRepository _followingRepository;
        private readonly GenreRepository _genreRepository;

        public GigsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _gigRepository = new GigRepository(_context);
            _attendanceRepository = new AttendanceRepositroy(_context);
            _followingRepository = new FollowingRepository(_context);
            _genreRepository = new GenreRepository(_context);
        }

        [Authorize]
        public IActionResult Mine()
        {
            var userId = _userManager.GetUserId(User);
            IEnumerable<Gig> gigs = _gigRepository.GetUpcomingGigsByArtist(userId);

            return View(gigs);
        }

        [Authorize]
        public IActionResult Attending()
        {
            var userId = _userManager.GetUserId(User);

            var viewModel = new GigsViewModel
            {
                UpcomingGigs = _gigRepository.GetGigsUserAttending(userId),
                ShowActions = User.Identity.IsAuthenticated,
                Heading = "Gigs I'm Attending",
                Attendances = _attendanceRepository.GetFutureAttendances(userId).ToLookup(a => a.GigId)
            };

            return View("Gigs", viewModel);
        }

        [Authorize]
        public IActionResult Create()
        {
            var viewModel = new GigFormViewModel()
            {
                Heading = "Add a Gig",
                Genres = _genreRepository.GetGenres(),
            };
            return View("GigForm", viewModel);
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            var userId = _userManager.GetUserId(User);

            Gig gig = _gigRepository.GetGig(id);

            if (gig == null)
                return NotFound();

            if (gig.ArtistId != userId)
                return new UnauthorizedResult();

            var viewModel = new GigFormViewModel()
            {
                Heading = "Edit a Gig",
                Id = gig.Id,
                Genres = _genreRepository.GetGenres(),
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
                viewModel.Genres = _genreRepository.GetGenres();
                return View("GigForm", viewModel);
            }

            var genre = _genreRepository.GetGenre(viewModel.Genre);

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
                viewModel.Genres = _genreRepository.GetGenres();
                return View("GigForm", viewModel);
            }

            var genre = _genreRepository.GetGenre(viewModel.Genre);


            // TODO: add validation for datetime
            var gig = _gigRepository.GetGigWithAttendees(viewModel.Id);

            if (gig == null)
                return NotFound();

            if (gig.ArtistId != _userManager.GetUserId(User))
                return new UnauthorizedResult();

            gig.Modify(viewModel.Venue, viewModel.GetDateTime(), viewModel.Genre);

            _context.SaveChanges();

            return RedirectToAction("Mine", "Gigs");
        }

        public IActionResult Details(int id)
        {
            var gig = _gigRepository.GetGig(id);

            if (gig == null)
            {
                return NotFound();
            }

            var viewModel = new GigDetailsViewModel { Gig = gig };

            if (User.Identity.IsAuthenticated)
            {
                var userId = _userManager.GetUserId(User);

                viewModel.IsAttending = _attendanceRepository.GetAttendance(gig.Id, userId) != null;

                viewModel.IsFollowing = _followingRepository.GetFollowing(gig.ArtistId, userId) != null;
            }

            return View("Details", viewModel);
        }
    }
}

using GigHub.Data;
using GigHub.Models;
using GigHub.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GigHub.Controllers
{
    public class GigsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public GigsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        [Authorize]
        public IActionResult Create()
        {
            var viewModel = new GigFormViewModel()
            {
                Genres = _context.Genres.ToList()
            };
            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(GigFormViewModel viewModel)
        {
            var artistId = _userManager.GetUserId(User);
            var artist = _context.Users.Single(u => u.Id == artistId);
            var genre = _context.Genres.Single(g => g.Id == viewModel.Genre);

            // TODO: add validation for datetime
            var gig = new Gig
            {
                Artist = artist,
                DateTime = DateTime.Parse($"{viewModel.Date} {viewModel.Time}"),
                Genre = genre,
                Venue = viewModel.Venue
            };

            _context.Gigs.Add(gig);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

    }
}

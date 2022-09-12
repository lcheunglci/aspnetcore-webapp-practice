using GigHub.Data;
using GigHub.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http;

namespace GigHub.API
{
    [Microsoft.AspNetCore.Mvc.Route("api/gigs")]
    [ApiController]
    [Authorize]
    public class GigsController : ControllerBase
    {
        private ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;

        public GigsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [System.Web.Http.HttpDelete]
        public IActionResult Cancel(int id)
        {
            var userId = _userManager.GetUserId(User);

            var gig = _context.Gigs.Single(g => g.Id == id && g.ArtistId == userId);

            gig.IsCanceled = true;
            _context.SaveChanges();

            return Ok();
        }
    }
}

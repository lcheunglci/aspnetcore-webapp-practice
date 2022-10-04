using GigHub.Core.Models;
using GigHub.Core.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http;

namespace GigHub.API
{
    [Microsoft.AspNetCore.Mvc.Route("api/gigs")]
    [ApiController]
    [Authorize]
    public class GigsController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private IUnitOfWork _unitOfWork;

        public GigsController(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        [System.Web.Http.HttpDelete]
        public IActionResult Cancel(int id)
        {
            var userId = _userManager.GetUserId(User);

            var gig = _unitOfWork.Gigs.GetGig(id);

            if (gig == null || gig.IsCanceled)
                return NotFound();

            gig.Cancel();

            _unitOfWork.Complete();

            return Ok();
        }
    }
}

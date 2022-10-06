using GigHub.Core.Dtos;
using GigHub.Core.Models;
using GigHub.Core.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GigHub.Controllers.API
{
    [Route("api/followings")]
    [ApiController]
    [Authorize]
    public class FollowingsController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        private UserManager<ApplicationUser> _userManager;

        public FollowingsController(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        [HttpPost]
        public IActionResult Follow(FollowingDto dto)
        {
            var userId = _userManager.GetUserId(User);

            if (_unitOfWork.Followings.GetFollowing(dto.FolloweeId, userId) != null)
            {
                return BadRequest("Following already exists.");
            }

            var following = new Following
            {
                FollowerId = userId,
                FolloweeId = dto.FolloweeId
            };

            _unitOfWork.Followings.Add(following);

            _unitOfWork.Complete();

            return Ok();
        }

        [HttpDelete]
        public IActionResult Unfollow(string artistId)
        {
            var userId = _userManager.GetUserId(User);

            var following = _unitOfWork.Followings.GetFollowing(artistId, userId);

            if (following == null)
                return NotFound();

            _unitOfWork.Followings.Remove(following);
            _unitOfWork.Complete();

            return Ok(artistId);
        }


    }
}

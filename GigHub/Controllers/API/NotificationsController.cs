using AutoMapper;
using GigHub.Core.Dtos;
using GigHub.Core.Models;
using GigHub.Core.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GigHub.Controllers.API
{
    [Route("api/notifications")]
    [ApiController]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        private UserManager<ApplicationUser> _userManager;
        private IMapper _mapper;

        public NotificationsController(UserManager<ApplicationUser> userManager, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IEnumerable<NotificationDto> GetNewNotifications()
        {
            var userId = _userManager.GetUserId(User);
            var notifications = _unitOfWork.Notifications.GetNewNotificationsFor(userId);

            // note we are not invoking Map, but passing a reference to it
            return notifications.Select(_mapper.Map<Notification, NotificationDto>);
        }

        [HttpPost("markAsRead")]
        public IActionResult MarkAsRead()
        {
            var userId = _userManager.GetUserId(User);
            var notifications = _unitOfWork.UserNotifications.GetUserNotificationsFor(userId);

            foreach (var notification in notifications)
            {
                notification.Read();
            }

            _unitOfWork.Complete();

            return Ok();
        }

    }
}

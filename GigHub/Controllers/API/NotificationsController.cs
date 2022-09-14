using AutoMapper;
using GigHub.Data;
using GigHub.Dtos;
using GigHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GigHub.Controllers.API
{
    [Route("api/notifications")]
    [ApiController]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;

        public NotificationsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public IEnumerable<NotificationDto> GetNewNotifications()
        {
            var userId = _userManager.GetUserId(User);
            var notifications = _context.UserNotifications
                .Where(un => un.UserId == userId)
                .Select(un => un.Notification)
                .Include(n => n.Gig.Artist)
                .ToList();

            var config = new MapperConfiguration(config =>
            {
                config.CreateMap<ApplicationUser, UserDto>();
                config.CreateMap<Gig, GigDto>();
                config.CreateMap<Notification, NotificationDto>();
            });

            var mapper = config.CreateMapper();

            // note we are not invoking Map, but passing a reference to it
            return notifications.Select(mapper.Map<Notification, NotificationDto>);
        }
    }
}

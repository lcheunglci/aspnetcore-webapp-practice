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

            return notifications.Select(n => new NotificationDto()
            {
                DateTime = n.DateTime,
                Gig = new GigDto
                {
                    Artist = new UserDto()
                    {
                        Id = n.Gig.Artist.Id,
                        Name = n.Gig.Artist.Name
                    },
                    DateTime = n.Gig.DateTime,
                    Id = n.Gig.Id,
                    IsCanceled = n.Gig.IsCanceled,
                    Venue = n.Gig.Venue
                },
                OriginalDateTime = n.OriginalDateTime,
                OriginalVenue = n.OriginalVenue,
                Type = n.Type
            });
        }
    }
}

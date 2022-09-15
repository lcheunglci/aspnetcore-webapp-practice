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
        private IMapper _mapper;

        public NotificationsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public IEnumerable<NotificationDto> GetNewNotifications()
        {
            var userId = _userManager.GetUserId(User);
            var notifications = _context.UserNotifications
                .Include(un => un.Notification)
                .ThenInclude(n => n.Gig)
                .ThenInclude(g => g.Artist)
                .Where(un => un.UserId == userId && !un.IsRead)
                .Select(un => un.Notification)
                .ToList();

            // note we are not invoking Map, but passing a reference to it
            return notifications.Select(_mapper.Map<Notification, NotificationDto>);
        }
    }
}

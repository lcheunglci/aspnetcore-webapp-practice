using GigHub.Core.Dtos;
using GigHub.Core.Models;
using GigHub.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GigHub.Controllers.API
{
    [Route("api/attendances")]
    [ApiController]
    [Authorize]
    public class AttendancesController : ControllerBase
    {
        private ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;


        public AttendancesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        public IActionResult Attend(AttendanceDto dto)
        {
            var userId = _userManager.GetUserId(User);


            if (_context.Attendances.Any(a => a.AttendeeId == userId && a.GigId == dto.GigId))
            {
                return BadRequest("The attendance already exists.");
            }

            var attendance = new Attendance()
            {
                GigId = dto.GigId,
                AttendeeId = userId
            };

            _context.Attendances.Add(attendance);
            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete]
        public IActionResult DeleteAttendance(int id)
        {
            var userId = _userManager.GetUserId(User);

            var attendence = _context.Attendances
                .SingleOrDefault(a => a.AttendeeId == userId && a.GigId == id);

            if (attendence == null)
            {
                return NotFound();
            }

            _context.Attendances.Remove(attendence);
            _context.SaveChanges();

            return Ok(id);
        }


    }
}

using GigHub.Core.Dtos;
using GigHub.Core.Models;
using GigHub.Core.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GigHub.Controllers.API
{
    [Route("api/attendances")]
    [ApiController]
    [Authorize]
    public class AttendancesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private IUnitOfWork _unitOfWork;

        public AttendancesController(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
        {

            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        [HttpPost]
        public IActionResult Attend(AttendanceDto dto)
        {
            var userId = _userManager.GetUserId(User);



            if (_unitOfWork.Attendances.GetAttendance(dto.GigId, userId) != null)
            {
                return BadRequest("The attendance already exists.");
            }

            var attendance = new Attendance()
            {
                GigId = dto.GigId,
                AttendeeId = userId
            };

            _unitOfWork.Attendances.Add(attendance);
            // _unitOfWork.Attendances.
            _unitOfWork.Complete();

            return Ok();
        }

        [HttpDelete]
        public IActionResult DeleteAttendance(int id)
        {
            var userId = _userManager.GetUserId(User);

            var attendence = _unitOfWork.Attendances.GetAttendance(id, userId);


            if (attendence == null)
            {
                return NotFound();
            }

            _unitOfWork.Attendances.Remove(attendence);
            _unitOfWork.Complete();

            return Ok(id);
        }


    }
}

using FluentAssertions;
using GigHub.Controllers.API;
using GigHub.Core.Dtos;
using GigHub.Core.Models;
using GigHub.Core.Repositories;
using GigHub.Tests.Extensions;
using Moq;
using System.Web.Http.Results;

namespace GigHub.Tests.Controllers.Api
{
    public class AttendancesControllerTests
    {
        private AttendancesController _controller;
        private Mock<IAttendanceRepository> _mockRepository;
        private string _userId;

        public AttendancesControllerTests()
        {
            _mockRepository = new Mock<IAttendanceRepository>();

            var mockUoW = new Mock<IUnitOfWork>();
            mockUoW.SetupGet(u => u.Attendances).Returns(_mockRepository.Object);

            var userManagerMock = ApiControllerExtensions.MockUserManager<ApplicationUser>(ApiControllerExtensions.s_users);


            _controller = new AttendancesController(userManagerMock.Object, mockUoW.Object);
            _userId = "1";
            _controller.MockCurrentUser(_userId, "user1@domain.com");
        }

        [Fact]
        public void Attend_UserAttendingAGigForWhichHeHasAnAttendance_ShouldReturnBadRequest()
        {
            var attendance = new Attendance();
            _mockRepository.Setup(r => r.GetAttendance(1, _userId)).Returns(attendance);

            var result = _controller.Attend(new AttendanceDto { GigId = 1 });

            result.Should().BeOfType<BadRequestErrorMessageResult>();
        }

        [Fact]
        public void Attend_ValidRequest_ShouldReturnOk()
        {
            var result = _controller.Attend(new AttendanceDto { GigId = 1 });

            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public void DeleteAttendance_NoAttendanceWithGivenIdExists_ShouldReturnNotFound()
        {
            var result = _controller.DeleteAttendance(1);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void DeleteAttendance_ValidRequest_ShouldReturnOk()
        {
            var attendance = new Attendance();
            _mockRepository.Setup(r => r.GetAttendance(1, _userId)).Returns(attendance);

            var result = _controller.DeleteAttendance(1);

            result.Should().BeOfType<OkNegotiatedContentResult<int>>();
        }

        [Fact]
        public void DeleteAttendance_ValidRequest_ShouldReturnTheIdOfDeletedAttendance()
        {
            var attendance = new Attendance();
            _mockRepository.Setup(r => r.GetAttendance(1, _userId)).Returns(attendance);

            var result = (OkNegotiatedContentResult<int>)_controller.DeleteAttendance(1);

            result.Content.Should().Be(1);
        }
    }
}

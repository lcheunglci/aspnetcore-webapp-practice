using FluentAssertions;
using GigHub.API;
using GigHub.Core.Models;
using GigHub.Core.Repositories;
using GigHub.Tests.Extensions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GigHub.Tests.Controllers.API
{
    public class GigsControllerTests
    {
        private const string _userId = "1";
        private GigsController _controller;
        private Mock<IGigRepository> _mockRepository;

        public GigsControllerTests()
        {
            _mockRepository = new Mock<IGigRepository>();


            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.SetupGet(u => u.Gigs).Returns(_mockRepository.Object);

            var userManagerMock = ApiControllerExtensions.MockUserManager<ApplicationUser>(ApiControllerExtensions.s_users);

            _controller = new GigsController(userManagerMock.Object, unitOfWorkMock.Object);

            _controller.MockCurrentUser(_userId, "user1@domain.com");

        }

        [Fact]
        public void Cancel_NoGigWithGivenIdExists_ShouldReturnNotFound()
        {
            var result = _controller.Cancel(1);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void Cancel_GigIsCanceled_ShouldReturnNotFound()
        {
            var gig = new Gig();
            gig.Cancel();

            _mockRepository.Setup(r => r.GetGigWithAttendees(1)).Returns(gig);

            var result = _controller.Cancel(1);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void Cancel_UserCancelingAnotherUsersGig_ShouldReturnUnauthorized()
        {
            var gig = new Gig { ArtistId = _userId + "-" };
            gig.Cancel();

            _mockRepository.Setup(r => r.GetGigWithAttendees(1)).Returns(gig);

            var result = _controller.Cancel(1);

            result.Should().BeOfType<UnauthorizedResult>();
        }

        [Fact]
        public void Cancel_ValidRequest_ShouldReturnOk()
        {
            var gig = new Gig { ArtistId = _userId };
            gig.Cancel();

            _mockRepository.Setup(r => r.GetGigWithAttendees(1)).Returns(gig);

            var result = _controller.Cancel(1);

            result.Should().BeOfType<OkResult>();
        }
    }
}

using FluentAssertions;
using GigHub.API;
using GigHub.Core.Repositories;
using GigHub.Models;
using GigHub.Tests.Extensions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GigHub.Tests.Controllers.API
{
    public class GigsControllerTests
    {


        private GigsController _controller;

        public GigsControllerTests()
        {
            var mockRepository = new Mock<IGigRepository>();


            var unitOfWOrkMock = new Mock<IUnitOfWork>();
            unitOfWOrkMock.SetupGet(u => u.Gigs).Returns(mockRepository.Object);

            var userManagerMock = ApiControllerExtensions.MockUserManager<ApplicationUser>(ApiControllerExtensions.s_users);

            _controller = new GigsController(userManagerMock.Object, unitOfWOrkMock.Object);

            _controller.MockCurrentUser("1", "user1@domain.com");

        }

        [Fact]
        public void Cancel_NoGigWithGivenIdExists_ShouldReturnNotFound()
        {
            var result = _controller.Cancel(1);

            result.Should().BeOfType<NotFoundResult>();
        }
    }
}

using GigHub.Controllers;
using GigHub.Core.Repositories;
using GigHub.Models;
using GigHub.Tests.Extensions;
using Moq;

namespace GigHub.Tests.Controllers.API
{
    public class GigsControllerTests
    {


        private GigsController _controller;

        public GigsControllerTests()
        {
            var unitOfWOrkMock = new Mock<IUnitOfWork>();

            var userManagerMock = ApiControllerExtensions.MockUserManager<ApplicationUser>(ApiControllerExtensions.s_users);

            _controller = new GigsController(userManagerMock.Object, unitOfWOrkMock.Object);

            _controller.MockCurrentUser("1", "user1@domain.com");

        }
    }
}

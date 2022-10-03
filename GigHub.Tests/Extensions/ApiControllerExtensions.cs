using GigHub.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using System.Security.Principal;

namespace GigHub.Tests.Extensions
{
    public static class ApiControllerExtensions
    {
        // https://stackoverflow.com/questions/49165810/how-to-mock-usermanager-in-net-core-testing
        public static Mock<UserManager<TUser>> MockUserManager<TUser>(List<TUser> ls) where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            var mgr = new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
            mgr.Object.UserValidators.Add(new UserValidator<TUser>());
            mgr.Object.PasswordValidators.Add(new PasswordValidator<TUser>());

            mgr.Setup(x => x.DeleteAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);
            mgr.Setup(x => x.CreateAsync(It.IsAny<TUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success).Callback<TUser, string>((x, y) => ls.Add(x));
            mgr.Setup(x => x.UpdateAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);

            return mgr;
        }

        public static List<ApplicationUser> s_users = new List<ApplicationUser>
        {
            new ApplicationUser{ Name = "1", Email = "user1@domain.com" },
            new ApplicationUser{ Name = "2", Email = "user2@domain.com" }
        };

        public static void MockCurrentUser(this Controller controller, string userId, string userName)
        {
            var identity = new GenericIdentity(userName);
            identity.AddClaim(new Claim(
                "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", userName));
            identity.AddClaim(new Claim(
                "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", userId));

            var principal = new GenericPrincipal(identity, null);

            controller.HttpContext.User = principal;

        }
    }
}

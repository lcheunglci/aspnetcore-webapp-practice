using FluentAssertions;
using GigHub.Core.Models;

namespace GigHub.Tests.Domain.Models
{
    public class ApplicationUserTests
    {
        [Fact]
        public void Notify_WhenCalled_ShouldAddTheNotification()
        {
            var user = new ApplicationUser();
            var notification = Notification.GigCanceled(new Gig());

            user.Notify(notification);

            // We have 3 assertions here, but this does not mean this test method
            // is violating the single responibility principle. These 3 assertions
            // are highly related and we're logically verifying one fact: that
            // the user object will have one UserNotification object in the right
            // state (meaning its User and Notification properties are set properly).
            user.UserNotifications.Count.Should().Be(1);

            var userNotification = user.UserNotifications.First();
            userNotification.Notification.Should().Be(notification);
            userNotification.User.Should().Be(user);
        }
    }
}

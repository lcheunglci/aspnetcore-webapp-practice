namespace GigHub.Core.Repositories
{
    public interface IUnitOfWork
    {
        IAttendanceRepository Attendances { get; }
        IFollowingRepository Followings { get; }
        IGenreRepository Genres { get; }
        IGigRepository Gigs { get; }
        IApplicationUserRepository Users { get; }
        INotificationRepository Notifications { get; }
        IUserNotificationRepository UserNotifications { get; }

        void Complete();
    }
}
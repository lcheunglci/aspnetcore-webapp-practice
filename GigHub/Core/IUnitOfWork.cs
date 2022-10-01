namespace GigHub.Core.Repositories
{
    public interface IUnitOfWork
    {
        IAttendanceRepositroy Attendances { get; }
        IFollowingRepository Followings { get; }
        IGenreRepository Genres { get; }
        IGigRepository Gigs { get; }

        void Complete();
    }
}
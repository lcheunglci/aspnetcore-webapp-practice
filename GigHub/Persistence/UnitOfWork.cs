using GigHub.Core;
using GigHub.Core.Repositories;
using GigHub.Repositories;

namespace GigHub.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        ApplicationDbContext _context;

        public IGigRepository Gigs { get; private set; }
        public IAttendanceRepository Attendances { get; private set; }
        public IFollowingRepository Followings { get; private set; }
        public IGenreRepository Genres { get; private set; }


        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            Gigs = new GigRepository(_context);
            Attendances = new AttendanceRepositroy(_context);
            Followings = new FollowingRepository(_context);
            Genres = new GenreRepository(_context);

        }

        public void Complete()
        {
            _context.SaveChanges();
        }

    }
}

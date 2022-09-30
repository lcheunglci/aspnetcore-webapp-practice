using GigHub.Data;
using GigHub.Repositories;

namespace GigHub.Persistence
{
    public class UnitOfWork
    {
        ApplicationDbContext _context;

        public GigRepository Gigs { get; private set; }
        public AttendanceRepositroy Attendances { get; private set; }
        public FollowingRepository Followings { get; private set; }
        public GenreRepository Genres { get; private set; }


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

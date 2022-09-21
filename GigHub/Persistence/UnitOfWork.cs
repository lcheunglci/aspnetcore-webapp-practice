using GigHub.Data;
using GigHub.Repositories;

namespace GigHub.Persistence
{
    public class UnitOfWork
    {
        ApplicationDbContext _context;

        public GigRepository Gig { get; private set; }
        public AttendanceRepositroy Attendance { get; private set; }
        public FollowingRepository Following { get; private set; }
        public GenreRepository Genre { get; private set; }


        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            Gig = new GigRepository(_context);
            Attendance = new AttendanceRepositroy(_context);
            Following = new FollowingRepository(_context);
            Genre = new GenreRepository(_context);

        }

        public void Complete()
        {
            _context.SaveChanges();
        }

    }
}

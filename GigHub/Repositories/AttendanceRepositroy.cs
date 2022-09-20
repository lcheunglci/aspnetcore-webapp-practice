using GigHub.Data;
using GigHub.Models;

namespace GigHub.Repositories
{
    public class AttendanceRepositroy
    {
        private readonly ApplicationDbContext _context;

        public AttendanceRepositroy(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public IEnumerable<Attendance> GetFutureAttendances(string userId)
        {
            return _context.Attendances
                            .Where(a => a.AttendeeId == userId && a.Gig.DateTime > DateTime.Now)
                            .ToList();
        }
    }
}

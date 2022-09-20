using GigHub.Data;
using GigHub.Models;
using Microsoft.EntityFrameworkCore;

namespace GigHub.Repositories
{
    public class GigRepository
    {
        private readonly ApplicationDbContext _context;

        public GigRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<Gig> GetGigsUserAttending(string userId)
        {
            return _context.Attendances
                .Include(a => a.Gig)
                    .ThenInclude(g => g.Genre)
                .Include(a => a.Gig)
                    .ThenInclude(g => g.Artist)
                .Where(a => a.AttendeeId == userId)
                .Select(a => a.Gig)
                .ToList();
        }

    }
}

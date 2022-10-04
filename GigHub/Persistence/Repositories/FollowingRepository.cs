using GigHub.Core.Models;
using GigHub.Core.Repositories;
using GigHub.Persistence;

namespace GigHub.Repositories
{
    public class FollowingRepository : IFollowingRepository
    {
        ApplicationDbContext _context;

        public FollowingRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Following GetFollowing(string artistId, string userId)
        {
            return _context.Followings
                                .SingleOrDefault(f => f.FolloweeId == artistId && f.FollowerId == userId);
        }
    }
}

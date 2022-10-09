using GigHub.Core.Models;
using GigHub.Persistence;

namespace GigHub.IntegrationTests
{
    public class Utilities
    {
        public static void InitializeDbForTests(ApplicationDbContext context)
        {
            if (context.Users.Any())
                return;

            context.Users.AddRange(GetSeedingUsers());
            context.SaveChanges();
        }

        public static void ReinitializeDbForTests(ApplicationDbContext context)
        {
            context.Users.RemoveRange(context.Users);
            InitializeDbForTests(context);
        }

        public static List<ApplicationUser> GetSeedingUsers()
        {
            return new List<ApplicationUser>()
            {
                new ApplicationUser { UserName = "user1", Name = "user1", Email = "-", PasswordHash = "-" },
                new ApplicationUser { UserName = "user2", Name = "user2", Email = "-", PasswordHash = "-" }

            };
        }
    }
}

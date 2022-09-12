using GigHub.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GigHub.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Gig> Gigs { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Following> Followings { get; set; }

        public DbSet<Notification> Notifications { get; set; }
        public DbSet<UserNotification> UserNotifications { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.Entity<Attendance>().HasKey(table =>
            //    new
            //    {
            //        table.AttendeeId,
            //        table.GigId
            //    });

            builder.Entity<Attendance>()
                .HasKey(nameof(Attendance.GigId), nameof(Attendance.AttendeeId));

            builder.Entity<Attendance>()
                .HasOne(a => a.Gig)
                .WithMany()
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Following>()
                .HasKey(nameof(Following.FollowerId), nameof(Following.FolloweeId));

            builder.Entity<ApplicationUser>()
                .HasMany(u => u.Followers)
                .WithOne(f => f.Followee)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);


            builder.Entity<ApplicationUser>()
                .HasMany(u => u.Followees)
                .WithOne(f => f.Follower)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UserNotification>()
                .HasKey(nameof(UserNotification.NotificationId), nameof(UserNotification.UserId));

            builder.Entity<Notification>()
                .HasOne(n => n.Gig)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);


            base.OnModelCreating(builder);
        }
    }
}
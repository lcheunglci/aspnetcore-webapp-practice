using GigHub.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GigHub.Persistence.EntityConfigurations
{
    public class UserNotificationConfiguration : IEntityTypeConfiguration<UserNotification>
    {
        public void Configure(EntityTypeBuilder<UserNotification> builder)
        {
            builder.HasKey(u => u.UserId);
            builder.HasKey(u => u.NotificationId);

            builder.Property(u => u.UserId)
                .HasColumnOrder(1);
            builder.Property(u => u.NotificationId)
                .HasColumnOrder(2);

            builder.HasKey(nameof(UserNotification.NotificationId), nameof(UserNotification.UserId));

            builder.HasOne(n => n.User)
                .WithMany(u => u.UserNotifications)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

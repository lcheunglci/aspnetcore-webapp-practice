using GigHub.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GigHub.Persistence.EntityConfigurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.Property(n => n.Gig)
                .IsRequired();

            builder.HasOne(n => n.Gig)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

using GigHub.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GigHub.Persistence.EntityConfigurations
{
    public class FollowingConfiguration : IEntityTypeConfiguration<Following>
    {
        public void Configure(EntityTypeBuilder<Following> builder)
        {
            builder.HasKey(f => f.FollowerId);
            builder.HasKey(f => f.FolloweeId);

            builder.Property(f => f.FollowerId)
                .HasColumnOrder(1);

            builder.Property(f => f.FolloweeId)
                .HasColumnOrder(2);

            builder.HasKey(nameof(Following.FollowerId), nameof(Following.FolloweeId));

        }
    }
}

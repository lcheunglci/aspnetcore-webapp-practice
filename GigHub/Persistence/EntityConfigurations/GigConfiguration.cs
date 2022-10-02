using GigHub.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GigHub.Persistence.EntityConfigurations
{
    public class GigConfiguration : IEntityTypeConfiguration<Gig>
    {
        public void Configure(EntityTypeBuilder<Gig> builder)
        {
            builder.Property(g => g.ArtistId)
                .IsRequired();

            builder.Property(g => g.GenreId)
                .IsRequired();

            builder.Property(g => g.Venue)
                .IsRequired()
                .HasMaxLength(255);


            builder.HasMany(g => g.Attendances)
                .WithOne(a => a.Gig)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

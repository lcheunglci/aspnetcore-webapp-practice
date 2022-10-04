using GigHub.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GigHub.Persistence.EntityConfigurations
{
    public class AttendanceConfiguration : IEntityTypeConfiguration<Attendance>
    {
        public void Configure(EntityTypeBuilder<Attendance> builder)
        {
            builder.HasKey(a => a.GigId);
            builder.HasKey(a => a.AttendeeId);

            builder.Property(a => a.GigId)
                .HasColumnOrder(1);

            builder.Property(a => a.AttendeeId)
                .HasColumnOrder(2);


            builder.HasKey(nameof(Attendance.GigId), nameof(Attendance.AttendeeId));

            builder.HasOne(a => a.Gig)
                .WithMany(a => a.Attendances)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

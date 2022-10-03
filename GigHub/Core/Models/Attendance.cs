namespace GigHub.Models
{
    public class Attendance
    {
        public Gig Gig { get; set; }
        public ApplicationUser Attendee { get; set; }

        // In EF Core, Composite Keys are generated from the DbContext using the modelBuilder with .HasKey(...)
        // [Required]
        public int GigId { get; set; }

        // [Required]
        public string AttendeeId { get; set; }

    }
}

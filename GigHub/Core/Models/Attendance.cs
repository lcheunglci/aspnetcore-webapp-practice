using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GigHub.Models
{
    public class Attendance
    {
        public Gig Gig { get; set; }
        public ApplicationUser Attendee { get; set; }

        // In EF Core, Composite Keys are generated from the DbContext using the modelBuilder with .HasKey(...)
        [Key]
        [Column(Order = 1)]
        // [Required]
        public int GigId { get; set; }

        [Key]
        [Column(Order = 2)]
        // [Required]
        public string AttendeeId { get; set; }

    }
}

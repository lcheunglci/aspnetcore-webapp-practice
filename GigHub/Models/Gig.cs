using Microsoft.AspNetCore.Identity;

namespace GigHub.Models
{
    public class Gig
    {
        public int Id { get; set; }
        public IdentityUser Artist { get; set; }
        public DateTime DateTime { get; set; }
        public string Venue { get; set; }

        public Genre Genre { get; set; }
    }

}

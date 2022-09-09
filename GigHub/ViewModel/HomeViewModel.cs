using GigHub.Models;

namespace GigHub.ViewModel
{
    public class HomeViewModel
    {
        public IEnumerable<Gig> upcomingGigs { get; set; }
        public bool ShowActions { get; set; }
    }
}
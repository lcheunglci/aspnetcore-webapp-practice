using GigHub.Models;
using System.ComponentModel.DataAnnotations;

namespace GigHub.ViewModel
{
    public class GigFormViewModel
    {
        [Required]
        public string Venue { get; set; }
        
        [Required]
        public string Date { get; set; }
        
        [Required]
        public string Time { get; set; }
        
        [Required]
        public byte Genre { get; set; }

        public IEnumerable<Genre> Genres { get; set; }
        public DateTime GetDateTime()
        {
            return DateTime.Parse($"{Date} {Time}");
        }
    }
}

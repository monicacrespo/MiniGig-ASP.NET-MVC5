using System;
using System.ComponentModel.DataAnnotations;

namespace MvcMiniGigApp.Domain
{
    public class Gig
    {
        public int Id { get; set; }
        [Required]

        public string Name { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Gig Date")]
        public DateTime GigDate { get; set; }
        
        [Display(Name = "Music Genre")]
        public int MusicGenreId { get; set; }
        public virtual MusicGenre MusicGenre { get; set; }

    }
}

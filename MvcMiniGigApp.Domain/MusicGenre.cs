using System.ComponentModel.DataAnnotations;

namespace MvcMiniGigApp.Domain
{
    /// <summary>
    /// Class which contains the music genre data
    /// </summary>
    public class MusicGenre
    {
        public int Id { get; set; }

        //Alternate keys can not be configured using Data Annotations.
        [Display(Name = "Music Genre")]
        public string Category { get; set; }
    }
    
}

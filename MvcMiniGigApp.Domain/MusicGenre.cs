
using System.ComponentModel.DataAnnotations;

namespace MvcMiniGigApp.Domain
{
    /// <summary>
    /// Class which contains the musicType data
    /// </summary>
    public class MusicGenre
    {
        public int Id { get; set; }

        [Display(Name = "Music Genre")]
        public string Category { get; set; }
    }
    
}

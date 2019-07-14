namespace MvcMiniGigApp.Services
{
    using System.Collections.Generic;
    using MvcMiniGigApp.Domain;

    public interface IGigService
    {
        IEnumerable<MusicGenre> GetMusicGenres();

        IEnumerable<Gig> GetGigs();

        Gig GetGig(int id); 

        void CreateGig(Gig gig);

        void DeleteGig(int id);

        void UpdateGig(Gig gig);
    }
}
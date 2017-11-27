using System.Collections.Generic;
using System.Linq;
using DisconnectedGenericRepository;
using MvcMiniGigApp.Domain;


namespace MvcMiniGigApp.Services
{
    public class GigService: IGigService
    {      
        private readonly GenericRepository<Gig> gigRepository;
        private readonly GenericRepository<MusicGenre> musicGenreRepository;
        public GigService(GenericRepository<Gig> _gigRepository,
                          GenericRepository<MusicGenre> _musicGenreRepository)
        {
            gigRepository = _gigRepository;
            musicGenreRepository = _musicGenreRepository;
        }

        public IEnumerable<MusicGenre> GetMusicGenres()
        {
            return musicGenreRepository.All();
        }

        public IEnumerable<Gig> GetGigs()
        {         
            return gigRepository.AllInclude(g=>g.Name, OrderByType.Ascending, g => g.MusicGenre);
        }

        public Gig GetGig(int id)
        {
            var gig = gigRepository.FindByInclude(c => c.Id == id, c => c.MusicGenre).FirstOrDefault();
            return gig;
        }     
        
        public void CreateGig(Gig gig)
        {
            gigRepository.Insert(gig);
        }

        public void DeleteGig(int id)
        {
            gigRepository.Delete(id);
        }

        public void UpdateGig(Gig gig)
        {
            gigRepository.Update(gig);           
        }

    }
}

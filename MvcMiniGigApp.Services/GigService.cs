namespace MvcMiniGigApp.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using DisconnectedGenericRepository;
    using MvcMiniGigApp.Domain;

    public class GigService : IGigService
    {      
        private readonly GenericRepository<Gig> gigRepository;
        private readonly GenericRepository<MusicGenre> musicGenreRepository;
        public GigService(GenericRepository<Gig> _gigRepository, 
                            GenericRepository<MusicGenre> _musicGenreRepository)
        {
            this.gigRepository = _gigRepository;
            this.musicGenreRepository = _musicGenreRepository;
        }

        public IEnumerable<MusicGenre> GetMusicGenres()
        {
            return this.musicGenreRepository.All();
        }

        public IEnumerable<Gig> GetGigs()
        {         
            return this.gigRepository.AllInclude(g => g.Name, OrderByType.Ascending, g => g.MusicGenre);
        }

        public Gig GetGig(int id)
        {
            var gig = this.gigRepository.FindByInclude(c => c.Id == id, c => c.MusicGenre).FirstOrDefault();
            return gig;
        }     
        
        public void CreateGig(Gig gig)
        {
            this.gigRepository.Insert(gig);
        }

        public void DeleteGig(int id)
        {
            this.gigRepository.Delete(id);
        }

        public void UpdateGig(Gig gig)
        {
            this.gigRepository.Update(gig);           
        }
    }
}
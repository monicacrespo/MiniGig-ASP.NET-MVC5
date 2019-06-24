using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcMiniGigApp.Domain;
using MvcMiniGigApp.Data;
using DisconnectedGenericRepository;
using System.Data.Entity;

namespace MvcMiniGigApp.Services.Tests
{
    /// <summary>
    /// Summary description for GigServicesTests
    /// </summary>
    [TestClass]
    public class GigServiceTests
    {
        private const int NON_EXISTENT_GIG_ID = -1;
        private static IGigService gigService;

        private MiniGigContext _context;
        private GenericRepository<Gig> _gigRepository;
        private GenericRepository<MusicGenre> _musicGenreRepository;

        public GigServiceTests()
        {
            //app.config points to a special testing database          
            Database.SetInitializer(new NullDatabaseInitializer<MiniGigContext>());
            _context = new MiniGigContext();
            _gigRepository = new GenericRepository<Gig>(_context);
            _musicGenreRepository = new GenericRepository<MusicGenre>(_context);
            gigService = new GigService(_gigRepository, _musicGenreRepository);
        }

        [TestMethod()]
        public void CanInsertGigWithMusicGenre()
        {
            var gig = new Gig
            {
                Name = "Freddie Mercury tribute concert",
                GigDate = DateTime.Now,
                MusicGenreId = 1
            };
            gigService.CreateGig(gig);
            Assert.AreNotEqual(0, gig.Id);
        }

        [TestMethod()]
        public void FindNonExistentGig()
        {
            Gig actual = gigService.GetGig(NON_EXISTENT_GIG_ID);
            Assert.IsNull(actual);
        }
        
        private IEnumerable<Gig> GetDummyGigs(int count)
        {
            var popMusicInMemory = new MusicGenre { Id = 1, Category = "Pop" };

            DateTime endDate;
            DateTime tempDate = DateTime.Now;
            for (int i = 0; i < count; i++)
            {
                tempDate = tempDate.AddMonths(i);
                yield return new Gig
                {
                    Id = i,
                    Name = string.Concat("gigNameTest", i),
                    GigDate = tempDate,
                    MusicGenreId = 1
                    ,
                    MusicGenre = popMusicInMemory
                };
            }
            endDate = tempDate;
        }
    }
}

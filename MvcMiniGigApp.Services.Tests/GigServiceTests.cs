namespace MvcMiniGigApp.Services.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using DisconnectedGenericRepository;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MvcMiniGigApp.Data;
    using MvcMiniGigApp.Domain;
    
    [TestClass]
    public class GigServiceTests
    {
        private const int NONEXISTENTGIGID = -1;
        private static IGigService gigService;

        private MiniGigContext context;
        private GenericRepository<Gig> gigRepository;
        private GenericRepository<MusicGenre> musicGenreRepository;

        public GigServiceTests()
        {
            // app.config points to a special testing database
            Database.SetInitializer(new NullDatabaseInitializer<MiniGigContext>());
            this.context = new MiniGigContext();
            this.gigRepository = new GenericRepository<Gig>(this.context);
            this.musicGenreRepository = new GenericRepository<MusicGenre>(this.context);
            gigService = new GigService(this.gigRepository, this.musicGenreRepository);
        }

        [TestMethod]
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

        [TestMethod]
        public void FindNonExistentGig()
        {
            Gig actual = gigService.GetGig(NONEXISTENTGIGID);
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
                    MusicGenreId = 1,
                    MusicGenre = popMusicInMemory
                };
            }

            endDate = tempDate;
        }
    }
}
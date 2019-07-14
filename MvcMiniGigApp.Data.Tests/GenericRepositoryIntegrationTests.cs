//-----------------------------------------------------------------------
// <copyright file="GenericRepositoryIntegrationTests.cs" company="MCA">
//    Copyright 2013 - 2021, Monica Crespo Arjona
// </copyright>
//-----------------------------------------------------------------------
namespace MvcMiniGigApp.Data.Tests
{
    using System;
    using System.Data.Entity;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using DisconnectedGenericRepository;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MvcMiniGigApp.Domain;

    [TestClass]
    public class GenericRepositoryIntegrationTests
    {
        private StringBuilder logBuilder = new StringBuilder();
        private string log;
        private MiniGigContext context;
        private GenericRepository<Gig> gigRepository;
        private GenericRepository<MusicGenre> musicGenreRepository;

        public GenericRepositoryIntegrationTests()
        {
            // Using this initializer disables database initialization for the given context type
            Database.SetInitializer(new NullDatabaseInitializer<MiniGigContext>());
            this.context = new MiniGigContext();
            this.gigRepository = new GenericRepository<Gig>(this.context);
            this.musicGenreRepository = new GenericRepository<MusicGenre>(this.context);
            this.SetupLogging();
        }

        [TestMethod]
        public void CanFindByGigByKeyWithFindMethod()
        {
            var existingGig = this.GetDefaultGig();
            this.gigRepository.Insert(existingGig);
            int gigId = existingGig.Id;

            var results = this.gigRepository.FindByKey(gigId);
            this.WriteLog();
            Assert.IsTrue(this.log.Contains("FROM [dbo].[Gigs"));
        }

        [TestMethod]
        public void CanFindByMusicGenreByKeyWithFindMethod()
        {
            // hard - code music genre id is 1. See GetDefaultGig() method
            var results = this.musicGenreRepository.FindByKey(1);
            this.WriteLog();
            Assert.IsTrue(this.log.Contains("FROM [dbo].[MusicGenres"));
        }

        [TestMethod]
        public void NoTrackingQueriesDoNotCacheObjects()
        {
            var results = this.gigRepository.All();
            Assert.AreEqual(0, this.context.ChangeTracker.Entries().Count());
        }

        [TestMethod]
        public void CanQueryWithSinglePredicate()
        {
            var results = this.gigRepository.FindBy(c => c.Name.StartsWith("D"));
            this.WriteLog();
            Assert.IsTrue(this.log.Contains("'D%'"));
        }

        [TestMethod]
        public void CanQueryWithDualPredicate()
        {
            var date = new DateTime(2019, 5, 2);
            var results = this.gigRepository
              .FindBy(c => c.Name.StartsWith("D") && c.GigDate >= date);
            this.WriteLog();
            Assert.IsTrue(this.log.Contains("'D%'") && this.log.Contains("02/05/2019"));
        }

        [TestMethod]
        public void GetAllIncludingComprehendsSingleNavigation()
        {
            var results = this.gigRepository.AllInclude(c => c.MusicGenre);
            this.WriteLog();
            Assert.IsTrue(this.log.Contains("MusicGenre"));
        }

        [TestMethod]
        public void FindByIncludingComprehendsSingleNavigation()
        {
            var results = this.gigRepository.FindByInclude(c => c.Id == 1, c => c.MusicGenre);
            this.WriteLog();
            Assert.IsTrue(this.log.Contains("MusicGenre") && this.log.Contains("1"));
        }

        [TestMethod]
        public void GetAllIncludingFirstOrDefaultById()
        {
            var results = this.gigRepository.AllInclude(c => c.MusicGenre).FirstOrDefault(n => n.Id == 1);
            this.WriteLog();
            Assert.IsTrue(this.log.Contains("MusicGenre"));
        }

        [TestMethod]
        public void ComposedOnAllListExecutedInMemory()
        {
            this.gigRepository.All().Where(c => c.Name == "Chacers").ToList();
            this.WriteLog();
            Assert.IsFalse(this.log.Contains("Chacers"));
        }

        private void WriteLog()
        {
            Debug.WriteLine(this.log);
        }

        private void SetupLogging()
        {
            this.context.Database.Log = this.BuildLogString;
        }

        private void BuildLogString(string message)
        {
            this.logBuilder.Append(message);
            this.log = logBuilder.ToString();
        }

        private Gig GetDefaultGig()
        {
            var defaultGenreInMemory = new MusicGenre { Id = 1, Category = "Default Genre" };
            return new Gig
            {
                Id = 100,
                Name = string.Format("Default Gig Name"),
                GigDate = new DateTime(2019, 5, 2),
                MusicGenreId = 1,
                MusicGenre = defaultGenreInMemory
            };
        }
    }
}
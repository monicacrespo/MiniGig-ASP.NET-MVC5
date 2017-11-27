using DisconnectedGenericRepository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcMiniGigApp.Domain;
using System;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MvcMiniGigApp.Data.Tests
{
    /// <summary>
    /// Summary description for GenericRepositoryIntegrationTests
    /// </summary>
    [TestClass]
    public class GenericRepositoryIntegrationTests
    {
        private StringBuilder _logBuilder = new StringBuilder();
        private string _log;
        private MiniGigContext _context;
        private GenericRepository<Gig> _gigRepository;
        private GenericRepository<MusicGenre> _musicGenreRepository;


        public GenericRepositoryIntegrationTests()
        {
            Database.SetInitializer(new NullDatabaseInitializer<MiniGigContext>());
            _context = new MiniGigContext();
            _gigRepository = new GenericRepository<Gig>(_context);
            _musicGenreRepository = new GenericRepository<MusicGenre>(_context);
            SetupLogging();
        }

        [TestMethod]
        public void CanFindByGigByKeyWithFindMethod()
        {
            var results = _gigRepository.FindByKey(1);
            WriteLog();
            Assert.IsTrue(_log.Contains("FROM [dbo].[Gigs"));
        }

        [TestMethod]
        public void CanFindByMusicGenreByKeyWithDynamicLambda()
        {
            var results = _musicGenreRepository.FindByKey(1);
            WriteLog();
            Assert.IsTrue(_log.Contains("FROM [dbo].[MusicGenres"));
        }

        [TestMethod]
        public void NoTrackingQueriesDoNotCacheObjects()
        {
            var results = _gigRepository.All();
            Assert.AreEqual(0, _context.ChangeTracker.Entries().Count());
        }

        [TestMethod]
        public void CanQueryWithSinglePredicate()
        {
            var results = _gigRepository.FindBy(c => c.Name.StartsWith("L"));
            WriteLog();
            Assert.IsTrue(_log.Contains("'L%'"));
        }

        [TestMethod]
        public void CanQueryWithDualPredicate()
        {
            var date = new DateTime(2016, 5, 2);
            var results = _gigRepository
              .FindBy(c => c.Name.StartsWith("L") && c.GigDate >= date);
            WriteLog();
            Assert.IsTrue(_log.Contains("'L%'") && _log.Contains("02/05/2016"));
        }

        [TestMethod]
        public void CanQueryWithComplexRelatedPredicate()
        {
            var date = new DateTime(2016, 5, 2);
            var results = _gigRepository
               .FindBy(c => c.Name.StartsWith("L") && c.GigDate >= date
                                                       && c.MusicGenreId == 1);
            WriteLog();
            Assert.IsTrue(_log.Contains("'L%'") && _log.Contains("02/05/2016") && _log.Contains("1"));
        }

        [TestMethod]
        public void GetAllIncludingComprehendsSingleNavigation()
        {
            var results = _gigRepository.AllInclude(c => c.MusicGenre);
            WriteLog();
            Assert.IsTrue(_log.Contains("MusicGenre"));
        }
        [TestMethod]
        public void FindByIncludingComprehendsSingleNavigation()
        {
            var results = _gigRepository.FindByInclude(c => c.Id == 1, c => c.MusicGenre);
            WriteLog();
            Assert.IsTrue(_log.Contains("MusicGenre") && _log.Contains("1"));
        }
        [TestMethod]
        public void GetAllIncludingFirstOrDefaultById()
        {
            var results = _gigRepository.AllInclude(c => c.MusicGenre).FirstOrDefault(n => n.Id == 1);
            WriteLog();
            Assert.IsTrue(_log.Contains("MusicGenre"));
        }

        [TestMethod]
        public void ComposedOnAllListExecutedInMemory()
        {
            _gigRepository.All().Where(c => c.Name == "Chacers").ToList();
            WriteLog();
            Assert.IsFalse(_log.Contains("Chacers"));
        }

        private void WriteLog()
        {
            Debug.WriteLine(_log);
        }

        private void SetupLogging()
        {
            _context.Database.Log = BuildLogString;
        }

        private void BuildLogString(string message)
        {
            _logBuilder.Append(message);
            _log = _logBuilder.ToString();
        }


    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcMiniGigApp.Data;
using MvcMiniGigApp.Domain;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Moq;
using DisconnectedGenericRepository;
using System;
namespace MvcMiniGigApp.Tests.Data
{
  [TestClass]
  public class UnitTestsWithMoq {
        List<Gig> expectedGigsInMemory;
        Gig expectedGigInMemory;

        //Use TestInitialize to run code before running each test
        [TestInitialize()]
        public void MyTestInitialize()
        {
            var popMusicInMemory = new MusicGenre { Id = 1, Category = "Pop" };

            expectedGigInMemory = new Gig
            {
                Id = 1,
                Name = "The Chacer's Live Gig",
                GigDate = new DateTime(2017, 12, 2),
                MusicGenreId = 1,
                MusicGenre = popMusicInMemory
            };
            expectedGigsInMemory = new List<Gig> { expectedGigInMemory };
        }

        #region Testing query scenarios
        
        [TestMethod]
        public void CanGetAllIncludeGigListDbContextAndDbSetMocked()
        {
            //arrange
            var mockGigObjectDbSet = GetQueryableMockDbSet<Gig>(expectedGigsInMemory);

            var miniGigMockingContext = new Mock<MiniGigContext>();
            miniGigMockingContext.Setup(c => c.Set<Gig>()).Returns(mockGigObjectDbSet.Object);
            var gigRepo = new GenericRepository<Gig>(miniGigMockingContext.Object);

            //act
            var actualGigsAllInclude = gigRepo.AllInclude(g => g.MusicGenre).ToList();
            
            //assert
            Assert.IsNotNull(gigRepo);          
            CollectionAssert.AreEqual(expectedGigsInMemory, actualGigsAllInclude);
            Assert.AreEqual(expectedGigsInMemory[0].Name, actualGigsAllInclude[0].Name);
        }
        
        [TestMethod]
        public void CanGetAllGigListDbSetMocked()
        {
            //arrange           
            var mockGigObjectDbSet = GetQueryableMockDbSet<Gig>(expectedGigsInMemory);

            var miniGigMockingContext = new MiniGigContext { Gigs = mockGigObjectDbSet.Object };
            
            //act
            var gigRepo = new GenericRepository<Gig>(miniGigMockingContext);
            var actualGig = gigRepo.All().FirstOrDefault();
            
            //assert
            Assert.IsNotNull(actualGig);
            Assert.IsInstanceOfType(actualGig, typeof(Gig));
        }
        #endregion

        #region Testing non-query scenarios

        [TestMethod]
        public void CanCreateGigAndSaveAGigViaContext()
        {
            //arrange
            var newGigsInMemory = new List<Gig>();
            var mockGigObjectDbSet = GetQueryableMockDbSet<Gig>(newGigsInMemory);

            var miniGigMockingContext = new Mock<MiniGigContext>();
            miniGigMockingContext.Setup(c => c.Set<Gig>()).Returns(mockGigObjectDbSet.Object);
            var gigRepo = new GenericRepository<Gig>(miniGigMockingContext.Object);

            //act
            gigRepo.Insert(expectedGigInMemory);

            //assert
            mockGigObjectDbSet.Verify(m => m.Add(It.IsAny<Gig>()), Times.Once());
            miniGigMockingContext.Verify(m => m.SaveChanges(), Times.Once());
        }
        #endregion

        private static Mock<DbSet<T>> GetQueryableMockDbSet<T>(List<T> inMemoryData) where T : class
        {
            if (inMemoryData == null)
            {
                inMemoryData = new List<T>();
            }

            var queryableData = inMemoryData.AsQueryable();
            var mockDbSet = new Mock<DbSet<T>>();

            mockDbSet.Setup(m => m.Add(It.IsAny<T>())).Callback<T>(inMemoryData.Add);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryableData.Provider);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryableData.Expression);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryableData.GetEnumerator());
            //mock the AsNoTracking() method
            mockDbSet.Setup(x => x.AsNoTracking()).Returns(mockDbSet.Object);
            //mock the Include() method
            mockDbSet.Setup(x => x.Include(It.IsAny<string>())).Returns(mockDbSet.Object);            
            return mockDbSet;
        }
       
    }
}

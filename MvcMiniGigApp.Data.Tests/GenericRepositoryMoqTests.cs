using DisconnectedGenericRepository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcMiniGigApp.Domain;
using SharedKernel.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Moq;
namespace MvcMiniGigApp.Data.Tests
{
    /// <summary>
    /// Summary description for GenericRepositoryMoqTests
    /// </summary>
    [TestClass]
    public class GenericRepositoryMoqTests
    {
        public GenericRepositoryMoqTests()
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<MiniGigContext>());
        }

        #region Testing query scenarios

        [TestMethod]
        public void Get_AllInclude_Should_Get_Expected_Amount_Of_Entities()
        {
            //arrange
            var gigsInMemory = GetDummyGigs(2).ToList();
            var mockGigObjectDbSet = TestHelpers.GetQueryableMockDbSet<Gig>(gigsInMemory);
            var miniGigMockingContext = new Mock<MiniGigContext>();
            miniGigMockingContext.Setup(c => c.Set<Gig>()).Returns(mockGigObjectDbSet.Object);
            var gigRepo = new GenericRepository<Gig>(miniGigMockingContext.Object);

            //act
            var actualGigs = gigRepo.AllInclude(g => g.MusicGenre).ToList();

            //assert            
            Assert.AreEqual(gigsInMemory.Count(), actualGigs.Count);
            Assert.AreEqual(gigsInMemory[0].Name, actualGigs[0].Name);
        }

        [TestMethod]
        public void Get_AllInclude_GigListSortedDbContextAndDbSetMocked()
        {
            //arrange           
            var expectedGigsInMemory = new List<Gig>
            {
                new Gig {Name = "BBB"},
                new Gig {Name = "ZZZ"},
                new Gig {Name = "AAA"},
            };

            var mockGigObjectDbSet = TestHelpers.GetQueryableMockDbSet(expectedGigsInMemory);

            var miniGigMockingContext = new Mock<MiniGigContext>();

            //setup the Set method on the DbContext for your mocked DbSet.
            miniGigMockingContext.Setup(c => c.Set<Gig>()).Returns(mockGigObjectDbSet.Object);

            //act
            var gigRepo = new GenericRepository<Gig>(miniGigMockingContext.Object);
            var actualGigs = gigRepo.AllInclude(g => g.Name, OrderByType.Ascending, g => g.MusicGenre).ToList();

            //assert
            Assert.AreEqual(expectedGigsInMemory.Count, actualGigs.Count());// Page count is available after calling the .ToList() method
            Assert.AreEqual(expectedGigsInMemory[2].Name, actualGigs[0].Name);
            Assert.AreEqual("BBB", actualGigs[1].Name);
            Assert.AreEqual(expectedGigsInMemory[1].Name, actualGigs[2].Name);
        }
        [TestMethod]
        public void Get_All_Should_Get_Expected_Amount_Of_Entities()
        {
            //arrange
            var gigsInMemory = GetDummyGigs(2).ToList();
            var mockGigObjectDbSet = TestHelpers.GetQueryableMockDbSet<Gig>(gigsInMemory);
            var miniGigMockingContext = new Mock<MiniGigContext>();
            miniGigMockingContext.Setup(c => c.Set<Gig>()).Returns(mockGigObjectDbSet.Object);
            var gigRepo = new GenericRepository<Gig>(miniGigMockingContext.Object);

            //act
            var actualGigs = gigRepo.All().ToList();

            //assert
            Assert.AreEqual(gigsInMemory.Count(), actualGigs.Count);
        }

        [TestMethod]
        public void GetSingle_Should_Get_Expected_Entity()
        {
            // Arrange

            var targetGigId = 2;
            var gigsInMemory = GetDummyGigs(3).ToList();
            var mockGigObjectDbSet = TestHelpers.GetQueryableMockDbSet<Gig>(gigsInMemory);
            var miniGigMockingContext = new Mock<MiniGigContext>();
            miniGigMockingContext.Setup(c => c.Set<Gig>()).Returns(mockGigObjectDbSet.Object);
            var gigRepo = new GenericRepository<Gig>(miniGigMockingContext.Object);
            var expectedGig = gigsInMemory.FirstOrDefault(x => x.Id == targetGigId);

            // Act
            //Gig gig = gigRepo.FindByKey(targetGigId);            
            var gig = gigRepo.FindBy(x => x.Id == targetGigId).FirstOrDefault();

            // Assert          
            Assert.AreSame(expectedGig, gig);
            Assert.IsInstanceOfType(gig, typeof(Gig));
        }

        [TestMethod]
        public void GetSingle_Should_Return_Null()
        {
            // Arrange
            var targetGigId = 4;
            var gigsInMemory = GetDummyGigs(3).ToList();
            var mockGigObjectDbSet = TestHelpers.GetQueryableMockDbSet<Gig>(gigsInMemory);
            var miniGigMockingContext = new Mock<MiniGigContext>();
            miniGigMockingContext.Setup(c => c.Set<Gig>()).Returns(mockGigObjectDbSet.Object);
            var gigRepo = new GenericRepository<Gig>(miniGigMockingContext.Object);
            var expectedGig = gigsInMemory.FirstOrDefault(x => x.Id == targetGigId);

            // Act
            Gig gig = gigRepo.FindByKey(targetGigId);

            // Assert         
            Assert.AreSame(expectedGig, gig);
        }


        #endregion

        #region Testing non-query scenarios

        [TestMethod]
        public void Insert_Should_Call_Add_Once_And_SaveChanges()
        {
            //arrange
            var newGigsInMemory = new List<Gig>();
            var gigsInMemory = GetDummyGigs(1).ToList();
            var mockGigObjectDbSet = TestHelpers.GetQueryableMockDbSet<Gig>(newGigsInMemory);

            var miniGigMockingContext = new Mock<MiniGigContext>();
            miniGigMockingContext.Setup(c => c.Set<Gig>()).Returns(mockGigObjectDbSet.Object);
            var gigRepo = new GenericRepository<Gig>(miniGigMockingContext.Object);

            //act
            gigRepo.Insert(gigsInMemory.FirstOrDefault());

            //assert
            mockGigObjectDbSet.Verify(m => m.Add(It.IsAny<Gig>()), Times.Once());
            miniGigMockingContext.Verify(m => m.SaveChanges(), Times.Once());
        }
        #endregion

        private IEnumerable<Gig> GetDummyGigs(int count)
        {
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
                };
            }
        }

        private List<Gig> GetDummyGigList(int count)
        {
            var gigsInMemory = new List<Gig>();
            DateTime tempDate = DateTime.Now;
            for (int i = 0; i < count; i++)
            {
                tempDate = tempDate.AddMonths(i);
                var gig = new Gig
                {
                    Id = i,
                    Name = string.Concat("gigNameTest", i),
                    GigDate = tempDate,
                    MusicGenreId = 1
                };
                gigsInMemory.Add(gig);
            }
            return gigsInMemory;
        }
    }
}

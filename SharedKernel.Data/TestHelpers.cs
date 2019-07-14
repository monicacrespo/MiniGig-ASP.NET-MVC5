namespace SharedKernel.Data
{
	using System;
	using System.Collections.Generic;
	using System.Data.Entity;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using Moq;

    public static class TestHelpers
    {
        public static Mock<DbSet<T>> GetQueryableMockDbSet<T>() where T : class
        {
            return GetQueryableMockDbSet<T>(null);
        }
        public static Mock<DbSet<T>> GetQueryableMockDbSet<T>(List<T> inMemoryData) where T : class
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
            // Mock the AsNoTracking() method
            mockDbSet.Setup(x => x.AsNoTracking()).Returns(mockDbSet.Object);
            // Mock the Include() method
            mockDbSet.Setup(x => x.Include(It.IsAny<string>())).Returns(mockDbSet.Object);
            return mockDbSet;
        }
    }
}
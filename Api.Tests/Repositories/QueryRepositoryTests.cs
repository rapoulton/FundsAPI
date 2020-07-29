using Api.DataSources;
using Api.Repositories;
using Api.Repositories.Specifications;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Api.Tests.Repositories
{
    public class QueryRepositoryTests
    {
        [Fact]
        public async void GetFirstOrDefaultDataEntity_WithManyResults_ReturnsFirstItem()
        {
            var dataSourceMock = new Mock<IDataSource<TestModel>>();
            dataSourceMock.Setup(m => m.GetIQueryable()).ReturnsAsync(
                new List<TestModel>() { 
                    new TestModel() { id = 2, order = 1 },
                    new TestModel() { id = 2, order = 2 },
                    new TestModel() { id = 10, order = 3 }
                }.AsQueryable());

            IQueryRepository<TestModel> queryRepository = new QueryRepository<TestModel>(dataSourceMock.Object);

            var result = await queryRepository.GetFirstOrDefaultDataEntity(new TestSpecification(2));

            Assert.NotNull(result);
            Assert.Equal(1, result.order);
        }

        [Fact]
        public async void GetFirstOrDefaultDataEntity_WithNoResults_ReturnsNull()
        {
            var dataSourceMock = new Mock<IDataSource<TestModel>>();
            dataSourceMock.Setup(m => m.GetIQueryable()).ReturnsAsync(
                new List<TestModel>() {
                    new TestModel() { id = 2, order = 1 },
                    new TestModel() { id = 3, order = 2 },
                    new TestModel() { id = 10, order = 3 }
                }.AsQueryable());

            IQueryRepository<TestModel> queryRepository = new QueryRepository<TestModel>(dataSourceMock.Object);

            var result = await queryRepository.GetFirstOrDefaultDataEntity(new TestSpecification(9));

            Assert.Null(result);
        }

        [Fact]
        public async void GetDataEntities_WithManyResults_ReturnsIEnumerableContainingExpectedItems()
        {
            var dataSourceMock = new Mock<IDataSource<TestModel>>();
            dataSourceMock.Setup(m => m.GetIQueryable()).ReturnsAsync(
                new List<TestModel>() {
                    new TestModel() { id = 2, order = 1 },
                    new TestModel() { id = 2, order = 2 },
                    new TestModel() { id = 10, order = 3 }
                }.AsQueryable());

            IQueryRepository<TestModel> queryRepository = new QueryRepository<TestModel>(dataSourceMock.Object);

            var results = await queryRepository.GetDataEntities(new TestSpecification(2));

            Assert.NotNull(results);
            Assert.Equal(2, results.Count());
        }

        #region Helper Inner classes
        public class TestModel
        {
            public int id { get; set; }
            public int order { get; set; }
        }

        public class TestSpecification : ISpecification<TestModel>
        {
            private readonly int _id;

            public TestSpecification(int id)
            {
                _id = id;
            }

            public IQueryable<TestModel> ApplySpecification(IQueryable<TestModel> baseQuery)
            {
                return baseQuery.Where(m => m.id == _id);
            }
        }
        #endregion
    }
}

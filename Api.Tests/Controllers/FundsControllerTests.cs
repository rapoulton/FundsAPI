using Api.Controllers;
using Api.DataModels;
using Api.Repositories;
using Api.Repositories.Specifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Api.Tests.Controllers
{
    public class FundsControllerTests
    {
        [Fact]
        public async void GetFundByMarketCode_WhenNoMatch_ReturnsNotFound()
        {
            var mockRepository = new Mock<IQueryRepository<FundDetails>>();

            mockRepository.Setup(m => m.GetFirstOrDefaultDataEntity(It.IsAny<ISpecification<FundDetails>>()))
                .Returns(Task.FromResult<FundDetails>(null));

            var controller = CreateController(mockRepository.Object);

            var result = await controller.GetFundByMarketCode("AAA");

            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void GetFundByMarketCode_WithMatch_ReturnsExpectedFund()
        {
            var expectedFund = new FundDetails()
            {
                Id = Guid.NewGuid(),
                Active = true,
                CurrentUnitPrice = 100.2345M,
                MarketCode = "AAA",
                FundManager = "Berty",
                Name = "GuiltIndex"
            };

            var mockRepository = new Mock<IQueryRepository<FundDetails>>();

            mockRepository.Setup(m => m.GetFirstOrDefaultDataEntity(It.IsAny<ISpecification<FundDetails>>()))
                .ReturnsAsync(expectedFund);

            var controller = CreateController(mockRepository.Object);

            var result = await controller.GetFundByMarketCode("AAA");

            Assert.NotNull(result);
            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedFund, objectResult.Value);
        }

        [Fact]
        public async void FundSearch_WithMatches_ReturnsExpectedFunds()
        {
            var expectedFunds = new List<FundDetails>()
            {
                new FundDetails() {
                    Id = Guid.NewGuid(),
                    Active = true,
                    CurrentUnitPrice = 100.2345M,
                    MarketCode = "AAA",
                    FundManager = "Berty",
                    Name = "GuiltIndex"
                },
                new FundDetails() {
                    Id = Guid.NewGuid(),
                    Active = true,
                    CurrentUnitPrice = 106.2345M,
                    MarketCode = "BBB",
                    FundManager = "Berty",
                    Name = "GoldIndex"
                },
            };

            var mockRepository = new Mock<IQueryRepository<FundDetails>>();

            mockRepository.Setup(m => m.GetDataEntities(It.IsAny<ISpecification<FundDetails>>()))
                .ReturnsAsync(expectedFunds);

            var controller = CreateController(mockRepository.Object);

            var result = await controller.FundSearch("");

            Assert.NotNull(result);
            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedFunds, objectResult.Value);
        }

        private FundsController CreateController(IQueryRepository<FundDetails> repository)
        {
            var mockLogger = new Mock<ILogger<FundsController>>();
            return new FundsController(mockLogger.Object, repository);
        }
    }
}

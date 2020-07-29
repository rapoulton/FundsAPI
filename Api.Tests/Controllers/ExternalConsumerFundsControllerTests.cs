using Api.Controllers;
using Api.DataModels;
using Api.Repositories;
using Api.Repositories.Specifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace Api.Tests.Controllers
{
    public class ExternalConsumerFundsControllerTests
    {
        [Fact]
        public async void GetExternalConsumerFunds_WithMatches_ReturnsExpectedFundsWithRoundedCurrentUnitPrice()
        {
            var sourceFunds = new List<FundDetails>()
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
                    CurrentUnitPrice = 106.9788M,
                    MarketCode = "BBB",
                    FundManager = "Berty",
                    Name = "GoldIndex"
                },
            };

            var mockRepository = new Mock<IQueryRepository<FundDetails>>();

            mockRepository.Setup(m => m.GetDataEntities(It.IsAny<ISpecification<FundDetails>>()))
                .ReturnsAsync(sourceFunds);

            var controller = CreateController(mockRepository.Object);

            var result = await controller.GetExternalConsumerFunds();

            Assert.NotNull(result);
            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Collection((IEnumerable<Dtos.ExternalConsumerFund>)objectResult.Value,
                fund1 => Assert.Equal(100.23M, fund1.CurrentUnitPrice),
                fund2 => Assert.Equal(106.98M, fund2.CurrentUnitPrice)
            );
        }

        private ExternalConsumerFundsController CreateController(IQueryRepository<FundDetails> repository)
        {
            var mockLogger = new Mock<ILogger<ExternalConsumerFundsController>>();
            return new ExternalConsumerFundsController(mockLogger.Object, repository);
        }
    }
}

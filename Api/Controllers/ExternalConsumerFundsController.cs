using Api.DataModels;
using Api.Repositories;
using Api.Repositories.Specifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("externalconsumer/funds")]
    public class ExternalConsumerFundsController : Controller
    {
        private readonly ILogger<ExternalConsumerFundsController> _logger;
        private readonly IQueryRepository<FundDetails> _queryRepository;

        public ExternalConsumerFundsController(ILogger<ExternalConsumerFundsController> logger, IQueryRepository<FundDetails> queryRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _queryRepository = queryRepository ?? throw new ArgumentNullException(nameof(queryRepository));
        }
 
        [HttpGet()]
        public async Task<IActionResult> GetExternalConsumerFunds()
        {
            _logger.LogInformation($"GetExternalConsumerFunds has been started...");

            var funds = await _queryRepository.GetDataEntities(new FundsSearchSpecification());

            _logger.LogInformation($"GetExternalConsumerFunds has completed.");

            // Use AutoMapper (ran out of time)
            return Ok(funds.Select(m => new Dtos.ExternalConsumerFund()
                {
                    Active = m.Active,
                    Name = m.Name,
                    Code = m.MarketCode,
                    CurrentUnitPrice = Math.Round(m.CurrentUnitPrice, 2),
                    FundManager  = m.FundManager
                })
            );
        }
    }
}
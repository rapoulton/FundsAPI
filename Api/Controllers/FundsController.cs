using Api.DataModels;
using Api.Repositories;
using Api.Repositories.Specifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("funds")]
    public class FundsController : Controller
    {
        private readonly ILogger<FundsController> _logger;
        private readonly IQueryRepository<FundDetails> _queryRepository;

        public FundsController(ILogger<FundsController> logger, IQueryRepository<FundDetails> queryRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _queryRepository = queryRepository ?? throw new ArgumentNullException(nameof(queryRepository));
        }

        [HttpGet("{marketCode}")]
        public async Task<IActionResult> GetFundByMarketCode(string marketCode)
        {
            _logger.LogInformation($"GetFundByMarketCode ({marketCode}) has been started...");

            if (string.IsNullOrEmpty(marketCode))
            {
                return BadRequest();
            }

            var fund = await _queryRepository.GetFirstOrDefaultDataEntity(new FundsByMarketCodeSpecification(marketCode));
            
            if (fund == null)
            {
                _logger.LogInformation($"GetFundByMarketCode ({marketCode}) found no matches.");
                return NotFound();
            }


            _logger.LogInformation($"GetFundByMarketCode ({marketCode}) has completed.");
            return Ok(fund);
        }

        [HttpGet()]
        public async Task<IActionResult> FundSearch(string fundManager)
        {
            _logger.LogInformation($"GetFundByMarketCode ({fundManager}) has been started...");

            var funds = await _queryRepository.GetDataEntities(new FundsSearchSpecification(fundManager));

            _logger.LogInformation($"GetFundByMarketCode ({fundManager}) has completed.");

            return Ok(funds);
        }
    }
}
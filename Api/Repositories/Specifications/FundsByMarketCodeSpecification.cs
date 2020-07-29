using Api.DataModels;
using System;
using System.Linq;

namespace Api.Repositories.Specifications
{
    public class FundsByMarketCodeSpecification : ISpecification<FundDetails>
    {
        private readonly string _marketCode;

        public FundsByMarketCodeSpecification(string marketCode)
        {
            if (string.IsNullOrEmpty(marketCode))
            {
                throw new ArgumentException("message", nameof(marketCode));
            }

            _marketCode = marketCode;
        }

        public IQueryable<FundDetails> ApplySpecification(IQueryable<FundDetails> baseQuery)
        {
            return baseQuery.Where(m => m.MarketCode == _marketCode);
        }
    }
}

using Api.DataModels;
using System.Linq;

namespace Api.Repositories.Specifications
{
    public class FundsSearchSpecification : ISpecification<FundDetails>
    {
        private readonly string _fundManager;

        public FundsSearchSpecification(string fundManager = null)
        {
            _fundManager = fundManager;
        }

        public IQueryable<FundDetails> ApplySpecification(IQueryable<FundDetails> baseQuery)
        {
            if (string.IsNullOrEmpty(_fundManager))
            {
                return baseQuery;
            }

            return baseQuery.Where(m => m.FundManager == _fundManager);
        }
    }
}

using Api.DataSources;
using Api.Repositories.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Repositories
{
    public class QueryRepository<TModel> : IQueryRepository<TModel> 
        where TModel : class
    {
        private readonly IDataSource<TModel> _dataSource;

        public QueryRepository(IDataSource<TModel> dataSource)
        {
            _dataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));
        }

        public async Task<IEnumerable<TModel>> GetDataEntities(ISpecification<TModel> specification)
        {
            if (specification is null)
            {
                throw new ArgumentNullException(nameof(specification));
            }

            var baseQuery = await _dataSource.GetIQueryable();

            return specification.ApplySpecification(baseQuery)
                .ToList();
        }

        public async Task<TModel> GetFirstOrDefaultDataEntity(ISpecification<TModel> specification)
        {
            if (specification is null)
            {
                throw new ArgumentNullException(nameof(specification));
            }

            var baseQuery = await _dataSource.GetIQueryable();

            return specification.ApplySpecification(baseQuery)
                .FirstOrDefault();
        }
    }
}

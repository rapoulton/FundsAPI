using Api.DataModels;
using Api.Repositories.Specifications;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Repositories
{
    public interface IQueryRepository<TModel> 
        where TModel : class
    {
        Task<TModel> GetFirstOrDefaultDataEntity(ISpecification<TModel> specification);
        Task<IEnumerable<TModel>> GetDataEntities(ISpecification<TModel> specification);
    }
}

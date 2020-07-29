using System.Linq;

namespace Api.Repositories.Specifications
{
    public interface ISpecification<TModel> 
        where TModel : class
    {
        IQueryable<TModel> ApplySpecification(IQueryable<TModel> baseQuery);
    }
}

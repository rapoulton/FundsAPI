using System.Linq;
using System.Threading.Tasks;

namespace Api.DataSources
{
    public interface IDataSource<TModel>
        where TModel : class
    {
        Task<IQueryable<TModel>> GetIQueryable();
    }
}

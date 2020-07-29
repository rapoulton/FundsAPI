using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Api.DataSources
{
    public class DataFileDataSource<TModel> : IDataSource<TModel>
        where TModel : class
    {
        private readonly string _dataFilePath;

        public DataFileDataSource(string dataFilePath)
        {
            if (string.IsNullOrWhiteSpace(dataFilePath))
            {
                throw new ArgumentException("Invalid data file path.", nameof(dataFilePath));
            }

            _dataFilePath = dataFilePath;
        }

        public async Task<IQueryable<TModel>> GetIQueryable()
        {
            var file = await File.ReadAllTextAsync(_dataFilePath);
            return JsonConvert.DeserializeObject<List<TModel>>(file).AsQueryable();
        }
    }
}

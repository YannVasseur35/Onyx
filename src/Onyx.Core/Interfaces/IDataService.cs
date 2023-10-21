using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onyx.Core.Interfaces
{
    public interface IDataService<T>
    {
        Task<T?> GetAsync(string id);

        Task<IEnumerable<T>?> GetAllAsync();

        Task AddOrUpdateAsync(T entity);

        Task DeleteAsync(string id);
    }
}
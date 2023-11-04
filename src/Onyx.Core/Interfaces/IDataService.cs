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
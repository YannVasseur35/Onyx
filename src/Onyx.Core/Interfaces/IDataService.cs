namespace Onyx.Core.Interfaces
{
    public interface IDataService<T>
    {
        Task<T?> GetAsync(Guid id);

        Task<IEnumerable<T>?> GetAllAsync();

        Task<Guid> AddOrUpdateAsync(T entity);

        Task DeleteAsync(Guid id);
    }
}
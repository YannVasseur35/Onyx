namespace Onyx.Application.Interfaces
{
    public interface IAppServices<T>
    {
        public Task<IEnumerable<T>?> GetAllAsync();

        public Task<OperationResult<T?>> GetByIdAsync(Guid guid);

        public Task<OperationResult<Guid>> SaveAsync(T dto);

        public Task<Operation> DeleteAsync(Guid guid);
    }
}
public interface IService<T>
{
    public Task<bool> DeleteItemByID(Guid id);
    public Task<T?> InsertAsync(T item);
    public Task<T?> GetByIdAsync(Guid id);
    public Task<IEnumerable<T>> GetAllAsync();
}
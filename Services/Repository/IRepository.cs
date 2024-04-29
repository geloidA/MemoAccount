namespace MemoAccount.Services.Repository;

public interface IRepository<T, in TKey> : IDisposable
    where T : class
{
    IAsyncEnumerable<T> GetItemsAsync();
    Task<ActionResult<T>> GetItemAsync(TKey key);
    Task<ActionResult<T>> CreateAsync(T item);
    Task<ActionResult<T>> UpdateAsync(T item);
    Task<ActionResult<T>> DeleteAsync(T item);
}
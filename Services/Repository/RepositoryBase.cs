namespace MemoAccount.Services.Repository;

public abstract class RepositoryBase<T, TKey> : IRepository<T, TKey>
    where T : class
{
    public abstract void Dispose();

    public abstract IAsyncEnumerable<T> GetItemsAsync();

    protected abstract TKey KeySelector(T item);

    public abstract Task<ActionResult<T>> GetItemAsync(TKey key);

    public abstract Task<ActionResult<T>> CreateAsync(T item);

    public abstract Task<ActionResult<T>> UpdateAsync(T item);

    public abstract Task<ActionResult<T>> DeleteAsync(T item);

    protected static ActionResult<T> NotFound() => new(default, ActionStatus.NotFound);
    protected static ActionResult<T> Success(T result) => new(result, ActionStatus.Success);
    protected static ActionResult<T> Error(string msg) => new(default, ActionStatus.Error, msg);
}
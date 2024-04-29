namespace MemoAccount.Models;

public class EntityBase<TId>
{
    public TId Id { get; init; } = default!;
}
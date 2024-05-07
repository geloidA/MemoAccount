namespace MemoAccount.Models;

/// <summary>
/// Model for <see cref="Services.Data.Dtos.DivisionDto"/>
/// </summary>
public class Division : EntityBase<int>
{
    public string Name { get; set; } = null!;
    public Department Department { get; set; } = null!;

    public override bool Equals(object? obj)
    {
        return obj is Division division && Equals(division);
    }

    protected bool Equals(Division other)
    {
        return Id == other.Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
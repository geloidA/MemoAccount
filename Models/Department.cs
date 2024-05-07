namespace MemoAccount.Models;

/// <summary>
/// Model for <see cref="Services.Data.Dtos.DepartmentDto"/>
/// </summary>
public class Department : EntityBase<int>
{
    public string Name { get; set; } = null!;

    public override bool Equals(object? obj)
    {
        return obj is Department department && Equals(department);
    }

    protected bool Equals(Department other)
    {
        return Id == other.Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
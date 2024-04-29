namespace MemoAccount.Models;

/// <summary>
/// Model for <see cref="Services.Data.Dtos.DepartmentDto"/>
/// </summary>
public class Department : EntityBase<int>
{
    public string Name { get; set; } = null!;
}
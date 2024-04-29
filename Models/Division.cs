namespace MemoAccount.Models;

/// <summary>
/// Model for <see cref="Services.Data.Dtos.DivisionDto"/>
/// </summary>
public class Division : EntityBase<int>
{
    public string Name { get; set; } = null!;
    public Department Department { get; set; } = null!;
}
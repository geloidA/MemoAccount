namespace MemoAccount.Models;

/// <summary>
/// Model for <see cref="Services.Data.Dtos.MemoDto"/>
/// </summary>
public class Memo : EntityBase<int>
{
    public MemoStatus Status { get; set; }
    public string Content { get; set;} = null!;
    public DateTime CreatedDate { get; set; }
    public DateTime? CompletionDate { get; set; }
    public string? ItemsWithdrawn { get; set; }
    public Department Department { get; set; } = null!;

    public Division? Division { get; set; }
    public User? User { get; set; }

    public string Number
    {
        get
        {
            var divisionPart = Division?.Name != null ? $"/{TakeNumberPart(Division.Name)}" : "";
            return $"{TakeNumberPart(Department.Name)}{divisionPart}/{Id}";
        }
    }

    public string ApplicantString => string.Join(", ", new List<string?> { Department?.Name, Division?.Name }
                .Where(x => x != null));

    private static string TakeNumberPart(string departmentName)
    {
        return departmentName[(departmentName.IndexOf('-') + 1)..];
    }
}
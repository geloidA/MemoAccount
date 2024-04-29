namespace MemoAccount.Models;

/// <summary>
/// Model for <see cref="Services.Data.Dtos.MemoDto"/>
/// </summary>
public class Memo : EntityBase<int>
{
    public MemoStatus Status { get; set; }
    public Applicant Applicant { get; set; } = null!;
    public string Content { get; set;} = null!;
    public DateTime CreatedDate { get; set; }
    public DateTime? CompletionDate { get; set; }
    public string? ItemsWithdrawn { get; set; }

    public string Number
    {
        get
        {
            var divisionPart = Applicant.Division != null ? $"/{Applicant.Division.Name}" : "";
            return $"{Applicant.Department.Name}{divisionPart}/{Id}";
        }
    }
}
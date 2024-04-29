namespace MemoAccount.Models;

public class Applicant : EntityBase<int>
{
    public Department Department { get; set; } = null!;
    public Division? Division { get; set; }
}
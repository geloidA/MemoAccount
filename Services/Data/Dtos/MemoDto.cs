using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MemoAccount.Services.Data.Dtos;

[Table("Memos")]
public class MemoDto
{
    [DatabaseGenerated(DatabaseGeneratedOption.None), Key]
    public int Id { get; set; }
    public int Status { get; set; }
    [MaxLength(1000)] public required string Content { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? CompletionDate { get; set; }
    [MaxLength(int.MaxValue)] public string? ItemsWithdrawn { get; set; }

    [ForeignKey("DepartmentId")] public int DepartmentId { get; set; }
    public virtual required DepartmentDto Department { get; set; }

    [ForeignKey("DivisionId")] public int? DivisionId { get; set; }
    public virtual DivisionDto? Division { get; set; }

    [ForeignKey("UserId")] public int? UserId { get; set; }
    public virtual UserDto? User { get; set; }
}
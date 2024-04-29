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

    [ForeignKey("ApplicantId")] public int ApplicantId { get; set; }
    public virtual required ApplicantDto Applicant { get; set; }
}
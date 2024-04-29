using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MemoAccount.Services.Data.Dtos;

[Table("Applicants")]
public class ApplicantDto
{
    [Key] public int Id { get; set; }

    [ForeignKey("DepartmentId")] public int DepartmentId { get; set; }
    public virtual required DepartmentDto Department { get; set; }

    [ForeignKey("DivisionId")] public int? DivisionId { get; set; }
    public virtual DivisionDto? Division { get; set; }
}
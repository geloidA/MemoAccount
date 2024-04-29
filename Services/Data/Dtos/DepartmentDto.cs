using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MemoAccount.Services.Data.Dtos;

[Table("Departments")]
public class DepartmentDto
{
    [Key] public int Id { get; set; }
    [MaxLength(100)] public required string Name { get; set; }

    public virtual List<DivisionDto>? Divisions { get; set; }
    public virtual List<ApplicantDto>? Applicants { get; set; }
}
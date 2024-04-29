using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MemoAccount.Services.Data.Dtos;

[Table("Divisions")]
public class DivisionDto
{
    [Key] public int Id { get; set; }
    [MaxLength(100)] public required string Name { get; set; }

    [ForeignKey("DepartmentId")] public int DepartmentId { get; set; }
    public virtual required DepartmentDto Department { get; set; }
}

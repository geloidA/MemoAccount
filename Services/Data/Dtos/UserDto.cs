using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MemoAccount.Services.Data.Dtos;

[Table("Users")]
public class UserDto
{
    [Key] public int Id { get; set; }
    [MaxLength(200)] public required string PasswordHash { get; set; }
    [MaxLength(100)] public required string Login { get; set; }
    [MaxLength(100)] public required string FirstName { get; set; }
    [MaxLength(100)] public required string LastName { get; set;}

    public virtual List<MemoDto>? Memos { get; set; }
}
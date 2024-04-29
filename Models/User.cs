namespace MemoAccount.Models;

/// <summary>
/// Model for <see cref="Services.Data.Dtos.UserDto"/>
/// </summary>
public class User : EntityBase<int>
{
    public string Login { get; set; } = "";
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string PasswordHash { get; set; } = "";
    public PermissionType Type { get; set; }
}
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

    public string DisplayName => $"{FirstName} {LastName}";

    public override bool Equals(object? obj)
    {
        return obj is User user && Equals(user);
    }

    protected bool Equals(User other)
    {
        return Id == other.Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
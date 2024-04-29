using MemoAccount.Models;
using MemoAccount.Services.Repository;

namespace MemoAccount.Services.Auth;

public interface IAuthService
{
    void Logout();
    Task<ActionResult<User>> Login(LoginDto user);
    Task<ActionResult> Register(RegistrationDto user);
}

public class RegistrationDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Login { get; set; }
    public string? Password { get; set; }
}

public class LoginDto
{
    public string? Login { get; set; }
    public string? Password { get; set; }
}
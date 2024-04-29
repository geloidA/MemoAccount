using MemoAccount.Models;

namespace MemoAccount.Services.Auth;

public class AuthenticationStateProvider : IAuthenticationStateProvider
{
    public User? AuthorizedUser { get; private set; }

    public event Action? AuthenticationStateChanged;

    public void Login(User user)
    {
        AuthorizedUser = user;
        AuthenticationStateChanged?.Invoke();
    }

    public void Logout()
    {
        AuthorizedUser = null;
        AuthenticationStateChanged?.Invoke();
    }
}
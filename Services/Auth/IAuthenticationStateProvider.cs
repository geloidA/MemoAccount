using MemoAccount.Models;

namespace MemoAccount.Services.Auth;

public interface IAuthenticationStateProvider
{
    User? AuthorizedUser { get; }
    event Action? AuthenticationStateChanged;
}
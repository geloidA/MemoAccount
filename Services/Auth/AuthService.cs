using System.Security.Cryptography;
using MemoAccount.Models;
using MemoAccount.Services.Repository;
using System.Text;

namespace MemoAccount.Services.Auth;

/// <summary>
/// Сервис аутентификации. Осуществляет логин и логаут пользователей,
/// а также проверку их данных на валидность.
/// </summary>
public class AuthService(IRepository<User, int> userRepository, AuthenticationStateProvider authenticationStateProvider) : IAuthService
{
    public void Logout() => authenticationStateProvider.Logout();

    public async Task<ActionResult<User>> Login(LoginDto user)
    {
        ArgumentNullException.ThrowIfNull(user, nameof(user));

        var repositoryUser = await userRepository
            .GetItemsAsync()
            .FirstOrDefaultAsync(x => user.Login == x.Login);

        if (repositoryUser == null) return ActionResult<User>.Error("Неверный логин");

        var passwordBytes = SHA256.HashData(Encoding.UTF8.GetBytes(user.Password!));

        var passwordHash = BitConverter.ToString(passwordBytes);

        if (repositoryUser.PasswordHash != passwordHash) return ActionResult<User>.Error("Неверный пароль");

        authenticationStateProvider.Login(repositoryUser);
        return ActionResult<User>.Success(repositoryUser);

    }

    public async Task<ActionResult> Register(RegistrationDto user)
    {
        ArgumentNullException.ThrowIfNull(user);

        var registerResult = await userRepository.CreateAsync(new User
        {
            FirstName = user.FirstName!,
            LastName = user.LastName!,
            Login = user.Login!,
            PasswordHash = BitConverter.ToString(SHA256.HashData(Encoding.UTF8.GetBytes(user.Password!)))
        });

        return registerResult.Status == ActionStatus.Success ? ActionResult.Success : ActionResult.Error(registerResult.ErrorMessage);
    }
}
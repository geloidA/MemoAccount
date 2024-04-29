using System.ComponentModel.DataAnnotations;
using MemoAccount.Models;
using MemoAccount.Services.Auth;
using MemoAccount.Services.Repository;
using MemoAccount.Views.Pages;
using Wpf.Ui;

namespace MemoAccount.ViewModels.Pages;

public partial class LoginViewModel(INavigationService navigationService, IAuthService authService) : ObservableValidator
{
    private string? _login;

    [Required(ErrorMessage = "Введите логин")]
    public string? Login
    {
        get => _login;
        set => SetProperty(ref _login, value, true);
    }

    private string? _password;

    [Required(ErrorMessage = "Введите пароль")]
    public string? Password
    {
        get => _password;
        set => SetProperty(ref _password, value, true);
    }

    [RelayCommand]
    private Task<ActionResult<User>> LoginAsync() => authService.Login(new LoginDto { Login = Login, Password = Password });

    private bool CanLogin() => !string.IsNullOrEmpty(_login) && !string.IsNullOrWhiteSpace(_password);

    [RelayCommand]
    private void OpenRegisterPage()
    {
        navigationService.Navigate(typeof(RegistrationPage));
    }
}
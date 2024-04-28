using System.ComponentModel.DataAnnotations;
using MemoAccount.Views.Pages;
using Wpf.Ui;

namespace MemoAccount.ViewModels.Pages;

public partial class LoginViewModel(INavigationWindow navigationWindow) : ObservableValidator
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
    private Task LoginAsync()
    {
        return Task.CompletedTask;
    }

    private bool CanLogin() => !string.IsNullOrEmpty(_login) && !string.IsNullOrWhiteSpace(_password);

    [RelayCommand]
    private void OpenRegisterPage()
    {
        navigationWindow.Navigate(typeof(RegistrationPage));
    }
}
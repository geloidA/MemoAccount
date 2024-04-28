using MemoAccount.Views.Pages;
using Wpf.Ui;

namespace MemoAccount.ViewModels.Pages;

public partial class RegistrationViewModel(INavigationWindow navigationWindow) : ObservableObject
{
    [ObservableProperty] private string? _firstName;
    [ObservableProperty] private string? _lastName;
    [ObservableProperty] private string? _login;
    [ObservableProperty] private string? _password;
    [ObservableProperty] private string? _passwordSuggest;

    [RelayCommand]
    private Task Register()
    {
        navigationWindow.Navigate(typeof(LoginPage));
        return Task.CompletedTask;
    }
}
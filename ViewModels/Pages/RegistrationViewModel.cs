using MemoAccount.Services.Auth;
using MemoAccount.Services.Repository;
using MemoAccount.Views.Pages;
using Wpf.Ui;

namespace MemoAccount.ViewModels.Pages;

public partial class RegistrationViewModel(INavigationService navigationService, IAuthService authService) : ObservableObject
{
    [ObservableProperty] private string? _firstName;
    [ObservableProperty] private string? _lastName;
    [ObservableProperty] private string? _login;
    [ObservableProperty] private string? _password;
    [ObservableProperty] private string? _passwordSuggest;

    [RelayCommand]
    private async Task Register()
    {
        var result = await authService.Register(new RegistrationDto
        {
            FirstName = FirstName,
            LastName = LastName,
            Login = Login,
            Password = Password
        });

        if (result.Status == ActionStatus.Success)
        {
            navigationService.Navigate(typeof(LoginPage));
        }
        else
        {
            MessageBox.Show(result.ErrorMessage);
        }
    }
}
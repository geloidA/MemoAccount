using System.ComponentModel.DataAnnotations;
using MemoAccount.Services.Auth;
using MemoAccount.Services.Repository;
using MemoAccount.Views.Pages;
using Wpf.Ui;
using MessageBox = Wpf.Ui.Controls.MessageBox;

namespace MemoAccount.ViewModels.Pages;

public partial class LoginViewModel(INavigationService navigationService, IAuthService authService) : ObservableValidator
{
    [ObservableProperty]
    [Required(ErrorMessage = "Введите логин")] 
    private string? _login;

    [ObservableProperty]
    [Required(ErrorMessage = "Введите пароль")]
    private string? _password;

    [RelayCommand]
    private async Task LoginAsync()
    {
        ValidateAllProperties();
        if (HasErrors)
        {
            await new MessageBox
            {
                Title = "Ошибка валидации",
                Content = GetErrors()
                    .Select(x => x.ErrorMessage)
                    .Aggregate((x, y) => $"{x}\n{y}") ?? "",
                CloseButtonText = "OK"
            }.ShowDialogAsync();
        }
        else
        {
            var res = await authService.Login(new LoginDto { Login = Login, Password = Password });
            if (res.Status == ActionStatus.Success)
            {
                navigationService.Navigate(typeof(MemoPage));
            }
            else
            {
                await new MessageBox
                {
                    Title = "Ошибка входа",
                    Content = res.ErrorMessage ?? "Непредвиденная ошибка",
                    CloseButtonText = "OK"
                }.ShowDialogAsync();
            }
        }
    }

    private bool CanLogin() => !string.IsNullOrEmpty(Login) && !string.IsNullOrWhiteSpace(Password);

    [RelayCommand]
    private void OpenRegisterPage()
    {
        navigationService.Navigate(typeof(RegistrationPage));
    }
}
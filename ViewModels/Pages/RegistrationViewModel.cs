using System.ComponentModel.DataAnnotations;
using MemoAccount.Services.Auth;
using MemoAccount.Services.Repository;
using MemoAccount.Views.Pages;
using Wpf.Ui;
using MessageBox = Wpf.Ui.Controls.MessageBox;

namespace MemoAccount.ViewModels.Pages;

public partial class RegistrationViewModel(INavigationService navigationService, IAuthService authService) : ObservableValidator
{
    [ObservableProperty]
    [Required(ErrorMessage = "Введите имя")]
    private string? _firstName;

    [ObservableProperty]
    [Required(ErrorMessage = "Введите фамилию")]
    private string? _lastName;

    [ObservableProperty]
    [Required(ErrorMessage = "Введите логин")]
    private string? _login;

    [ObservableProperty]
    [Required(ErrorMessage = "Введите пароль")]
    private string? _password;

    [ObservableProperty]
    [CustomValidation(typeof(RegistrationViewModel), nameof(ShouldEqualPassword))]
    private string? _passwordSuggest;

    public static ValidationResult? ShouldEqualPassword(string passwordSuggest, ValidationContext context)
    {
        var instance = (RegistrationViewModel)context.ObjectInstance;

        return passwordSuggest == instance.Password ? ValidationResult.Success : new("Пароли должны совпадать");
    }

    [RelayCommand]
    private async Task Register()
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

            return;
        }

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
            await new MessageBox
            {
                Title = "Ошибка валидации",
                Content = result.ErrorMessage ?? "Непредвиденная ошибка",
                CloseButtonText = "OK"
            }.ShowDialogAsync();
        }
    }
}
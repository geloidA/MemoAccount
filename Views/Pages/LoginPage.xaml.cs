using MemoAccount.ViewModels.Pages;
using Wpf.Ui.Controls;

namespace MemoAccount.Views.Pages;

public partial class LoginPage : INavigableView<LoginViewModel>
{
    public LoginViewModel ViewModel { get; }

    public LoginPage(LoginViewModel model)
    {
        ViewModel = model;
        DataContext = this;

        InitializeComponent();
    }
}
using MemoAccount.ViewModels.Pages;
using Wpf.Ui.Controls;

namespace MemoAccount.Views.Pages;

public partial class RegistrationPage : INavigableView<RegistrationViewModel>
{
    public RegistrationViewModel ViewModel { get; }

    public RegistrationPage(RegistrationViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;

        InitializeComponent();
    }
}
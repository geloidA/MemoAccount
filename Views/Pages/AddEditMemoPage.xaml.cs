using MemoAccount.ViewModels.Pages;
using Wpf.Ui.Controls;

namespace MemoAccount.Views.Pages;

/// <summary>
/// Interaction logic for AddEditMemoPage.xaml
/// </summary>
public partial class AddEditMemoPage : INavigableView<AddEditMemoViewMode>
{
    public AddEditMemoPage(AddEditMemoViewMode viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;

        InitializeComponent();
    }

    public AddEditMemoViewMode ViewModel { get; }
}
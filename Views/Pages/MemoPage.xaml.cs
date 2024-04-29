using MemoAccount.ViewModels.Pages;
using Wpf.Ui.Controls;

namespace MemoAccount.Views.Pages;

/// <summary>
/// Interaction logic for MemoPage.xaml
/// </summary>
public partial class MemoPage : INavigableView<MemoViewModel>
{
    public MemoViewModel ViewModel { get; }

    public MemoPage(MemoViewModel model)
    {
        ViewModel = model;
        DataContext = this;

        InitializeComponent();
    }
}
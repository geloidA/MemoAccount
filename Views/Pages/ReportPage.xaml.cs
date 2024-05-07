using MemoAccount.ViewModels.Pages;
using Wpf.Ui.Controls;

namespace MemoAccount.Views.Pages;

public partial class ReportPage : INavigableView<ReportViewModel>
{
    public ReportPage(ReportViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;

        InitializeComponent();
    }

    public ReportViewModel ViewModel { get; }
}
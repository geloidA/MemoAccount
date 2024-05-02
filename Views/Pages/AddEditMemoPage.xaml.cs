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

    private async void DepartmentChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
    {
        await ViewModel.DepartmentChosen((string)args.SelectedItem);
    }

    private void OnDepartmentSuggestBoxOnTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        if (!string.IsNullOrWhiteSpace(args.Text)) return;

        ViewModel.Department = null;
        ViewModel.Divisions = [];
    }

    private void DivisionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
    {
        ViewModel.Division = ViewModel.Divisions.First(x => x.Name == (string)args.SelectedItem);
    }

    private void OnDivisionSuggestBoxOnTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        if (!string.IsNullOrWhiteSpace(args.Text)) return;

        ViewModel.Division = null;
    }
}
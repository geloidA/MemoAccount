using MemoAccount.ViewModels.Pages;
using Wpf.Ui.Controls;

namespace MemoAccount.Views.Pages;

/// <summary>
/// страница создания/редактирования мемуарной книжки
/// </summary>
public partial class AddEditMemoPage : INavigableView<AddEditMemoViewMode>
{
    /// <summary>
    /// конструктор
    /// </summary>
    /// <param name="viewModel">модель представления</param>
    public AddEditMemoPage(AddEditMemoViewMode viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;

        InitializeComponent();
    }

    /// <summary>
    /// модель представления
    /// </summary>
    public AddEditMemoViewMode ViewModel { get; }
}

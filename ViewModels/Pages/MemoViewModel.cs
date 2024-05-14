using System.Collections.ObjectModel;
using MemoAccount.Models;
using MemoAccount.Services.Auth;
using MemoAccount.Services.Repository;
using MemoAccount.Views.Pages;
using Wpf.Ui;
using Wpf.Ui.Controls;
using Wpf.Ui.Extensions;

namespace MemoAccount.ViewModels.Pages;


/// <summary>
/// ViewModel для страницы "Служебные записки", отвечающий за логику работы с памятниками.
/// Содержит список всех записок, позволяет их создавать, изменять, удалять,
/// а также поиск и фильтрацию по ним.
/// </summary>
public partial class MemoViewModel : ObservableObject
{
    private readonly IRepository<Division, int> _divisionRepository;
    private readonly IAuthenticationStateProvider _authenticationStateProvider;
    private readonly IRepository<Memo, int> _memoRepository;
    private readonly INavigationService _navigationService;
    private readonly IContentDialogService _contentDialogService;
    private readonly IPageService _pageService;

    [ObservableProperty] private ObservableCollection<Memo> _memos = [];
    [ObservableProperty] private bool _onlyClosed;
    [ObservableProperty] private bool _onlyOpened;
    [ObservableProperty] private string _searchText = "";
    [ObservableProperty] private Memo? _selectedMemo;
    [ObservableProperty] private string? _itemsWithdrawn;

    public MemoViewModel(
        IRepository<Memo, int> memoRepository, 
        INavigationService navigationService, 
        IContentDialogService contentDialogService,
        IRepository<Division, int> divisionRepository,
        IAuthenticationStateProvider authenticationStateProvider,
        IPageService pageService)
    {
        _memoRepository = memoRepository;
        _navigationService = navigationService;
        _contentDialogService = contentDialogService;
        _pageService = pageService;
        _divisionRepository = divisionRepository;
        _authenticationStateProvider = authenticationStateProvider;

        PropertyChanged += OnSearchTextChanged;

        _ = InitializeMemosAsync();
    }

    private async void OnSearchTextChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(SearchText))
        {
            Memos = new(await AllMemos.ToListAsync());
        }
    }

    private async Task InitializeMemosAsync()
    {
        await foreach (var memo in _memoRepository.GetItemsAsync().OrderBy(x => x.Status == MemoStatus.Closed))
        {
            Memos.Add(memo);
        }
    }

    [RelayCommand]
    private void ShowCreationMemoPage() => _navigationService.Navigate(typeof(AddEditMemoPage));

    [RelayCommand]
    private async Task EditMemo()
    {
        var page = _pageService.GetPage(typeof(AddEditMemoPage)) as AddEditMemoPage ?? throw new InvalidOperationException();

        page.ViewModel.Divisions = new(await _divisionRepository.GetItemsAsync()
            .Where(x => x.Department.Id == SelectedMemo!.Department.Id)
            .ToListAsync());

        page.ViewModel.IsEditMode = true;
        page.ViewModel.Number = SelectedMemo!.Id;
        page.ViewModel.Department = SelectedMemo!.Department;
        page.ViewModel.Division = SelectedMemo.Division;
        page.ViewModel.Content = SelectedMemo.Content;
        page.ViewModel.ItemsWithdrawn = SelectedMemo.ItemsWithdrawn;
        page.ViewModel.CreationDate = SelectedMemo.CreatedDate;

        _navigationService.Navigate(typeof(AddEditMemoPage), page);
    }

    [RelayCommand]
    private async Task OnOnlyClosedToggled()
    {
        if (OnlyClosed)
        {
            OnlyOpened = false;
        }

        Memos = new(await AllMemos.ToListAsync());
    }

    [RelayCommand]
    private async Task OnOnlyOpenedToggled()
    {
        if (OnlyOpened)
        {
            OnlyClosed = false;
        }

        Memos = new(await AllMemos.ToListAsync());
    }

    [RelayCommand]
    private async Task DeleteMemo()
    {
        var result = await _contentDialogService.ShowSimpleDialogAsync(new SimpleContentDialogCreateOptions
        {
            CloseButtonText = "Отмена",
            PrimaryButtonText = "ОК",
            Title = "Удаление служебной записки",
            Content = "Вы действительно хотите удалить записку? Внимание действие необратимо."
        });

        if (result == ContentDialogResult.Primary && SelectedMemo != null)
        {
            await _memoRepository.DeleteAsync(SelectedMemo);
            Memos = new(await AllMemos.ToListAsync());
        }
    }

    [RelayCommand]
    private Task ShowCompletionDialog(object content) => SelectedMemo?.Status == MemoStatus.Closed ? MarkSelectedMemoOpened() : MarkSelectedMemoClosed(content);

    private async Task MarkSelectedMemoClosed(object content)
    {
        SelectedMemo!.Status = MemoStatus.Closed;
        SelectedMemo.CompletionDate = DateTime.Now;
        SelectedMemo.User = _authenticationStateProvider.AuthorizedUser;
        var result = await _memoRepository.UpdateAsync(SelectedMemo);
        if (result.Status == ActionStatus.Success)
        {
            Memos = new(await AllMemos.ToListAsync());
        }
    }

    private async Task MarkSelectedMemoOpened()
    {
        SelectedMemo!.Status = MemoStatus.Open;
        SelectedMemo.CompletionDate = null;
        SelectedMemo.User = null;
        var result = await _memoRepository.UpdateAsync(SelectedMemo);
        if (result.Status == ActionStatus.Success)
        {
            Memos = new(await AllMemos.ToListAsync());
        }
    }

    private IAsyncEnumerable<Memo> AllMemos => _memoRepository.GetItemsAsync()
        .Where(x => x.Number.Contains(SearchText) || x.Content.Contains(SearchText))
        .Where(x => !OnlyClosed || x.Status == MemoStatus.Closed)
        .Where(x => !OnlyOpened || x.Status == MemoStatus.Open)
        .OrderBy(x => x.Status == MemoStatus.Closed);
}

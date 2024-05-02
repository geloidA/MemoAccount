using System.Collections.ObjectModel;
using MemoAccount.Models;
using MemoAccount.Services.Repository;
using MemoAccount.Views.Pages;
using Wpf.Ui;

namespace MemoAccount.ViewModels.Pages;

public partial class MemoViewModel : ObservableObject
{
    private readonly IRepository<Memo, int> _memoRepository;
    private readonly INavigationService _navigationService;
    private readonly IContentDialogService _contentDialogService;
    private readonly IPageService _pageService;

    [ObservableProperty] private ObservableCollection<Memo> _memos = [];
    [ObservableProperty] private bool _onlyClosed;
    [ObservableProperty] private bool _onlyOpened;
    [ObservableProperty] private string _searchText = "";
    [ObservableProperty] private Memo? _selectedMemo;

    public MemoViewModel(
        IRepository<Memo, int> memoRepository, 
        INavigationService navigationService, 
        IContentDialogService contentDialogService,
        IPageService pageService)
    {
        _memoRepository = memoRepository;
        _navigationService = navigationService;
        _contentDialogService = contentDialogService;
        _pageService = pageService;

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
    private Task ShowCompletionDialog() => MarkSelectedMemoOpened();

    private async Task MarkSelectedMemoOpened()
    {
        SelectedMemo!.Status = MemoStatus.Open;
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
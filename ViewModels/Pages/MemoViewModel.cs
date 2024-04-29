using System.Collections.ObjectModel;
using MemoAccount.Models;
using MemoAccount.Services.Repository;

namespace MemoAccount.ViewModels.Pages;

public partial class MemoViewModel : ObservableObject
{
    private readonly IRepository<Memo, int> _memoRepository;
    [ObservableProperty] private ObservableCollection<Memo> _memos = [];

    public MemoViewModel(IRepository<Memo, int> memoRepository)
    {
        _memoRepository = memoRepository;

        _ = InitializeMemosAsync();
    }

    private async Task InitializeMemosAsync()
    {
        await foreach (var memo in _memoRepository.GetItemsAsync())
        {
            Memos.Add(memo);
        }
    }
}
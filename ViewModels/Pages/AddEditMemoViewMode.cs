using System.Collections.ObjectModel;
using MemoAccount.Models;
using MemoAccount.Services.Data.Dtos;
using MemoAccount.Services.Repository;
using MemoAccount.Views.Pages;
using Wpf.Ui;

namespace MemoAccount.ViewModels.Pages;

public partial class AddEditMemoViewMode : ObservableValidator
{
    private readonly INavigationService _navigationService;
    private readonly IRepository<Department, int> _departmentRepository;
    private readonly IRepository<Division, int> _divisionRepository;
    private readonly IRepository<Memo, int> _memoRepository;
    [ObservableProperty] private int _number;
    [ObservableProperty] private string _content = string.Empty;
    [ObservableProperty] private Department? _department;
    [ObservableProperty] private Division? _division;
    [ObservableProperty] private DateTime? _creationDate = DateTime.Now;

    [ObservableProperty] private ObservableCollection<Department> _departments = [];
    [ObservableProperty] private List<string> _departmentNames = [];

    [ObservableProperty] private ObservableCollection<Division> _divisions = [];
    [ObservableProperty] private List<string>? _divisionsNames;

    public AddEditMemoViewMode(INavigationService navigationService, 
        IRepository<Department, int> departmentRepository,
        IRepository<Division, int> divisionRepository,
        IRepository<Memo, int> memoRepository)
    {
        _navigationService = navigationService;
        _departmentRepository = departmentRepository;
        _divisionRepository = divisionRepository;
        _memoRepository = memoRepository;
        _ = InitializeDepartmentsAsync();
    }

    private async Task InitializeDepartmentsAsync()
    {
        Departments = new ObservableCollection<Department>(await _departmentRepository.GetItemsAsync().ToListAsync());
        DepartmentNames = [..Departments.Select(x => x.Name)];
    }

    [RelayCommand]
    private async Task Create()
    {
        var created = new Memo
        {
            Id = Number,
            Content = Content,
            Department = Department!,
            Division = Division,
            CreatedDate = CreationDate!.Value
        };

        var result = await _memoRepository.CreateAsync(created);

        if (result.ErrorMessage == null)
            _navigationService.Navigate(typeof(MemoPage));
    }

    [RelayCommand]
    private void Cancel() => _navigationService.Navigate(typeof(MemoPage));

    public async Task DepartmentChosen(string name)
    {
        Department = Departments.First(x => x.Name == name);
        Divisions = new(await _divisionRepository.GetItemsAsync()
            .Where(x => x.Department.Id == Department.Id)
            .ToListAsync());
        DivisionsNames = [..Divisions.Select(x => x.Name)];
    }
}
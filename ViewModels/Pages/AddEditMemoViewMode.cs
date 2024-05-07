using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using MemoAccount.Models;
using MemoAccount.Services.Repository;
using MemoAccount.Views.Pages;
using Wpf.Ui;
using MessageBox = Wpf.Ui.Controls.MessageBox;

namespace MemoAccount.ViewModels.Pages;

public partial class AddEditMemoViewMode : ObservableValidator
{
    private readonly INavigationService _navigationService;
    private readonly IRepository<Department, int> _departmentRepository;
    private readonly IRepository<Division, int> _divisionRepository;
    private readonly IRepository<Memo, int> _memoRepository;

    [ObservableProperty]
    [Required(ErrorMessage = "Введите номер")]
    private int? _number;

    [ObservableProperty]
    [Required(ErrorMessage = "Введите содержание")]
    private string _content = string.Empty;

    [ObservableProperty] private string? _itemsWithdrawn;

    [ObservableProperty]
    [Required(ErrorMessage = "Выберите отдел")]
    private Department? _department;

    [ObservableProperty] private Division? _division;

    [ObservableProperty] private DateTime? _creationDate = DateTime.Now;

    [ObservableProperty] private ObservableCollection<Department> _departments = [];
    [ObservableProperty] private ObservableCollection<Division> _divisions = [];

    public bool IsEditMode { get; set; }

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
        PropertyChanged += async (_, args) =>
        {
            if (args.PropertyName == nameof(Department))
            {
                await OnDepartmentChosenAsync();
            }
        };
    }

    [RelayCommand]
    private async Task ActionButton()
    {
        ValidateAllProperties();

        if (HasErrors)
        {
            await new MessageBox
            {
                Title = "Ошибка валидации",
                Content = GetErrors()
                    .Select(x => x.ErrorMessage)
                    .Aggregate((x, y) => $"{x}\n{y}") ?? "",
                CloseButtonText = "OK"
            }.ShowDialogAsync();
            return;
        }

        var resultTask = IsEditMode ? Edit() : Create();

        var result = await resultTask;

        if (result.Status == ActionStatus.Error)
        {
            await new MessageBox
            {
                Title = "Ошибка",
                Content = result.ErrorMessage ?? "Непредвиденная ошибка",
                CloseButtonText = "OK"
            }.ShowDialogAsync();
            return;
        }

        _navigationService.Navigate(typeof(MemoPage));
    }

    private Task<ActionResult<Memo>> Edit() => _memoRepository.UpdateAsync(GetMemo());

    private Task<ActionResult<Memo>> Create() => _memoRepository.CreateAsync(GetMemo());

    private Memo GetMemo()
    {
        return new Memo
        {
            Id = Number!.Value,
            Content = Content,
            Department = Department!,
            Division = Division,
            CreatedDate = CreationDate!.Value,
            ItemsWithdrawn = ItemsWithdrawn
        };
    }

    private Task ShowErrorMessage(string? message) => new MessageBox
    {
        Title = "Ошибка",
        CloseButtonText = "Закрыть",
        Content = message
    }.ShowDialogAsync();

    [RelayCommand]
    private void Cancel() => _navigationService.Navigate(typeof(MemoPage));

    public async Task OnDepartmentChosenAsync()
    {
        Divisions = new(await _divisionRepository.GetItemsAsync()
            .Where(x => x.Department.Id == Department!.Id)
            .ToListAsync());
    }
}
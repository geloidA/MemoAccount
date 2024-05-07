using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Forms;
using MemoAccount.Models;
using MemoAccount.Services;
using MemoAccount.Services.Repository;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;
using Application = System.Windows.Application;
using MessageBox = Wpf.Ui.Controls.MessageBox;

namespace MemoAccount.ViewModels.Pages;

public partial class ReportViewModel : ObservableValidator
{
    private readonly IRepository<Memo, int> _memoRepository;
    private readonly IRepository<User, int> _userRepository;
    private readonly Notifier _notifier = new(cfg =>
    {
        cfg.PositionProvider = new WindowPositionProvider(
            parentWindow: Application.Current.MainWindow,
            corner: Corner.BottomRight,
            offsetX: 20,
            offsetY: 20);

        cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
            notificationLifetime: TimeSpan.FromSeconds(3),
            maximumNotificationCount: MaximumNotificationCount.FromCount(5));

        cfg.Dispatcher = Application.Current.Dispatcher;
    });

    [ObservableProperty]
    [CustomValidation(typeof(ReportViewModel), nameof(ShouldLessThanEnd))]
    private DateTime _start;

    [ObservableProperty] private DateTime _end;

    [ObservableProperty] private ObservableCollection<User>? _users;

    [ObservableProperty] private User? _user;

    public static ValidationResult? ShouldLessThanEnd(DateTime start, ValidationContext context)
    {
        var instance = (ReportViewModel)context.ObjectInstance;

        return start <= instance.End ? ValidationResult.Success : new("Начальная дата должна быть меньше конечной");
    }

    public ReportViewModel(IRepository<Memo, int> memoRepository,
        IRepository<User, int> userRepository)
    {
        var now = DateTime.Now;
        Start = new DateTime(now.Year, now.Month, 1);
        End = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month));

        _memoRepository = memoRepository;
        _userRepository = userRepository;

        _ = InitializeUsersAsync();
    }

    private async Task InitializeUsersAsync()
    {
        Users = new(await _userRepository.GetItemsAsync().ToListAsync());
    }

    [RelayCommand]
    private void ClearUser() => User = null;

    [RelayCommand]
    private async Task GenerateReport()
    {
        ValidateAllProperties();

        if (HasErrors)
        {
            await new MessageBox()
            {
                Title = "Ошибка валидации",
                Content = GetErrors().Select(x => x.ErrorMessage).Aggregate((x, y) => $"{x}\n{y}")
            }.ShowDialogAsync();
            return;
        }

        try
        {
            var memos = await _memoRepository.GetItemsAsync()
                .Where(x => User == null || x.User?.Id == User.Id)
                .Where(x => x.CreatedDate >= Start && x.CreatedDate <= End)
                .ToListAsync();

            if (memos.Count == 0)
            {
                await new MessageBox
                {
                    Title = "Отчет не сгенерирован",
                    Content = "По вашему запросу ничего не найдено"
                }.ShowDialogAsync();
                return;
            }

            ReportService.GenerateReport(memos);
            _notifier.ShowSuccess("Отчет сгенерирован");
        }
        catch (Exception e)
        {
            await new MessageBox()
            {
                Title = "Произошла ошибка во время генерации отчета",
                Content = e.Message
            }.ShowDialogAsync();
        }
    }
}
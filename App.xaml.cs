using MemoAccount.Services;
using MemoAccount.ViewModels.Pages;
using MemoAccount.ViewModels.Windows;
using MemoAccount.Views.Pages;
using MemoAccount.Views.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Reflection;
using System.Windows.Threading;
using MemoAccount.Services.Mappers;
using Serilog;
using Wpf.Ui;

namespace MemoAccount;

public partial class App
{
    // регистрация сервисов приложения
    private static readonly IHost _host = Host
        .CreateDefaultBuilder()
        .ConfigureAppConfiguration(c => { c.SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)); })
        .ConfigureServices((_, services) =>
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File(Path.Combine("logs", "log-.txt"), rollingInterval: RollingInterval.Day)
                .CreateLogger();

            services.AddHostedService<ApplicationHostService>();

            // Служба, содержащая навигацию
            services.AddSingleton<IPageService, PageService>();

            // Управление темой приложения
            services.AddSingleton<IThemeService, ThemeService>();

            // TaskBar manipulation
            services.AddSingleton<ITaskBarService, TaskBarService>();

            // Служба, содержащая навигацию, аналогично INavigationWindow... но без окна
            services.AddSingleton<INavigationService, NavigationService>()
                .AddSingleton<IContentDialogService, ContentDialogService>();

            // Основное окно с навигацией
            services.AddSingleton<INavigationWindow, MainWindow>();
            services.AddSingleton<MainWindowViewModel>();

            services.AddAuthentication()
                .AddDomainRepositories();

            // Добавляет мапперы из папочки Services/Mappers текущей сборки
            services.AddAutoMapper(x => x.AddProfile(typeof(MainProfile)));

            services.AddSingleton<SettingsPage>();
            services.AddSingleton<SettingsViewModel>();
            services.AddTransient<ReportViewModel>();
            services.AddTransient<ReportPage>();
            services.AddTransient<MemoPage>();
            services.AddTransient<MemoViewModel>();
            services.AddTransient<AddEditMemoPage>();
            services.AddTransient<AddEditMemoViewMode>();
        }).Build();

    /// <summary>
    /// Возвращает зарегистрированную службу.
    /// </summary>
    /// <typeparam name="T">Тип службы, которую нужно получить.</typeparam>
    /// <returns>Экземпляр службы или <see langword="null"/>.</returns>
    public static T GetService<T>()
        where T : class
    {
        return _host.Services.GetService(typeof(T)) as T;
    }

    /// <summary>
    /// Срабатывает при загрузке приложения.
    /// </summary>
    private void OnStartup(object sender, StartupEventArgs e)
    {
        _host.Start();
    }

    /// <summary>
    /// Срабатывает при закрытии приложения.
    /// </summary>
    private async void OnExit(object sender, ExitEventArgs e)
    {
        await _host.StopAsync();

        _host.Dispose();
    }

    /// <summary>
    /// Срабатывает при ошибке в приложении.
    /// </summary>
    private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        Log.Error(e.Exception.Message);
    }
}

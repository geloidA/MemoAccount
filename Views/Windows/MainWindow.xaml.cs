using MemoAccount.ViewModels.Windows;
using Wpf.Ui;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace MemoAccount.Views.Windows;

/// <summary>
/// Окно приложения.
/// </summary>
public partial class MainWindow : INavigationWindow
{
    /// <summary>
    /// Модель окна.
    /// </summary>
    public MainWindowViewModel ViewModel { get; }

    /// <summary>
    /// Инициализирует окно.
    /// </summary>
    /// <param name="viewModel">Модель окна.</param>
    /// <param name="pageService">Сервис страниц.</param>
    /// <param name="navigationService">Сервис навигации.</param>
    /// <param name="contentDialogService">Сервис модальных диалогов.</param>
    public MainWindow(
        MainWindowViewModel viewModel,
        IPageService pageService,
        INavigationService navigationService,
        IContentDialogService contentDialogService
    )
    {
        ViewModel = viewModel;
        DataContext = this;

        // Подписываемся на изменение темы оформления
        SystemThemeWatcher.Watch(this);

        InitializeComponent();

        // Устанавливаем сервис страниц
        SetPageService(pageService);

        // Устанавливаем сервис модальных диалогов
        contentDialogService.SetDialogHost(RootContentDialog);

        // Устанавливаем сервис навигации
        navigationService.SetNavigationControl(RootNavigation);
    }

    #region INavigationWindow методы

    /// <summary>
    /// Возвращает контрол навигации.
    /// </summary>
    public INavigationView GetNavigation() => RootNavigation;

    /// <summary>
    /// Перейти на страницу.
    /// </summary>
    /// <param name="pageType">Тип страницы.</param>
    /// <returns>Истина, если переход выполнен успешно.</returns>
    public bool Navigate(Type pageType) => RootNavigation.Navigate(pageType);

    /// <summary>
    /// Устанавливает сервис страниц.
    /// </summary>
    /// <param name="pageService">Сервис страниц.</param>
    public void SetPageService(IPageService pageService) => RootNavigation.SetPageService(pageService);

    /// <summary>
    /// Показать окно.
    /// </summary>
    public void ShowWindow() => Show();

    /// <summary>
    /// Закрыть окно.
    /// </summary>
    public void CloseWindow() => Close();

    #endregion INavigationWindow методы

    /// <summary>
    /// Вызывается при закрытии окна.
    /// </summary>
    /// <param name="e">Аргументы события.</param>
    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);

        // Закрытие окна приводит к завершению работы приложения
        Application.Current.Shutdown();
    }

    /// <summary>
    /// Не реализовано.
    /// </summary>
    INavigationView INavigationWindow.GetNavigation()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Не реализовано.
    /// </summary>
    public void SetServiceProvider(IServiceProvider serviceProvider)
    {
        throw new NotImplementedException();
    }
}
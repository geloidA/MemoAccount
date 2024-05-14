using MemoAccount.Services.Auth;
using System.Collections.ObjectModel;
using MemoAccount.Views.Pages;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace MemoAccount.ViewModels.Windows
{
    public partial class MainWindowViewModel : ObservableObject
    {
        #region комментарии

        /// <summary>
        /// Элементы меню, доступные только для неавторизованных пользователей
        /// </summary>
        private readonly ObservableCollection<object> _unauthorizedFooterItems =
            new()
            {
                new NavigationViewItem
                {
                    Content = "Вход",
                    Icon = new SymbolIcon { Symbol = SymbolRegular.Person16 },
                    TargetPageType = typeof(LoginPage),
                    ToolTip = "Вход в систему"
                }
            };

        /// <summary>
        /// Элементы меню, доступные только для авторизованных пользователей
        /// </summary>
        private readonly ObservableCollection<object> _authorizedMenuItems =
            new()
            {
                new NavigationViewItem
                {
                    Content = "Служебные записки",
                    ToolTip = "Служебные записки",
                    Icon = new SymbolIcon { Symbol = SymbolRegular.AlignSpaceEvenlyVertical20 },
                    TargetPageType = typeof(MemoPage)
                },
                new NavigationViewItem
                {
                    Content = "Отчет",
                    ToolTip = "Отчет",
                    Icon = new SymbolIcon { Symbol = SymbolRegular.DocumentTable16 },
                    TargetPageType = typeof(ReportPage)
                }
            };

        #endregion

        public MainWindowViewModel(
            IAuthenticationStateProvider stateProvider, 
            IAuthService authService, 
            INavigationService navigationService)
        {
            _footerMenuItems = _unauthorizedFooterItems;
            _menuItems = [];

            var authorizedFooterItems = new ObservableCollection<object>
            {
                new NavigationViewItem
                {
                    Content = "Настройки",
                    Icon = new SymbolIcon { Symbol = SymbolRegular.Settings16 },
                    TargetPageType = typeof(SettingsPage),
                    ToolTip = "Настройки"
                },
                new NavigationViewItem
                {
                    Content = "Выход",
                    ToolTip = "Выход из системы",   
                    Icon = new SymbolIcon { Symbol = SymbolRegular.DoorArrowRight16 },
                    Command = new RelayCommand(() =>
                    {
                        navigationService.Navigate(typeof(LoginPage));
                        authService.Logout();
                    })
                }
            };

            stateProvider.AuthenticationStateChanged += () =>
            {
                if (stateProvider.AuthorizedUser == null)
                {
                    FooterMenuItems = _unauthorizedFooterItems;
                    MenuItems = new ObservableCollection<object>();
                }
                else
                {
                    FooterMenuItems = authorizedFooterItems;
                    MenuItems = _authorizedMenuItems;
                }
            };
        }

        /// <summary>
        /// Заголовок окна
        /// </summary>
        [ObservableProperty]
        private string _applicationTitle = "Учет служебных записок";

        /// <summary>
        /// Меню, доступное для неавторизованных пользователей
        /// </summary>
        [ObservableProperty] private ObservableCollection<object> _menuItems;

        /// <summary>
        /// Меню, доступное для авторизованных пользователей
        /// </summary>
        [ObservableProperty] private ObservableCollection<object> _footerMenuItems;

        /// <summary>
        /// Элементы контекстного меню в трее
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<MenuItem> _trayMenuItems = new()
        {
            new MenuItem { Header = "Главная", Tag = "tray_home" }
        };
    }
}
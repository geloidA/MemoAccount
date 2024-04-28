using System.Collections.ObjectModel;
using Wpf.Ui.Controls;

namespace MemoAccount.ViewModels.Windows
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _applicationTitle = "Учет служебных записок";

        [ObservableProperty] private bool _isAuthenticated;

        [ObservableProperty]
        private ObservableCollection<object> _menuItems =
        [
            new NavigationViewItem
            {
                Content = "Все записки",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Grid24 },
                TargetPageType = typeof(Views.Pages.DataPage)
            },
            new NavigationViewItem
            {
                Content = "Просмотр",
                Icon = new SymbolIcon { Symbol = SymbolRegular.AppsListDetail24 },
                TargetPageType = typeof(Views.Pages.DashboardPage)
            }
        ];

        [ObservableProperty]
        private ObservableCollection<object> _footerMenuItems =
        [
            new NavigationViewItem
            {
                Content = "Вход",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Person24 },
                TargetPageType = typeof(Views.Pages.LoginPage)
            },
            new NavigationViewItem
            {
                Content = "Регистрация",
                Visibility = Visibility.Collapsed,
                TargetPageType = typeof(Views.Pages.RegistrationPage)
            },
            new NavigationViewItem
            {
                Content = "Settings",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Settings24 },
                TargetPageType = typeof(Views.Pages.SettingsPage)
            }
        ];

        [ObservableProperty]
        private ObservableCollection<MenuItem> _trayMenuItems = [new MenuItem { Header = "Home", Tag = "tray_home" }];
    }
}

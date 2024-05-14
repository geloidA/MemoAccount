using MemoAccount.Views.Pages;
using MemoAccount.Views.Windows;
using Microsoft.Extensions.Hosting;
using Wpf.Ui;

namespace MemoAccount.Services
{
    /// <summary>
    /// Управляет запуском приложения
    /// </summary>
    public class ApplicationHostService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        private INavigationWindow _navigationWindow;

        public ApplicationHostService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Срабатывает при старте приложения
        /// </summary>
        /// <param name="cancellationToken">Токен отмены.</param>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await HandleActivationAsync();
        }

        /// <summary>
        /// Срабатывает при остановке приложения
        /// </summary>
        /// <param name="cancellationToken">Токен отмены.</param>
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }

        /// <summary>
        /// Создает окно приложения
        /// </summary>
        private async Task HandleActivationAsync()
        {
            if (!Application.Current.Windows.OfType<MainWindow>().Any())
            {
                _navigationWindow = (
                    _serviceProvider.GetService(typeof(INavigationWindow)) as INavigationWindow
                )!;
                _navigationWindow!.ShowWindow();

                _navigationWindow.Navigate(typeof(LoginPage));
            }

            await Task.CompletedTask;
        }
    }
}

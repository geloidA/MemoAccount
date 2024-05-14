using Wpf.Ui;

namespace MemoAccount.Services
{
    /// <summary>
    /// Сервис, предоставляющий страницы для навигации.
    /// </summary>
    public class PageService : IPageService
    {
        /// <summary>
        /// Сервис, который предоставляет экземпляры страниц.
        /// </summary>
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Создает новый экземпляр и прикрепляет <see cref="IServiceProvider"/>.
        /// </summary>
        public PageService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc />
        public T? GetPage<T>()
            where T : class
        {
            if (!typeof(FrameworkElement).IsAssignableFrom(typeof(T)))
                throw new InvalidOperationException("Страница должна быть элементом WPF.");

            return (T?)_serviceProvider.GetService(typeof(T));
        }

        /// <inheritdoc />
        public FrameworkElement? GetPage(Type pageType)
        {
            if (!typeof(FrameworkElement).IsAssignableFrom(pageType))
                throw new InvalidOperationException("Страница должна быть элементом WPF.");

            return _serviceProvider.GetService(pageType) as FrameworkElement;
        }
    }
}


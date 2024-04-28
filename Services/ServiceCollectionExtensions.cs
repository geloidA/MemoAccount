using MemoAccount.ViewModels.Pages;
using MemoAccount.Views.Pages;
using Microsoft.Extensions.DependencyInjection;

namespace MemoAccount.Services;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAuthentication(this IServiceCollection services)
    {
        return services
            .AddSingleton<LoginPage>()
            .AddSingleton<LoginViewModel>()
            .AddSingleton<RegistrationPage>()
            .AddSingleton<RegistrationViewModel>();

    }
}
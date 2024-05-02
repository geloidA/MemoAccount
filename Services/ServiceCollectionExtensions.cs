using MemoAccount.Models;
using MemoAccount.Services.Auth;
using MemoAccount.Services.Data.Repositories;
using MemoAccount.Services.Repository;
using MemoAccount.ViewModels.Pages;
using MemoAccount.Views.Pages;
using Microsoft.Extensions.DependencyInjection;

namespace MemoAccount.Services;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAuthentication(this IServiceCollection services)
    {
        return services
            .AddAuthViews()
            .AddAuthServices();
    }

    public static IServiceCollection AddDomainRepositories(this IServiceCollection services)
    {
        return services
            .AddTransient<IRepository<Memo, int>, MemoRepository>()
            .AddTransient<IRepository<Department, int>, DepartmentRepository>()
            .AddTransient<IRepository<Division, int>, DivisionRepository>();
    }

    private static IServiceCollection AddAuthViews(this IServiceCollection services)
    {
        return services
            .AddTransient<LoginPage>()
            .AddTransient<LoginViewModel>()
            .AddTransient<RegistrationPage>()
            .AddTransient<RegistrationViewModel>();
    }

    private static IServiceCollection AddAuthServices(this IServiceCollection services)
    {
        var authStateProvider = new AuthenticationStateProvider();
        return services
            .AddScoped<IAuthService, AuthService>()
            .AddScoped<IRepository<User, int>, UserRepository>()
            .AddSingleton(authStateProvider)
            .AddSingleton<IAuthenticationStateProvider>(authStateProvider);
    }
}
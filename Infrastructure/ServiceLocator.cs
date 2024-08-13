using Microsoft.Extensions.DependencyInjection;

namespace Dotnet.Experiments.Sql.Infrastructure;

public static class ServiceLocator
{
    private static IServiceProvider _serviceProvider = null!;

    public static void Initialize(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public static T GetService<T>() where T : class
    {
        return (_serviceProvider.GetRequiredService(typeof(T)) as T)!;
    }
}
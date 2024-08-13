using Dotnet.Experiments.Sql.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dotnet.Experiments.Sql.Infrastructure;

public static class ApplicationBuilderExtensions
{
    public static void ApplyMigrationsAndSeedData(this IHost app)
    {
        using var scope = app.Services.CreateScope();

        using var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

        dbContext.Database.Migrate();
        
        scope.ServiceProvider.GetRequiredService<DataSeeder>().SeedData();
    }
}

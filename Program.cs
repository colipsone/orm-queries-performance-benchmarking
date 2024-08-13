using BenchmarkDotNet.Running;
using Dotnet.Experiments.Sql.Benchmarks;
using Dotnet.Experiments.Sql.Dapper;
using Dotnet.Experiments.Sql.EntityFrameworkCore;
using Dotnet.Experiments.Sql.EntityFrameworkCore.TestStrategies;
using Dotnet.Experiments.Sql.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args);
builder.ConfigureServices((hostContext, services) =>
{
    var connectionString = hostContext.Configuration.GetConnectionString("DefaultConnection");

    services.AddDbContext<DatabaseContext>(options =>
    {
        options.UseSqlServer(connectionString);
    });

    services.AddScoped<DataSeeder>();
    services.AddScoped<DapperWithSqlKata>(_ => new DapperWithSqlKata(connectionString!));
    services.AddScoped<EntityFrameworkWithNoTracking>(_ => new EntityFrameworkWithNoTracking(connectionString!));
    services.AddScoped<ClassicEntityFramework>(_ => new ClassicEntityFramework(connectionString!));
    services.AddScoped<EntityFrameworkWithCompiledQuery>(_ => new EntityFrameworkWithCompiledQuery(connectionString!));
});

var app = builder.Build();

app.ApplyMigrationsAndSeedData();

// var dapperWithSqlKata = app.Services.GetRequiredService<DapperWithSqlKata>();
// await dapperWithSqlKata.GetTopRecentOrders(DateTime.Now.AddYears(-2));

// var classicEntityFramework = app.Services.GetRequiredService<ClassicEntityFramework>();
// await classicEntityFramework.GetTopRecentOrders(DateTime.Now.AddYears(-2));

// var entityFrameworkWithNoTracking = app.Services.GetRequiredService<EntityFrameworkWithNoTracking>();
// await entityFrameworkWithNoTracking.GetTopRecentOrders(DateTime.Now.AddYears(-2));

// var entityFrameworkWithCompiled = app.Services.GetRequiredService<EntityFrameworkWithCompiledQuery>();
// await entityFrameworkWithCompiled.GetTopRecentOrders(DateTime.Now.AddYears(-2));

BenchmarkRunner.Run<Benchmarks>();




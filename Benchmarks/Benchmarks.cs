using Dotnet.Experiments.Sql.Dapper;
using Dotnet.Experiments.Sql.EntityFrameworkCore.TestStrategies;
using Microsoft.Extensions.Configuration;

namespace Dotnet.Experiments.Sql.Benchmarks;

using BenchmarkDotNet.Attributes;
using System.Threading.Tasks;

[MemoryDiagnoser]
public class Benchmarks
{
    private DapperWithSqlKata _dapperWithSqlKata = null!;
    private ClassicEntityFramework _classicEntityFramework = null!;
    private EntityFrameworkWithNoTracking _entityFrameworkWithNoTracking = null!;
    private EntityFrameworkWithCompiledQuery _entityFrameworkWithCompiledQuery = null!;
    
    [GlobalSetup]
    public void Setup()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = config.GetConnectionString("DefaultConnection")!;

        _dapperWithSqlKata = new DapperWithSqlKata(connectionString);
        _classicEntityFramework = new ClassicEntityFramework(connectionString);
        _entityFrameworkWithNoTracking = new EntityFrameworkWithNoTracking(connectionString);
        _entityFrameworkWithCompiledQuery = new EntityFrameworkWithCompiledQuery(connectionString);
    }

    [Benchmark]
    public async Task EntityFrameworkWithNoTracking()
    {
        await _entityFrameworkWithNoTracking.GetTopRecentOrders(DateTime.Now.AddYears(-2));
    }

    [Benchmark]
    public async Task EntityFrameworkWithCompiledQuery()
    {
        await _entityFrameworkWithCompiledQuery.GetTopRecentOrders(DateTime.Now.AddYears(-2));
    }

    [Benchmark]
    public async Task DapperWithSqlKata()
    {
        await _dapperWithSqlKata.GetTopRecentOrders(DateTime.Now.AddYears(-2));
    }

    [Benchmark]
    public async Task ClassicEntityFramework()
    {
        await _classicEntityFramework.GetTopRecentOrders(DateTime.Now.AddYears(-2));
    }
}
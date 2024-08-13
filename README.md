# ORM Performance Benchmark: Dapper vs Entity Framework Core

This project provides a benchmark for comparing the query performance of Dapper and Entity Framework Core (EF Core). It focuses solely on data retrieval operations, offering insights into the efficiency of these popular .NET ORMs.

## Features

- Utilizes [SqlKata](https://sqlkata.com/) for efficient and flexible query construction
- Implements four distinct test strategies:
  1. EF Core (Classic approach)
  2. EF Core with No Tracking
  3. EF Core with Compiled Queries
  4. Dapper
- Automatically seeds the database with 100 customers and 1000 orders of fake data

## Prerequisites

To run this benchmarking utility, ensure you have the following installed:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Microsoft SQL Server

## Getting Started

1. Clone the repository

2. Open the project in your preferred IDE (e.g., Visual Studio, VS Code, JetBrains Rider).

3. Locate the `appsettings.json` configuration file (in the root folder) and update the connection string to point to your local SQL Server instance:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=YourDatabaseName;Trusted_Connection=True;"
     }
   }
   ```

4. Build the project in Release mode:
   ```
   dotnet build -c Release
   ```

5. Run the benchmark:
   ```
   dotnet run -c Release
   ```

## Benchmark Results

Here are my results obtained from running this benchmark:

```
BenchmarkDotNet v0.14.0, Windows 11 (10.0.22631.3958/23H2/2023Update/SunValley3)
13th Gen Intel Core i9-13900HX, 1 CPU, 32 logical and 24 physical cores
.NET SDK 8.0.303
  [Host]     : .NET 8.0.7 (8.0.724.31311), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.7 (8.0.724.31311), X64 RyuJIT AVX2


| Method                           | Mean     | Error     | StdDev    | Median   | Gen0     | Gen1    | Allocated |
|--------------------------------- |---------:|----------:|----------:|---------:|---------:|--------:|----------:|
| EntityFrameworkWithNoTracking    | 7.633 ms | 0.1107 ms | 0.0924 ms | 7.648 ms | 187.5000 | 62.5000 |   3.83 MB |
| EntityFrameworkWithCompiledQuery | 6.802 ms | 0.1039 ms | 0.0921 ms | 6.762 ms | 132.8125 |  7.8125 |   2.45 MB |
| DapperWithSqlKata                | 8.857 ms | 1.2543 ms | 3.6190 ms | 9.981 ms | 109.3750 | 46.8750 |   1.99 MB |
| ClassicEntityFramework           | 6.815 ms | 0.0568 ms | 0.0504 ms | 6.800 ms | 125.0000 |       - |   2.45 MB |
```
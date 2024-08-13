# ORM Performance Benchmark: Dapper vs Entity Framework Core

This project provides a benchmark for comparing the query performance of Dapper and Entity Framework Core (EF Core). It focuses solely on data retrieval operations, offering insights into the efficiency of these popular .NET ORMs.

## Features

- Utilizes [SqlKata](https://sqlkata.com/) for efficient and flexible query construction
- Implements four distinct test strategies:
  1. EF Core (Classic approach)
  2. EF Core with No Tracking
  3. EF Core with Compiled Queries
  4. Dapper

## Prerequisites

To run this benchmarking utility, ensure you have the following installed:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Microsoft SQL Server

## Getting Started

1. Clone the repository:

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
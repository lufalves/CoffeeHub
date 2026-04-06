using CoffeeHub.Infrastructure.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace CoffeeHub.Tests.Common;

public sealed class SqliteTestDatabase : IDisposable
{
    private readonly SqliteConnection _connection;

    public CoffeeHubDbContext Context { get; }

    private SqliteTestDatabase(SqliteConnection connection, CoffeeHubDbContext context)
    {
        _connection = connection;
        Context = context;
    }

    public static SqliteTestDatabase Create()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<CoffeeHubDbContext>()
            .UseSqlite(connection)
            .Options;

        var context = new CoffeeHubDbContext(options);
        context.Database.EnsureCreated();

        return new SqliteTestDatabase(connection, context);
    }

    public void Dispose()
    {
        Context.Dispose();
        _connection.Dispose();
    }
}

public sealed class IntegrationDbFixture : IDisposable
{
    public SqliteTestDatabase CreateDatabase()
    {
        return SqliteTestDatabase.Create();
    }

    public void Dispose()
    {
    }
}

[CollectionDefinition("Integration")]
public sealed class IntegrationCollection : ICollectionFixture<IntegrationDbFixture>
{
}

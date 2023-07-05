using Microsoft.EntityFrameworkCore;
using Helpdesk.DataAccess;

namespace Helpdesk.Tests;

public class AppDatabaseContextFixture
{
    private const string ConnectionString =
        @"Server=127.0.0.1;Port=5432;Database=helpdesk_test;User Id=postgres;Password=XXXXXXXXXX;";

    private static readonly object Lock = new();

    private static bool _databaseInitialized;

    public AppDatabaseContextFixture()
    {
        lock (Lock)
        {
            if (!_databaseInitialized)
            {
                using (var context = CreateContext())
                {
                    context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();

                    context.SaveChanges();
                }

                _databaseInitialized = true;
            }
        }
    }

    public AppDatabaseContext CreateContext()
    {
        return new AppDatabaseContext(
            new DbContextOptionsBuilder<AppDatabaseContext>()
            .UseNpgsql(ConnectionString)
            .UseSnakeCaseNamingConvention()
            .Options
        );
    }
}
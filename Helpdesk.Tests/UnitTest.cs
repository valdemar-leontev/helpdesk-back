using Helpdesk.Domain.Models.Admin;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Helpdesk.Tests;

public class UnitTest : IClassFixture<AppDatabaseContextFixture>
{
    private readonly AppDatabaseContextFixture _appDatabaseContextFixture;

    public UnitTest(AppDatabaseContextFixture fixture)
    {
        _appDatabaseContextFixture = fixture;
    }

    [Fact]
    public void AppDatabaseContextIsExistedTest()
    {
        using var context = _appDatabaseContextFixture.CreateContext();
        Assert.NotNull(context);
    }

    [Fact]
    public async Task UsersTest()
    {
        await using var context = _appDatabaseContextFixture.CreateContext();
        var users = await context.Set<UserDataModel>().ToArrayAsync();

        Assert.NotNull(users);
        Assert.NotEmpty(users);
    }
}
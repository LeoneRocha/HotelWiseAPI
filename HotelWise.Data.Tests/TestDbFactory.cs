using HotelWise.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace HotelWise.Data.Tests;

internal static class TestDbFactory
{
    public static (HotelWiseDbContextMysql Context, DbContextOptions<HotelWiseDbContextMysql> Options) Create()
    {
        var options = new DbContextOptionsBuilder<HotelWiseDbContextMysql>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var context = new HotelWiseDbContextMysql(options);
        context.Database.EnsureCreated();
        return (context, options);
    }
}

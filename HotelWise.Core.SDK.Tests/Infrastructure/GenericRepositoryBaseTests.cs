using HotelWise.Core.SDK.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace HotelWise.Core.SDK.Tests.Infrastructure;

public class TestEntity
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class TestDbContext : DbContext
{
    public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
    {
    }

    public DbSet<TestEntity> Entities => Set<TestEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TestEntity>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).ValueGeneratedOnAdd();
        });
    }
}

public class TestRepository : GenericRepositoryBase<TestEntity, TestDbContext>
{
    public TestRepository(TestDbContext context, DbContextOptions<TestDbContext> options)
        : base(context, options)
    {
    }

    public TestDbContext InvokeCreateContext() => CreateContext();
}

public class GenericRepositoryBaseTests
{
    private static (TestRepository Repo, TestDbContext Context, DbContextOptions<TestDbContext> Options) CreateSut()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var context = new TestDbContext(options);
        return (new TestRepository(context, options), context, options);
    }

    [Fact]
    public void Ctor_Should_Throw_When_Context_Is_Null()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var act = () => new TestRepository(null!, options);
        act.Should().Throw<ArgumentNullException>().WithParameterName("context");
    }

    [Fact]
    public async Task Add_Get_Update_Delete_Should_Work()
    {
        var (repo, _, _) = CreateSut();

        var added = await repo.AddAsync(new TestEntity { Name = "A" });
        added.Id.Should().BeGreaterThan(0);

        var byId = await repo.GetByIdAsync(added.Id);
        byId!.Name.Should().Be("A");

        byId.Name = "B";
        await repo.UpdateAsync(byId);
        (await repo.GetByIdAsync(added.Id))!.Name.Should().Be("B");

        await repo.DeleteAsync(added.Id);
        (await repo.GetByIdAsync(added.Id)).Should().BeNull();
    }

    [Fact]
    public async Task GetAll_Find_Exists_Count_Fetch_Should_Work()
    {
        var (repo, _, _) = CreateSut();
        await repo.AddRangeAsync(new[]
        {
            new TestEntity { Name = "x1" },
            new TestEntity { Name = "x2" },
            new TestEntity { Name = "y" }
        });

        (await repo.GetAllAsync()).Should().HaveCount(3);
        (await repo.FindAsync(e => e.Name.StartsWith('x'))).Should().HaveCount(2);
        (await repo.ExistsAsync(e => e.Name == "y")).Should().BeTrue();
        (await repo.CountAsync()).Should().Be(3);
        (await repo.FetchAsync(0, 2)).Should().HaveCount(2);
    }

    [Fact]
    public void CreateContext_Should_Return_New_Instance()
    {
        var (repo, context, _) = CreateSut();
        var created = repo.InvokeCreateContext();
        created.Should().NotBeSameAs(context);
        created.Should().BeOfType<TestDbContext>();
    }
}

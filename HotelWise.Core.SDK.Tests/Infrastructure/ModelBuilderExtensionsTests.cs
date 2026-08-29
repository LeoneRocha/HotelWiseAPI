using System.Reflection;
using HotelWise.Core.SDK.Extensions;
using HotelWise.Core.SDK.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelWise.Core.SDK.Tests.Infrastructure;

public class SampleEntity
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class SampleEntityConfiguration : IEntityTypeConfiguration<SampleEntity>
{
    public void Configure(EntityTypeBuilder<SampleEntity> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name).IsRequired();
    }
}

public class SampleDbContext : DbContext
{
    public SampleDbContext(DbContextOptions<SampleDbContext> options) : base(options)
    {
    }

    public DbSet<SampleEntity> Samples => Set<SampleEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.AddConfigurationEntities(Assembly.GetExecutingAssembly(), new List<Type>());
    }
}

public class ModelBuilderExtensionsTests
{
    [Fact]
    public void HelperCharSet_Should_Expose_Latin1()
    {
        HelperCharSet.DefaultCharSet.Should().Be("latin1");
    }

    [Fact]
    public void AddConfigurationEntities_Should_Apply_Discovered_Configurations()
    {
        var options = new DbContextOptionsBuilder<SampleDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new SampleDbContext(options);
        var entityType = context.Model.FindEntityType(typeof(SampleEntity));
        entityType.Should().NotBeNull();
        entityType!.FindProperty(nameof(SampleEntity.Name))!.IsNullable.Should().BeFalse();
    }
}

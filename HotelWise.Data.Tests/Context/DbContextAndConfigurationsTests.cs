using HotelWise.Data.Context;
using HotelWise.Data.Context.Configure.Entity;
using HotelWise.Data.Context.Configure.Entity.HotelModelConfigurations;
using HotelWise.Data.Context.Configure.Helper;
using HotelWise.Domain.Model;
using HotelWise.Domain.Model.AI;
using HotelWise.Domain.Model.HotelModels;
using Microsoft.EntityFrameworkCore;

namespace HotelWise.Data.Tests.Context;

public class DbContextAndConfigurationsTests
{
    // Cenário: Validação de entidades e tabelas geradas pelo DbContext.
    // Objetivo: Cobrir OnModelCreating, DbSets e convenções de chave primária/estrangeira no HotelWiseDbContextMysql.
    [Fact]
    public void HotelWiseDbContextMysql_ShouldConfigureAllDbSetsAndModels()
    {
        // Arrange
        var (context, _) = TestDbFactory.Create();
        using (context)
        {
            // Act & Assert
            Assert.Multiple(() =>
            {
                context.Hotels.Should().NotBeNull();
                context.Rooms.Should().NotBeNull();
                context.Reservations.Should().NotBeNull();
                context.RoomAvailabilities.Should().NotBeNull();
                context.Users.Should().NotBeNull();
                context.ChatSessionHistories.Should().NotBeNull();

                context.Model.FindEntityType(typeof(Hotel)).Should().NotBeNull();
                context.Model.FindEntityType(typeof(Room)).Should().NotBeNull();
                context.Model.FindEntityType(typeof(Reservation)).Should().NotBeNull();
                context.Model.FindEntityType(typeof(RoomAvailability)).Should().NotBeNull();
                context.Model.FindEntityType(typeof(User)).Should().NotBeNull();
                context.Model.FindEntityType(typeof(ChatSessionHistory)).Should().NotBeNull();
            });
        }
    }

    // Cenário: Execução explícita de ConfigurationEntitiesHelper (manual e por reflexão).
    // Objetivo: Cobrir AddConfigurationEntitiesManually e AddConfigurationEntities.
    [Fact]
    public void ConfigurationEntitiesHelper_ShouldApplyConfigurationsSuccessfully()
    {
        // Arrange
        var builderManual = new ModelBuilder();
        var builderDiscovery = new ModelBuilder();

        // Act
        ConfigurationEntitiesHelper.AddConfigurationEntitiesManually(builderManual);
        ConfigurationEntitiesHelper.AddConfigurationEntities(builderDiscovery);

        // Assert
        builderManual.Model.FindEntityType(typeof(Hotel)).Should().NotBeNull();
        builderManual.Model.FindEntityType(typeof(User)).Should().NotBeNull();

        builderDiscovery.Model.FindEntityType(typeof(Room)).Should().NotBeNull();
        builderDiscovery.Model.FindEntityType(typeof(Reservation)).Should().NotBeNull();
        builderDiscovery.Model.FindEntityType(typeof(RoomAvailability)).Should().NotBeNull();
        builderDiscovery.Model.FindEntityType(typeof(ChatSessionHistory)).Should().NotBeNull();
    }

    // Cenário: Aplicação de charset em EntityTypeBuilder via PomeloCharSetHelper.
    // Objetivo: Cobrir PomeloCharSetHelper.AddCharSet.
    [Fact]
    public void PomeloCharSetHelper_ShouldApplyCharSetToEntityBuilder()
    {
        // Arrange
        var modelBuilder = new ModelBuilder();
        var entityBuilder = modelBuilder.Entity<Hotel>();

        // Act
        Action act = () => PomeloCharSetHelper.AddCharSet(entityBuilder);

        // Assert
        act.Should().NotThrow();
    }
}

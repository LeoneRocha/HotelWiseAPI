using HotelWise.Data.Context.Configure.Mock;

namespace HotelWise.Data.Tests.Mock;

public class MockDataGeneratorsTests
{
    // Cenário: Geração e validação do catálogo de hotéis mock.
    // Objetivo: Cobrir HotelsMockData.GetHotels e validar preenchimento de campos obrigatórios.
    [Fact]
    public void HotelsMockData_GetHotels_ShouldReturnPopulatedHotelList()
    {
        // Act
        var hotels = HotelsMockData.GetHotels();

        // Assert
        Assert.Multiple(() =>
        {
            hotels.Should().NotBeNullOrEmpty();
            hotels.Should().HaveCountGreaterThanOrEqualTo(1);
            hotels.Should().AllSatisfy(h =>
            {
                h.HotelName.Should().NotBeNullOrWhiteSpace();
                h.Description.Should().NotBeNullOrWhiteSpace();
                h.Stars.Should().BeInRange(1, 5);
                h.InitialRoomPrice.Should().BeGreaterThan(0);
                h.Tags.Should().NotBeNullOrEmpty();
            });
        });
    }

    // Cenário: Geração e validação do catálogo de quartos mock.
    // Objetivo: Cobrir RoomsMockData.GetRooms e validar integridade dos quartos.
    [Fact]
    public void RoomsMockData_GetRooms_ShouldReturnPopulatedRoomList()
    {
        // Act
        var rooms = RoomsMockData.GetRooms();

        // Assert
        Assert.Multiple(() =>
        {
            rooms.Should().NotBeNullOrEmpty();
            rooms.Should().AllSatisfy(r =>
            {
                r.Name.Should().NotBeNullOrWhiteSpace();
                r.Capacity.Should().BeGreaterThan(0);
                r.MinimumNights.Should().BeGreaterThanOrEqualTo(1);
            });
        });
    }

    // Cenário: Geração e validação de usuários mock.
    // Objetivo: Cobrir UserMockData.GetMock e validar campos de login e perfil.
    [Fact]
    public void UserMockData_GetMock_ShouldReturnValidUsers()
    {
        // Act
        var users = UserMockData.GetMock();

        // Assert
        Assert.Multiple(() =>
        {
            users.Should().NotBeNullOrEmpty();
            users.Should().AllSatisfy(u =>
            {
                u.Name.Should().NotBeNullOrWhiteSpace();
                u.Email.Should().Contain("@");
                u.Login.Should().NotBeNullOrWhiteSpace();
                u.Role.Should().NotBeNullOrWhiteSpace();
            });
        });
    }
}

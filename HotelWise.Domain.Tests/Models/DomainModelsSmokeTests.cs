using HotelWise.Domain.Enuns.Hotel;
using HotelWise.Domain.Model;
using HotelWise.Domain.Model.AI;
using HotelWise.Domain.Model.HotelModels;

namespace HotelWise.Domain.Tests.Models;

public class DomainModelsSmokeTests
{
    // Cenário: construção padrão de Hotel
    // Objetivo: garantir defaults de inicialização do modelo
    [Fact]
    public void Hotel_DefaultConstructor_HasExpectedDefaults()
    {
        // Arrange / Act
        var hotel = new Hotel();

        // Assert
        hotel.HotelId.Should().Be(0);
        hotel.HotelName.Should().BeEmpty();
        hotel.Description.Should().BeEmpty();
        hotel.Tags.Should().BeEmpty();
        hotel.Stars.Should().Be(0);
        hotel.InitialRoomPrice.Should().Be(0);
        hotel.ZipCode.Should().BeEmpty();
        hotel.Location.Should().BeEmpty();
        hotel.City.Should().BeEmpty();
        hotel.StateCode.Should().BeEmpty();
        hotel.CreatedUser.Should().BeNull();
        hotel.ModifyUser.Should().BeNull();
    }

    // Cenário: construção padrão de Room
    // Objetivo: garantir defaults, inclusive MinimumNights = 1
    [Fact]
    public void Room_DefaultConstructor_HasExpectedDefaults()
    {
        // Arrange / Act
        var room = new Room();

        // Assert
        room.Id.Should().Be(0);
        room.HotelId.Should().Be(0);
        room.Name.Should().BeEmpty();
        room.Description.Should().BeEmpty();
        room.Capacity.Should().Be(0);
        room.MinimumNights.Should().Be(1);
        room.RoomType.Should().Be(default(RoomType));
        room.Status.Should().Be(default(RoomStatus));
        room.Hotel.Should().BeNull();
        room.RoomAvailabilities.Should().NotBeNull().And.BeEmpty();
    }

    // Cenário: construção padrão de User
    // Objetivo: garantir defaults de strings vazias e coleções vazias
    [Fact]
    public void User_DefaultConstructor_HasExpectedDefaults()
    {
        // Arrange / Act
        var user = new User();

        // Assert
        user.Id.Should().Be(0);
        user.Name.Should().BeEmpty();
        user.Email.Should().BeEmpty();
        user.Login.Should().BeEmpty();
        user.PasswordHash.Should().BeEmpty();
        user.PasswordSalt.Should().BeEmpty();
        user.Role.Should().BeEmpty();
        user.Admin.Should().BeFalse();
        user.Language.Should().BeEmpty();
        user.TimeZone.Should().BeEmpty();
        user.RefreshToken.Should().BeEmpty();
        user.RefreshTokenExpiryTime.Should().BeNull();
    }

    // Cenário: construção padrão de ChatSessionHistory
    // Objetivo: garantir defaults de título, token e histórico vazio
    [Fact]
    public void ChatSessionHistory_DefaultConstructor_HasExpectedDefaults()
    {
        // Arrange / Act
        var history = new ChatSessionHistory();

        // Assert
        history.Id.Should().Be(0);
        history.Title.Should().BeEmpty();
        history.IdToken.Should().BeEmpty();
        history.PromptMessageHistory.Should().BeEmpty();
        history.CountMessages.Should().Be(0);
        history.TotalTokensMessage.Should().Be(0);
        history.SessionDateTime.Should().Be(default);
        history.IdUser.Should().BeNull();
    }
}

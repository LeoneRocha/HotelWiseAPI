using FluentValidation;
using HotelWise.Domain.Model.HotelModels;
using HotelWise.Domain.Validator.HotelValidators;

namespace HotelWise.Domain.Tests.Validators;

public class HotelValidatorTests
{
    private readonly HotelValidator _validator = new();

    // Cenário: hotel com todos os campos obrigatórios preenchidos corretamente
    // Objetivo: garantir que a validação passa para um hotel válido
    [Fact]
    public async Task ValidateAsync_ValidHotel_Passes()
    {
        // Arrange
        var hotel = new Hotel
        {
            HotelName = "StayMate Inn",
            Description = "Hotel de teste",
            Stars = 4,
            InitialRoomPrice = 150m,
            ZipCode = "01310-100",
            StateCode = "SP",
            Location = "Av. Paulista, 1000",
            City = "São Paulo"
        };

        // Act
        var result = await _validator.ValidateAsync(hotel);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    // Cenário: nome do hotel vazio
    // Objetivo: garantir que HotelName vazio falha a validação
    [Fact]
    public async Task ValidateAsync_EmptyHotelName_Fails()
    {
        // Arrange
        var hotel = new Hotel
        {
            HotelName = string.Empty,
            Stars = 3,
            InitialRoomPrice = 100m
        };

        // Act
        var result = await _validator.ValidateAsync(hotel);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(Hotel.HotelName));
    }

    // Cenário: quantidade de estrelas fora do intervalo 1–5
    // Objetivo: garantir que Stars inválido falha a validação
    [Fact]
    public async Task ValidateAsync_StarsOutOfRange_Fails()
    {
        // Arrange
        var hotel = new Hotel
        {
            HotelName = "StayMate Inn",
            Stars = 0,
            InitialRoomPrice = 100m
        };

        // Act
        var result = await _validator.ValidateAsync(hotel);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(Hotel.Stars) || e.PropertyName.Contains("Stars"));
    }
}

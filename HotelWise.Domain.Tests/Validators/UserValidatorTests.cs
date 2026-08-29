using FluentValidation;
using HotelWise.Domain.Model;
using HotelWise.Domain.Validator;

namespace HotelWise.Domain.Tests.Validators;

public class UserValidatorTests
{
    private readonly UserValidator _validator = new();

    // Cenário: usuário com todos os campos obrigatórios válidos
    // Objetivo: garantir que a validação passa para um usuário válido
    [Fact]
    public async Task ValidateAsync_ValidUser_Passes()
    {
        // Arrange
        var user = CreateValidUser();

        // Act
        var result = await _validator.ValidateAsync(user);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    // Cenário: e-mail em formato inválido
    // Objetivo: garantir que EmailAddress rejeita endereço malformado
    [Fact]
    public async Task ValidateAsync_InvalidEmail_Fails()
    {
        // Arrange
        var user = CreateValidUser();
        user.Email = "not-an-email";

        // Act
        var result = await _validator.ValidateAsync(user);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(User.Email));
    }

    private static User CreateValidUser() => new()
    {
        Id = 1,
        Name = "Leo Costa",
        Email = "leo@hotelwise.com",
        Login = "leocosta",
        PasswordHash = [1, 2, 3],
        PasswordSalt = [4, 5, 6],
        Role = "Admin",
        Admin = true,
        Language = "pt-BR",
        TimeZone = "America/Sao_Paulo",
        RefreshToken = "refresh-token",
        RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7)
    };
}

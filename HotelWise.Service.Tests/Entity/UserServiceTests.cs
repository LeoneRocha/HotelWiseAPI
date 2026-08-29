using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using HotelWise.Core.SDK.Abstractions;
using HotelWise.Core.SDK.Security;
using HotelWise.Domain.Dto;
using HotelWise.Domain.Interfaces.Entity;
using HotelWise.Domain.Model;
using HotelWise.Service.Entity;

namespace HotelWise.Service.Tests.Entity;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepository = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<Serilog.ILogger> _logger = new();
    private readonly Mock<ITokenService> _tokenService = new();
    private readonly Mock<ITokenConfigurationDto> _tokenConfig = new();
    private readonly Mock<IValidator<User>> _validator = new();

    public UserServiceTests()
    {
        _validator.Setup(v => v.ValidateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _tokenConfig.SetupGet(c => c.DaysToExpiry).Returns(7);
        _tokenConfig.SetupGet(c => c.Minutes).Returns(30);
        _tokenService.Setup(t => t.GenerateAccessToken(It.IsAny<IEnumerable<System.Security.Claims.Claim>>()))
            .Returns("access-token");
        _tokenService.Setup(t => t.GenerateRefreshToken()).Returns("refresh-token");
    }

    private UserService CreateSut() =>
        new(
            _logger.Object,
            _userRepository.Object,
            _mapper.Object,
            _tokenService.Object,
            _tokenConfig.Object,
            _validator.Object);

    [Fact]
    public async Task Login_Should_Succeed_With_Valid_Credentials()
    {
        SecurityHelper.CreatePasswordHash("secret", out var hash, out var salt);
        var user = new User
        {
            Id = 1,
            Name = "Admin",
            Login = "admin",
            PasswordHash = hash,
            PasswordSalt = salt
        };
        var authDto = new GetUserAuthenticatedDto { Name = "Admin" };

        _userRepository.Setup(r => r.FindByLogin("admin")).ReturnsAsync(user);
        _userRepository.Setup(r => r.UpdateAsync(user)).ReturnsAsync(user);
        _mapper.Setup(m => m.Map<GetUserAuthenticatedDto>(user)).Returns(authDto);

        var response = await CreateSut().Login("admin", "secret");

        response.Success.Should().BeTrue();
        response.Data.Should().NotBeNull();
        response.Data!.TokenAuth.Should().NotBeNull();
        response.Data.TokenAuth!.AccessToken.Should().Be("access-token");
        response.Message.Should().Contain("successfully");
    }

    [Fact]
    public async Task Login_Should_Fail_When_User_Not_Found()
    {
        _userRepository.Setup(r => r.FindByLogin("missing")).ReturnsAsync((User?)null);

        var response = await CreateSut().Login("missing", "any");

        response.Success.Should().BeFalse();
        response.Data.Should().BeNull();
    }

    [Fact]
    public async Task Login_Should_Fail_When_Password_Is_Wrong()
    {
        SecurityHelper.CreatePasswordHash("correct", out var hash, out var salt);
        var user = new User
        {
            Id = 1,
            Name = "Admin",
            Login = "admin",
            PasswordHash = hash,
            PasswordSalt = salt
        };
        _userRepository.Setup(r => r.FindByLogin("admin")).ReturnsAsync(user);

        var response = await CreateSut().Login("admin", "wrong");

        response.Success.Should().BeFalse();
        response.Message.Should().Be("Wrong password.");
    }
}

using System.Security.Claims;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using HotelWise.Core.SDK.Abstractions;
using HotelWise.Core.SDK.Security;
using HotelWise.Core.SDK.Services;

namespace HotelWise.Core.SDK.Tests.Services;

public class SampleEntity
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class SampleDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class SampleEntityService : GenericEntityServiceBase<SampleEntity, SampleDto>
{
    public SampleEntityService(
        IGenericRepository<SampleEntity> repository,
        IMapper mapper,
        Serilog.ILogger logger,
        IValidator<SampleEntity> entityValidator)
        : base(repository, mapper, logger, entityValidator)
    {
    }
}

public class GenericEntityServiceBaseTests
{
    private readonly Mock<IGenericRepository<SampleEntity>> _repo = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<Serilog.ILogger> _logger = new();
    private readonly Mock<IValidator<SampleEntity>> _validator = new();

    private SampleEntityService CreateSut() =>
        new(_repo.Object, _mapper.Object, _logger.Object, _validator.Object);

    [Fact]
    public async Task GetAllAsync_Should_Map_Entities()
    {
        var entities = new List<SampleEntity> { new() { Id = 1, Name = "A" } };
        var dtos = new List<SampleDto> { new() { Id = 1, Name = "A" } };
        _repo.Setup(r => r.GetAllAsync()).ReturnsAsync(entities);
        _mapper.Setup(m => m.Map<List<SampleDto>>(entities)).Returns(dtos);

        var result = await CreateSut().GetAllAsync();
        result.Should().HaveCount(1);
        result[0].Name.Should().Be("A");
    }

    [Fact]
    public async Task CreateAsync_Should_Fail_When_Validation_Fails()
    {
        var dto = new SampleDto { Name = "x" };
        var entity = new SampleEntity { Name = "x" };
        _mapper.Setup(m => m.Map<SampleEntity>(dto)).Returns(entity);
        _validator.Setup(v => v.ValidateAsync(entity, default)).ReturnsAsync(new ValidationResult(new[]
        {
            new ValidationFailure("Name", "required")
        }));

        var response = await CreateSut().CreateAsync(dto);
        response.Success.Should().BeFalse();
        response.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public async Task CreateAsync_Should_Succeed_When_Valid()
    {
        var dto = new SampleDto { Name = "ok" };
        var entity = new SampleEntity { Name = "ok" };
        var saved = new SampleEntity { Id = 9, Name = "ok" };
        var savedDto = new SampleDto { Id = 9, Name = "ok" };

        _mapper.Setup(m => m.Map<SampleEntity>(dto)).Returns(entity);
        _mapper.Setup(m => m.Map<SampleDto>(saved)).Returns(savedDto);
        _validator.Setup(v => v.ValidateAsync(entity, default)).ReturnsAsync(new ValidationResult());
        _repo.Setup(r => r.AddAsync(entity)).ReturnsAsync(saved);

        var response = await CreateSut().CreateAsync(dto);
        response.Success.Should().BeTrue();
        response.Data!.Id.Should().Be(9);
    }
}

public class TokenServiceTests
{
    [Fact]
    public void GenerateAccessToken_Should_Return_Jwt()
    {
        var config = new Mock<ITokenConfigurationDto>();
        config.SetupGet(c => c.Secret).Returns(new string('a', 64));
        config.SetupGet(c => c.Issuer).Returns("issuer");
        config.SetupGet(c => c.Audience).Returns("audience");
        config.SetupGet(c => c.Minutes).Returns(30);

        var service = new TokenService(config.Object);
        var token = service.GenerateAccessToken(new[] { new Claim(ClaimTypes.Name, "user") });
        token.Should().NotBeNullOrWhiteSpace();
        token.Split('.').Should().HaveCount(3);
    }

    [Fact]
    public void GenerateRefreshToken_Should_Return_Base64()
    {
        var config = new Mock<ITokenConfigurationDto>();
        config.SetupGet(c => c.Secret).Returns(new string('b', 64));
        var service = new TokenService(config.Object);
        var refresh = service.GenerateRefreshToken();
        refresh.Should().NotBeNullOrWhiteSpace();
        Convert.FromBase64String(refresh).Should().HaveCount(32);
    }
}

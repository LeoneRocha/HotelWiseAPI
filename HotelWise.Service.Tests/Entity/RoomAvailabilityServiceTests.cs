using System.Linq.Expressions;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using HotelWise.Domain.Dto.Enitty.HotelDtos;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Repository;
using HotelWise.Domain.Model.HotelModels;
using HotelWise.Service.Entity;

namespace HotelWise.Service.Tests.Entity;

public class RoomAvailabilityServiceTests
{
    private readonly Mock<IRoomAvailabilityRepository> _availabilityRepository = new();
    private readonly Mock<IRoomRepository> _roomRepository = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<Serilog.ILogger> _logger = new();
    private readonly Mock<IValidator<RoomAvailability>> _validator = new();

    public RoomAvailabilityServiceTests()
    {
        _validator.Setup(v => v.ValidateAsync(It.IsAny<RoomAvailability>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
    }

    private RoomAvailabilityService CreateSut() =>
        new(
            _logger.Object,
            _availabilityRepository.Object,
            _roomRepository.Object,
            _mapper.Object,
            _validator.Object);

    [Fact]
    public async Task CreateAsync_Should_Succeed_When_Valid()
    {
        var dto = new RoomAvailabilityDto
        {
            RoomId = 1,
            StartDate = DateTime.UtcNow.Date,
            EndDate = DateTime.UtcNow.Date.AddDays(7),
            Currency = "USD"
        };
        var entity = new RoomAvailability { RoomId = 1, Currency = "USD" };
        var saved = new RoomAvailability { Id = 4, RoomId = 1, Currency = "USD" };
        var savedDto = new RoomAvailabilityDto { Id = 4, RoomId = 1, Currency = "USD" };

        _mapper.Setup(m => m.Map<RoomAvailability>(dto)).Returns(entity);
        _mapper.Setup(m => m.Map<RoomAvailabilityDto>(saved)).Returns(savedDto);
        _availabilityRepository.Setup(r => r.AddAsync(entity)).ReturnsAsync(saved);

        var response = await CreateSut().CreateAsync(dto);

        response.Success.Should().BeTrue();
        response.Data!.Id.Should().Be(4);
    }

    [Fact]
    public async Task CreateAsync_WhenValidationFails_ShouldReturnError()
    {
        var dto = new RoomAvailabilityDto { RoomId = 1 };
        var entity = new RoomAvailability { RoomId = 1 };

        _mapper.Setup(m => m.Map<RoomAvailability>(dto)).Returns(entity);
        _validator.Setup(v => v.ValidateAsync(entity, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult([new ValidationFailure("StartDate", "Data inicial inválida")]));

        var response = await CreateSut().CreateAsync(dto);

        response.Success.Should().BeFalse();
        response.Message.Should().Contain("Data inicial inválida");
    }

    [Fact]
    public async Task CreateBatchAsync_WithEmptyItems_ShouldReturnError()
    {
        var response = await CreateSut().CreateBatchAsync([]);

        response.Success.Should().BeFalse();
        response.Message.Should().Contain("Nenhum item");
    }

    [Fact]
    public async Task CreateBatchAsync_WithInvalidCreationItem_ShouldReturnError()
    {
        var items = new[] { new RoomAvailabilityDto { Id = 0, RoomId = 1 } };
        var entities = new[] { new RoomAvailability { Id = 0, RoomId = 1 } };

        _mapper.Setup(m => m.Map<RoomAvailability[]>(items)).Returns(entities);
        _validator.Setup(v => v.ValidateAsync(entities[0], It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult([new ValidationFailure("Currency", "Moeda obrigatória")]));

        var response = await CreateSut().CreateBatchAsync(items);

        response.Success.Should().BeFalse();
        response.Message.Should().Contain("Erro ao criar item");
    }

    [Fact]
    public async Task CreateBatchAsync_WithNonExistentUpdateItem_ShouldReturnError()
    {
        var items = new[] { new RoomAvailabilityDto { Id = 999, RoomId = 1 } };

        _availabilityRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((RoomAvailability?)null);

        var response = await CreateSut().CreateBatchAsync(items);

        response.Success.Should().BeFalse();
        response.Message.Should().Contain("não encontrada");
    }

    [Fact]
    public async Task CreateBatchAsync_WithInvalidUpdateItem_ShouldReturnError()
    {
        var items = new[] { new RoomAvailabilityDto { Id = 10, RoomId = 1 } };
        var existing = new RoomAvailability { Id = 10, RoomId = 1 };

        _availabilityRepository.Setup(r => r.GetByIdAsync(10)).ReturnsAsync(existing);
        _validator.Setup(v => v.ValidateAsync(existing, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult([new ValidationFailure("EndDate", "Data final inválida")]));

        var response = await CreateSut().CreateBatchAsync(items);

        response.Success.Should().BeFalse();
        response.Message.Should().Contain("Erro ao atualizar item 10");
    }

    [Fact]
    public async Task UpdateAsync_WhenNotFound_ShouldReturnError()
    {
        var dto = new RoomAvailabilityDto { Id = 999 };
        _availabilityRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((RoomAvailability?)null);

        var response = await CreateSut().UpdateAsync(dto);

        response.Success.Should().BeFalse();
        response.Message.Should().Contain("não encontrada");
    }

    [Fact]
    public async Task UpdateAsync_WhenValidationFails_ShouldReturnError()
    {
        var dto = new RoomAvailabilityDto { Id = 5 };
        var existing = new RoomAvailability { Id = 5 };
        var mapped = new RoomAvailability { Id = 5 };

        _availabilityRepository.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(existing);
        _mapper.Setup(m => m.Map<RoomAvailability>(dto)).Returns(mapped);
        _validator.Setup(v => v.ValidateAsync(mapped, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult([new ValidationFailure("Currency", "Moeda inválida")]));

        var response = await CreateSut().UpdateAsync(dto);

        response.Success.Should().BeFalse();
        response.Message.Should().Contain("Moeda inválida");
    }

    [Fact]
    public async Task DeleteAsync_WhenNotFound_ShouldReturnError()
    {
        _availabilityRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((RoomAvailability?)null);

        var response = await CreateSut().DeleteAsync(999);

        response.Success.Should().BeFalse();
        response.Message.Should().Contain("não encontrada");
    }

    [Fact]
    public async Task GetAvailabilitiesByRoomIdAsync_Should_Return_Mapped_Items()
    {
        RoomAvailability[] items =
        [
            new RoomAvailability { Id = 1, RoomId = 9, Currency = "BRL" }
        ];
        RoomAvailabilityDto[] dtos =
        [
            new RoomAvailabilityDto { Id = 1, RoomId = 9, Currency = "BRL" }
        ];

        _roomRepository.Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Room, bool>>>()))
            .ReturnsAsync(true);
        _availabilityRepository.Setup(r => r.GetAvailabilityByRoomId(9)).ReturnsAsync(items);
        _mapper.Setup(m => m.Map<RoomAvailabilityDto[]>(items)).Returns(dtos);

        var response = await CreateSut().GetAvailabilitiesByRoomIdAsync(9);

        response.Success.Should().BeTrue();
        response.Data.Should().ContainSingle(a => a.Id == 1);
    }

    [Fact]
    public async Task GetAvailabilitiesByRoomIdAsync_Should_Fail_When_Room_Does_Not_Exist()
    {
        _roomRepository.Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Room, bool>>>()))
            .ReturnsAsync(false);

        var response = await CreateSut().GetAvailabilitiesByRoomIdAsync(99);

        response.Success.Should().BeFalse();
        response.Message.Should().Contain("não existe");
    }

    [Fact]
    public async Task GetAvailabilitiesBySearchCriteriaAsync_ShouldReturnMappedResults()
    {
        var searchDto = new RoomAvailabilitySearchDto
        {
            HotelId = 1,
            StartDate = DateTime.UtcNow.Date,
            EndDate = DateTime.UtcNow.Date.AddDays(3),
            Currency = "BRL"
        };
        var items = new[] { new RoomAvailability { Id = 10, RoomId = 1 } };
        var dtos = new[] { new RoomAvailabilityDto { Id = 10, RoomId = 1 } };

        _availabilityRepository.Setup(r => r.GetAvailabilitiesByHotelAndPeriodAsync(It.IsAny<HotelAvailabilityRequestDto>()))
            .ReturnsAsync(items);
        _mapper.Setup(m => m.Map<RoomAvailabilityDto[]>(items)).Returns(dtos);

        var response = await CreateSut().GetAvailabilitiesBySearchCriteriaAsync(searchDto);

        response.Success.Should().BeTrue();
        response.Data.Should().ContainSingle();
    }
}

using System.Linq.Expressions;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using HotelWise.Domain.Dto.Enitty.HotelDtos;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Repository;
using HotelWise.Domain.Model.HotelModels;
using HotelWise.Service.Entity.HotelServices;

namespace HotelWise.Service.Tests.Entity;

public class RoomServiceTests
{
    private readonly Mock<IRoomRepository> _roomRepository = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<Serilog.ILogger> _logger = new();
    private readonly Mock<IValidator<Room>> _validator = new();

    public RoomServiceTests()
    {
        _validator.Setup(v => v.ValidateAsync(It.IsAny<Room>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
    }

    private RoomService CreateSut() =>
        new(_logger.Object, _roomRepository.Object, _mapper.Object, _validator.Object);

    [Fact]
    public async Task CreateAsync_Should_Succeed_When_Valid()
    {
        var dto = new RoomDto { HotelId = 1, Name = "Suite", Description = "Nice" };
        var entity = new Room { HotelId = 1, Name = "Suite", Description = "Nice" };
        var saved = new Room { Id = 10, HotelId = 1, Name = "Suite", Description = "Nice" };
        var savedDto = new RoomDto { Id = 10, HotelId = 1, Name = "Suite", Description = "Nice" };

        _mapper.Setup(m => m.Map<Room>(dto)).Returns(entity);
        _mapper.Setup(m => m.Map<RoomDto>(saved)).Returns(savedDto);
        _roomRepository.Setup(r => r.AddAsync(entity)).ReturnsAsync(saved);

        var response = await CreateSut().CreateAsync(dto);

        response.Success.Should().BeTrue();
        response.Data!.Id.Should().Be(10);
        response.Message.Should().Contain("sucesso");
    }

    [Fact]
    public async Task GetRoomsByHotelIdAsync_Should_Return_Mapped_Rooms()
    {
        Room[] rooms = [new Room { Id = 1, HotelId = 5, Name = "A" }];
        RoomDto[] dtos = [new RoomDto { Id = 1, HotelId = 5, Name = "A" }];

        _roomRepository.Setup(r => r.GetRoomsByHotelIdAsync(5)).ReturnsAsync(rooms);
        _mapper.Setup(m => m.Map<RoomDto[]>(rooms)).Returns(dtos);

        var response = await CreateSut().GetRoomsByHotelIdAsync(5);

        response.Success.Should().BeTrue();
        response.Data.Should().ContainSingle(r => r.Id == 1);
    }

    [Fact]
    public async Task DeleteAsync_Should_Fail_When_Room_Not_Found()
    {
        _roomRepository.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Room?)null);

        var response = await CreateSut().DeleteAsync(99);

        response.Success.Should().BeFalse();
        response.Message.Should().Contain("não encontrado");
    }
}

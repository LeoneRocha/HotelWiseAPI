using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using HotelWise.Domain.Dto.Enitty.HotelDtos;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Repository;
using HotelWise.Domain.Model.HotelModels;
using HotelWise.Service.Entity;

namespace HotelWise.Service.Tests.Entity;

public class ReservationServiceTests
{
    private readonly Mock<IReservationRepository> _reservationRepository = new();
    private readonly Mock<IRoomRepository> _roomRepository = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<Serilog.ILogger> _logger = new();
    private readonly Mock<IValidator<Reservation>> _validator = new();

    public ReservationServiceTests()
    {
        _validator.Setup(v => v.ValidateAsync(It.IsAny<Reservation>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
    }

    private ReservationService CreateSut() =>
        new(
            _logger.Object,
            _reservationRepository.Object,
            _roomRepository.Object,
            _mapper.Object,
            _validator.Object);

    [Fact]
    public async Task CreateAsync_Should_Add_Reservation()
    {
        var dto = new ReservationDto
        {
            RoomId = 1,
            CheckInDate = DateTime.UtcNow.Date,
            CheckOutDate = DateTime.UtcNow.Date.AddDays(2),
            TotalAmount = 500,
            Currency = "BRL"
        };
        var entity = new Reservation { RoomId = 1, TotalAmount = 500, Currency = "BRL" };
        var saved = new Reservation { Id = 7, RoomId = 1, TotalAmount = 500, Currency = "BRL" };
        var savedDto = new ReservationDto { Id = 7, RoomId = 1, TotalAmount = 500, Currency = "BRL" };

        _mapper.Setup(m => m.Map<Reservation>(dto)).Returns(entity);
        _mapper.Setup(m => m.Map<ReservationDto>(saved)).Returns(savedDto);
        _reservationRepository.Setup(r => r.AddAsync(entity)).ReturnsAsync(saved);

        var response = await CreateSut().CreateAsync(dto);

        response.Success.Should().BeTrue();
        response.Data!.Id.Should().Be(7);
        response.Message.Should().Contain("sucesso");
    }

    [Fact]
    public async Task GetReservationByIdAsync_Should_Return_Mapped_Reservation()
    {
        var reservation = new Reservation { Id = 3, RoomId = 1, TotalAmount = 100 };
        var dto = new ReservationDto { Id = 3, RoomId = 1, TotalAmount = 100 };

        _reservationRepository.Setup(r => r.GetByIdAsync(3)).ReturnsAsync(reservation);
        _mapper.Setup(m => m.Map<ReservationDto>(reservation)).Returns(dto);

        var response = await CreateSut().GetReservationByIdAsync(3);

        response.Success.Should().BeTrue();
        response.Data!.Id.Should().Be(3);
    }

    [Fact]
    public async Task GetReservationByIdAsync_Should_Fail_When_Not_Found()
    {
        _reservationRepository.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Reservation?)null);

        var response = await CreateSut().GetReservationByIdAsync(99);

        response.Success.Should().BeFalse();
        response.Message.Should().Contain("não encontrada");
    }
}

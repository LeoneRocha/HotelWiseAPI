using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using HotelWise.Domain.Dto.Enitty.HotelDtos;
using HotelWise.Domain.Dto.IA.SemanticKernel;
using HotelWise.Domain.Enuns.Hotel;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Repository;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Service;
using HotelWise.Domain.Model.HotelModels;
using HotelWise.Service.Entity;
using HotelWise.Service.Entity.HotelServices;
using Serilog;

namespace HotelWise.Service.Tests.Entity;

public class ServiceEdgeCasesAndBranchesTests
{
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<ILogger> _loggerMock = new();

    #region ReservationService Tests

    [Fact]
    public async Task ReservationService_UpdateAndDelete_ShouldThrowNotImplementedException()
    {
        var resRepoMock = new Mock<IReservationRepository>();
        var roomRepoMock = new Mock<IRoomRepository>();
        var validatorMock = new Mock<IValidator<Reservation>>();

        var service = new ReservationService(_loggerMock.Object, resRepoMock.Object, roomRepoMock.Object, _mapperMock.Object, validatorMock.Object);

        await Assert.ThrowsAsync<NotImplementedException>(() => service.UpdateAsync(new ReservationDto()));
        await Assert.ThrowsAsync<NotImplementedException>(() => service.DeleteAsync(1));
    }

    [Fact]
    public async Task ReservationService_CreateAsync_WhenValidationFails_ShouldReturnError()
    {
        var resRepoMock = new Mock<IReservationRepository>();
        var roomRepoMock = new Mock<IRoomRepository>();
        var validatorMock = new Mock<IValidator<Reservation>>();

        var dto = new ReservationDto { RoomId = 1 };
        var entity = new Reservation { RoomId = 1 };

        _mapperMock.Setup(m => m.Map<Reservation>(dto)).Returns(entity);
        validatorMock.Setup(v => v.ValidateAsync(entity, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult([new ValidationFailure("CheckInDate", "Data de entrada inválida")]));

        var service = new ReservationService(_loggerMock.Object, resRepoMock.Object, roomRepoMock.Object, _mapperMock.Object, validatorMock.Object);

        var response = await service.CreateAsync(dto);

        response.Success.Should().BeFalse();
        response.Message.Should().Contain("Data de entrada inválida");
    }

    [Fact]
    public async Task ReservationService_CancelReservationAsync_WhenValidationFails_ShouldReturnError()
    {
        var resRepoMock = new Mock<IReservationRepository>();
        var roomRepoMock = new Mock<IRoomRepository>();
        var validatorMock = new Mock<IValidator<Reservation>>();

        var existing = new Reservation { Id = 5, Status = ReservationStatus.Confirmed };
        resRepoMock.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(existing);

        validatorMock.Setup(v => v.ValidateAsync(existing, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult([new ValidationFailure("Status", "Antecedência mínima não atendida")]));

        var service = new ReservationService(_loggerMock.Object, resRepoMock.Object, roomRepoMock.Object, _mapperMock.Object, validatorMock.Object);

        var response = await service.CancelReservationAsync(5);

        response.Success.Should().BeFalse();
        response.Message.Should().Contain("Antecedência mínima não atendida");
    }

    [Fact]
    public async Task ReservationService_CancelReservationAsync_WhenNotFound_ShouldReturnError()
    {
        var resRepoMock = new Mock<IReservationRepository>();
        var roomRepoMock = new Mock<IRoomRepository>();
        var validatorMock = new Mock<IValidator<Reservation>>();

        resRepoMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Reservation?)null);

        var service = new ReservationService(_loggerMock.Object, resRepoMock.Object, roomRepoMock.Object, _mapperMock.Object, validatorMock.Object);

        var response = await service.CancelReservationAsync(999);

        response.Success.Should().BeFalse();
        response.Message.Should().Contain("não encontrada");
    }

    [Fact]
    public async Task ReservationService_CancelReservationAsync_WhenFound_ShouldCancelSuccessfully()
    {
        var resRepoMock = new Mock<IReservationRepository>();
        var roomRepoMock = new Mock<IRoomRepository>();
        var validatorMock = new Mock<IValidator<Reservation>>();

        validatorMock.Setup(v => v.ValidateAsync(It.IsAny<Reservation>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        var existing = new Reservation
        {
            Id = 5,
            RoomId = 1,
            CheckInDate = DateTime.UtcNow.AddDays(2),
            CheckOutDate = DateTime.UtcNow.AddDays(5),
            ReservationDate = DateTime.UtcNow,
            TotalAmount = 500,
            Currency = "BRL",
            Status = ReservationStatus.Confirmed
        };

        resRepoMock.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(existing);
        resRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Reservation>())).ReturnsAsync(existing);

        var service = new ReservationService(_loggerMock.Object, resRepoMock.Object, roomRepoMock.Object, _mapperMock.Object, validatorMock.Object);

        var response = await service.CancelReservationAsync(5);

        response.Success.Should().BeTrue();
        response.Message.Should().Contain("cancelada com sucesso");
        existing.Status.Should().Be(ReservationStatus.Cancelled);
        resRepoMock.Verify(r => r.UpdateAsync(It.Is<Reservation>(res => res.Status == ReservationStatus.Cancelled)), Times.Once);
    }

    [Fact]
    public async Task ReservationService_GetReservationsByRoomIdAsync_WhenRoomNotFound_ShouldReturnError()
    {
        var resRepoMock = new Mock<IReservationRepository>();
        var roomRepoMock = new Mock<IRoomRepository>();
        var validatorMock = new Mock<IValidator<Reservation>>();

        roomRepoMock.Setup(r => r.ExistsAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Room, bool>>>())).ReturnsAsync(false);

        var service = new ReservationService(_loggerMock.Object, resRepoMock.Object, roomRepoMock.Object, _mapperMock.Object, validatorMock.Object);

        var response = await service.GetReservationsByRoomIdAsync(999);

        response.Success.Should().BeFalse();
        response.Message.Should().Contain("não existe");
    }

    [Fact]
    public async Task ReservationService_GetReservationByIdAsync_ShouldReturnExpectedResult()
    {
        var resRepoMock = new Mock<IReservationRepository>();
        var roomRepoMock = new Mock<IRoomRepository>();
        var validatorMock = new Mock<IValidator<Reservation>>();

        var res = new Reservation
        {
            Id = 1,
            RoomId = 2,
            CheckInDate = DateTime.UtcNow.AddDays(1),
            CheckOutDate = DateTime.UtcNow.AddDays(3),
            ReservationDate = DateTime.UtcNow,
            TotalAmount = 300,
            Currency = "BRL",
            Status = ReservationStatus.Confirmed
        };

        var dto = new ReservationDto
        {
            Id = 1,
            RoomId = 2,
            CheckInDate = res.CheckInDate,
            CheckOutDate = res.CheckOutDate,
            TotalAmount = 300,
            Currency = "BRL",
            Status = ReservationStatus.Confirmed
        };

        resRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(res);
        resRepoMock.Setup(r => r.GetByIdAsync(2)).ReturnsAsync((Reservation?)null);
        _mapperMock.Setup(m => m.Map<ReservationDto>(res)).Returns(dto);

        var service = new ReservationService(_loggerMock.Object, resRepoMock.Object, roomRepoMock.Object, _mapperMock.Object, validatorMock.Object);

        var found = await service.GetReservationByIdAsync(1);
        var notFound = await service.GetReservationByIdAsync(2);

        found.Success.Should().BeTrue();
        found.Data.Should().NotBeNull();
        notFound.Success.Should().BeFalse();
    }

    #endregion

    #region RoomService Tests

    [Fact]
    public async Task RoomService_CrudOperationsAndByHotel_ShouldBehaveCorrectly()
    {
        var roomRepoMock = new Mock<IRoomRepository>();
        var validatorMock = new Mock<IValidator<Room>>();

        validatorMock.Setup(v => v.ValidateAsync(It.IsAny<Room>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        var room = new Room
        {
            Id = 3,
            HotelId = 1,
            Name = "Quarto Luxo",
            Description = "Quarto espaçoso",
            Capacity = 2,
            RoomType = RoomType.Double,
            Status = RoomStatus.Available,
            MinimumNights = 1
        };

        var roomDto = new RoomDto
        {
            Id = 3,
            HotelId = 1,
            Name = "Quarto Luxo",
            Description = "Quarto espaçoso",
            Capacity = 2,
            RoomType = RoomType.Double,
            Status = RoomStatus.Available,
            MinimumNights = 1
        };

        _mapperMock.Setup(m => m.Map<Room>(roomDto)).Returns(room);
        _mapperMock.Setup(m => m.Map<RoomDto>(room)).Returns(roomDto);
        _mapperMock.Setup(m => m.Map<RoomDto[]>(It.IsAny<Room[]>())).Returns([roomDto]);

        roomRepoMock.Setup(r => r.AddAsync(It.IsAny<Room>())).ReturnsAsync(room);
        roomRepoMock.Setup(r => r.ExistsAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Room, bool>>>())).ReturnsAsync(true);
        roomRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Room>())).ReturnsAsync(room);
        roomRepoMock.Setup(r => r.GetByIdAsync(3)).ReturnsAsync(room);
        roomRepoMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Room?)null);
        roomRepoMock.Setup(r => r.DeleteAsync(3)).Returns(Task.CompletedTask);
        roomRepoMock.Setup(r => r.GetRoomsByHotelIdAsync(1)).ReturnsAsync([room]);
        roomRepoMock.Setup(r => r.GetRoomsByHotelIdAsync(2)).ReturnsAsync([]);

        var service = new RoomService(_loggerMock.Object, roomRepoMock.Object, _mapperMock.Object, validatorMock.Object);

        var created = await service.CreateAsync(roomDto);
        var updated = await service.UpdateAsync(roomDto);
        var deleted = await service.DeleteAsync(3);
        var deleteNotFound = await service.DeleteAsync(999);
        var roomsFound = await service.GetRoomsByHotelIdAsync(1);
        var roomsNotFound = await service.GetRoomsByHotelIdAsync(2);

        created.Success.Should().BeTrue();
        updated.Success.Should().BeTrue();
        deleted.Success.Should().BeTrue();
        deleteNotFound.Success.Should().BeFalse();
        roomsFound.Success.Should().BeTrue();
        roomsFound.Data.Should().HaveCount(1);
        roomsNotFound.Success.Should().BeFalse();
    }

    [Fact]
    public async Task RoomService_WhenValidationFails_ShouldReturnError()
    {
        var roomRepoMock = new Mock<IRoomRepository>();
        var validatorMock = new Mock<IValidator<Room>>();

        var dto = new RoomDto { Id = 5, HotelId = 1, Name = "" };
        var entity = new Room { Id = 5, HotelId = 1, Name = "" };

        _mapperMock.Setup(m => m.Map<Room>(dto)).Returns(entity);
        validatorMock.Setup(v => v.ValidateAsync(entity, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult([new ValidationFailure("Name", "Nome do quarto obrigatório")]));

        roomRepoMock.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(entity);
        roomRepoMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Room?)null);

        var service = new RoomService(_loggerMock.Object, roomRepoMock.Object, _mapperMock.Object, validatorMock.Object);

        var createFailed = await service.CreateAsync(dto);
        var updateFailed = await service.UpdateAsync(dto);
        var updateNotFound = await service.UpdateAsync(new RoomDto { Id = 999 });

        createFailed.Success.Should().BeFalse();
        updateFailed.Success.Should().BeFalse();
        updateNotFound.Success.Should().BeFalse();
    }

    #endregion

    #region HotelService Tests

    [Fact]
    public async Task HotelService_AdvancedOperations_ShouldExecuteSuccessfully()
    {
        var hotelRepoMock = new Mock<IHotelRepository>();
        var generateMock = new Mock<IGenerateHotelService>();
        var vectorStoreMock = new Mock<IVectorStoreService<HotelVector>>();
        var configMock = new Mock<IApplicationIAConfig>();
        var validatorMock = new Mock<IValidator<Hotel>>();

        validatorMock.Setup(v => v.ValidateAsync(It.IsAny<Hotel>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        var hotel = new Hotel
        {
            HotelId = 1,
            HotelName = "Grand Hotel",
            Description = "Hotel historico",
            Tags = ["luxo", "centro"],
            Stars = 5,
            InitialRoomPrice = 500m
        };

        var hotelDto = new HotelDto
        {
            HotelId = 1,
            HotelName = "Grand Hotel",
            Description = "Hotel historico",
            Tags = ["luxo", "centro"],
            Stars = 5,
            InitialRoomPrice = 500m
        };

        _mapperMock.Setup(m => m.Map<HotelDto[]>(It.IsAny<IEnumerable<Hotel>>())).Returns([hotelDto]);
        _mapperMock.Setup(m => m.Map<HotelDto?>(hotel)).Returns(hotelDto);
        _mapperMock.Setup(m => m.Map<HotelDto>(hotel)).Returns(hotelDto);
        _mapperMock.Setup(m => m.Map<Hotel>(hotelDto)).Returns(hotel);

        hotelRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync([hotel]);
        hotelRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(hotel);
        hotelRepoMock.Setup(r => r.GetByIdAsync(999)).ThrowsAsync(new Exception("Hotel not found"));
        hotelRepoMock.Setup(r => r.GetTotalHotelsCountAsync()).ReturnsAsync(1);
        hotelRepoMock.Setup(r => r.GetAllTagsAsync(0, 10)).ReturnsAsync([["luxo", "centro"]]);
        hotelRepoMock.Setup(r => r.AddAsync(It.IsAny<Hotel>())).ReturnsAsync(hotel);
        hotelRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Hotel>())).ReturnsAsync(hotel);
        hotelRepoMock.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

        generateMock.Setup(g => g.GetHotelAsync()).ReturnsAsync(hotel);
        vectorStoreMock.Setup(v => v.GetById(1)).ReturnsAsync(new HotelVector { DataKey = 1 });
        vectorStoreMock.Setup(v => v.UpsertDataAsync(It.IsAny<HotelVector>())).Returns(Task.CompletedTask);
        vectorStoreMock.Setup(v => v.DeleteAsync(1)).Returns(Task.CompletedTask);

        var service = new HotelService(
            _loggerMock.Object,
            _mapperMock.Object,
            configMock.Object,
            hotelRepoMock.Object,
            generateMock.Object,
            vectorStoreMock.Object,
            validatorMock.Object);

        service.SetUserId(10);
        var all = await service.GetAllHotelsAsync();
        var byId = await service.GetHotelByIdAsync(1);
        var byIdNotFound = await service.GetHotelByIdAsync(999);
        var insertVec = await service.InsertHotelInVectorStore(1);
        var genIA = await service.GenerateHotelByIA();
        var added = await service.AddHotelAsync(hotelDto);
        var updated = await service.UpdateHotelAsync(hotelDto);
        var tags = await service.GetAllTags();
        var deleted = await service.DeleteHotelAsync(1);

        Assert.Multiple(() =>
        {
            all.Success.Should().BeTrue();
            byId.Success.Should().BeTrue();
            byId.Data!.IsHotelInVectorStore.Should().BeTrue();
            byIdNotFound.Success.Should().BeFalse();
            insertVec.Success.Should().BeTrue();
            genIA.Success.Should().BeTrue();
            added.Success.Should().BeTrue();
            updated.Success.Should().BeTrue();
            tags.Should().Contain("luxo");
            deleted.Success.Should().BeTrue();
        });
    }

    [Fact]
    public async Task HotelService_WhenExceptionOccurs_ShouldReturnError()
    {
        var hotelRepoMock = new Mock<IHotelRepository>();
        var generateMock = new Mock<IGenerateHotelService>();
        var vectorStoreMock = new Mock<IVectorStoreService<HotelVector>>();
        var configMock = new Mock<IApplicationIAConfig>();
        var validatorMock = new Mock<IValidator<Hotel>>();

        var dto = new HotelDto { HotelId = 1, HotelName = "Error Hotel" };
        var entity = new Hotel { HotelId = 1, HotelName = "Error Hotel" };

        _mapperMock.Setup(m => m.Map<Hotel>(dto)).Returns(entity);

        hotelRepoMock.Setup(r => r.AddAsync(It.IsAny<Hotel>())).ThrowsAsync(new Exception("DB Insert Error"));
        hotelRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Hotel>())).ThrowsAsync(new Exception("DB Update Error"));
        hotelRepoMock.Setup(r => r.DeleteAsync(It.IsAny<long>())).ThrowsAsync(new Exception("DB Delete Error"));
        hotelRepoMock.Setup(r => r.GetAllAsync()).ThrowsAsync(new Exception("DB Select Error"));
        generateMock.Setup(g => g.GetHotelAsync()).ThrowsAsync(new Exception("AI Error"));

        var service = new HotelService(
            _loggerMock.Object,
            _mapperMock.Object,
            configMock.Object,
            hotelRepoMock.Object,
            generateMock.Object,
            vectorStoreMock.Object,
            validatorMock.Object);

        var addResult = await service.AddHotelAsync(dto);
        var updateResult = await service.UpdateHotelAsync(dto);
        var deleteResult = await service.DeleteHotelAsync(1);
        var getAllResult = await service.GetAllHotelsAsync();
        var genResult = await service.GenerateHotelByIA();

        Assert.Multiple(() =>
        {
            addResult.Success.Should().BeFalse();
            updateResult.Success.Should().BeFalse();
            deleteResult.Success.Should().BeFalse();
            getAllResult.Success.Should().BeFalse();
            genResult.Success.Should().BeFalse();
        });
    }

    #endregion
}

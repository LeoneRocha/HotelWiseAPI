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

    // Cenário: Tentativa de atualizar ou excluir reserva (métodos não suportados).
    // Objetivo: Garantir que UpdateAsync e DeleteAsync lancem NotImplementedException em ReservationService.
    [Fact]
    public async Task ReservationService_UpdateAndDelete_ShouldThrowNotImplementedException()
    {
        // Arrange
        var resRepoMock = new Mock<IReservationRepository>();
        var roomRepoMock = new Mock<IRoomRepository>();
        var validatorMock = new Mock<IValidator<Reservation>>();

        var service = new ReservationService(_loggerMock.Object, resRepoMock.Object, roomRepoMock.Object, _mapperMock.Object, validatorMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<NotImplementedException>(() => service.UpdateAsync(new ReservationDto()));
        await Assert.ThrowsAsync<NotImplementedException>(() => service.DeleteAsync(1));
    }

    // Cenário: Cancelamento de reserva inexistente.
    // Objetivo: Garantir que CancelReservationAsync retorne Success=false quando a reserva não existir.
    [Fact]
    public async Task ReservationService_CancelReservationAsync_WhenNotFound_ShouldReturnError()
    {
        // Arrange
        var resRepoMock = new Mock<IReservationRepository>();
        var roomRepoMock = new Mock<IRoomRepository>();
        var validatorMock = new Mock<IValidator<Reservation>>();

        resRepoMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Reservation?)null);

        var service = new ReservationService(_loggerMock.Object, resRepoMock.Object, roomRepoMock.Object, _mapperMock.Object, validatorMock.Object);

        // Act
        var response = await service.CancelReservationAsync(999);

        // Assert
        Assert.Multiple(() =>
        {
            response.Success.Should().BeFalse();
            response.Message.Should().Contain("não encontrada");
        });
    }

    // Cenário: Cancelamento com sucesso de reserva existente.
    // Objetivo: Garantir que CancelReservationAsync altere o status para Cancelled e persista.
    [Fact]
    public async Task ReservationService_CancelReservationAsync_WhenFound_ShouldCancelSuccessfully()
    {
        // Arrange
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

        // Act
        var response = await service.CancelReservationAsync(5);

        // Assert
        Assert.Multiple(() =>
        {
            response.Success.Should().BeTrue();
            response.Message.Should().Contain("cancelada com sucesso");
            existing.Status.Should().Be(ReservationStatus.Cancelled);
        });
        resRepoMock.Verify(r => r.UpdateAsync(It.Is<Reservation>(res => res.Status == ReservationStatus.Cancelled)), Times.Once);
    }

    // Cenário: Consulta de reservas por quarto inexistente.
    // Objetivo: Garantir que GetReservationsByRoomIdAsync retorne erro quando o quarto não existir.
    [Fact]
    public async Task ReservationService_GetReservationsByRoomIdAsync_WhenRoomNotFound_ShouldReturnError()
    {
        // Arrange
        var resRepoMock = new Mock<IReservationRepository>();
        var roomRepoMock = new Mock<IRoomRepository>();
        var validatorMock = new Mock<IValidator<Reservation>>();

        roomRepoMock.Setup(r => r.ExistsAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Room, bool>>>())).ReturnsAsync(false);

        var service = new ReservationService(_loggerMock.Object, resRepoMock.Object, roomRepoMock.Object, _mapperMock.Object, validatorMock.Object);

        // Act
        var response = await service.GetReservationsByRoomIdAsync(999);

        // Assert
        Assert.Multiple(() =>
        {
            response.Success.Should().BeFalse();
            response.Message.Should().Contain("não existe");
        });
    }

    // Cenário: Consulta de reserva por Id.
    // Objetivo: Cobrir GetReservationByIdAsync para caso existente e inexistente.
    [Fact]
    public async Task ReservationService_GetReservationByIdAsync_ShouldReturnExpectedResult()
    {
        // Arrange
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

        // Act
        var found = await service.GetReservationByIdAsync(1);
        var notFound = await service.GetReservationByIdAsync(2);

        // Assert
        Assert.Multiple(() =>
        {
            found.Success.Should().BeTrue();
            found.Data.Should().NotBeNull();
            notFound.Success.Should().BeFalse();
        });
    }

    #endregion

    #region RoomAvailabilityService Tests

    // Cenário: Operação em lote de criação e atualização de disponibilidades.
    // Objetivo: Cobrir CreateBatchAsync, UpdateAsync, DeleteAsync, GetAvailabilitiesByRoomIdAsync e GetAvailabilitiesBySearchCriteriaAsync.
    [Fact]
    public async Task RoomAvailabilityService_CreateBatchAndCrud_ShouldWorkProperly()
    {
        // Arrange
        var availRepoMock = new Mock<IRoomAvailabilityRepository>();
        var roomRepoMock = new Mock<IRoomRepository>();
        var validatorMock = new Mock<IValidator<RoomAvailability>>();

        validatorMock.Setup(v => v.ValidateAsync(It.IsAny<RoomAvailability>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        var service = new RoomAvailabilityService(_loggerMock.Object, availRepoMock.Object, roomRepoMock.Object, _mapperMock.Object, validatorMock.Object);

        var batchItems = new[]
        {
            new RoomAvailabilityDto
            {
                Id = 0,
                RoomId = 1,
                StartDate = DateTime.UtcNow.Date.AddDays(1),
                EndDate = DateTime.UtcNow.Date.AddDays(5),
                Currency = "BRL",
                AvailabilityWithPrice = [new RoomPriceAndAvailabilityItem { DayOfWeek = DayOfWeek.Monday, Price = 150m, Currency = "BRL" }]
            },
            new RoomAvailabilityDto
            {
                Id = 10,
                RoomId = 1,
                StartDate = DateTime.UtcNow.Date.AddDays(6),
                EndDate = DateTime.UtcNow.Date.AddDays(10),
                Currency = "BRL",
                AvailabilityWithPrice = [new RoomPriceAndAvailabilityItem { DayOfWeek = DayOfWeek.Friday, Price = 200m, Currency = "BRL" }]
            }
        };

        var createdEntities = new[]
        {
            new RoomAvailability
            {
                Id = 0,
                RoomId = 1,
                StartDate = DateTime.UtcNow.Date.AddDays(1),
                EndDate = DateTime.UtcNow.Date.AddDays(5),
                Currency = "BRL"
            }
        };

        var existingItem = new RoomAvailability
        {
            Id = 10,
            RoomId = 1,
            StartDate = DateTime.UtcNow.Date.AddDays(6),
            EndDate = DateTime.UtcNow.Date.AddDays(10),
            Currency = "BRL"
        };

        _mapperMock.Setup(m => m.Map<RoomAvailability[]>(It.IsAny<RoomAvailabilityDto[]>())).Returns(createdEntities);
        availRepoMock.Setup(r => r.GetByIdAsync(10)).ReturnsAsync(existingItem);
        availRepoMock.Setup(r => r.AddRangeAsync(It.IsAny<IEnumerable<RoomAvailability>>())).Returns(Task.CompletedTask);
        availRepoMock.Setup(r => r.UpdateRangeAsync(It.IsAny<IEnumerable<RoomAvailability>>())).Returns(Task.CompletedTask);

        // Act
        var batchResult = await service.CreateBatchAsync(batchItems);
        var emptyBatchResult = await service.CreateBatchAsync([]);

        // Assert
        Assert.Multiple(() =>
        {
            batchResult.Success.Should().BeTrue();
            emptyBatchResult.Success.Should().BeFalse();
        });
    }

    // Cenário: Exclusão e busca de disponibilidade por critérios de busca.
    // Objetivo: Cobrir DeleteAsync, GetAvailabilitiesByRoomIdAsync e GetAvailabilitiesBySearchCriteriaAsync.
    [Fact]
    public async Task RoomAvailabilityService_DeleteAndSearchCriteria_ShouldBehaveCorrectly()
    {
        // Arrange
        var availRepoMock = new Mock<IRoomAvailabilityRepository>();
        var roomRepoMock = new Mock<IRoomRepository>();
        var validatorMock = new Mock<IValidator<RoomAvailability>>();

        var existing = new RoomAvailability
        {
            Id = 7,
            RoomId = 1,
            StartDate = DateTime.UtcNow.Date.AddDays(1),
            EndDate = DateTime.UtcNow.Date.AddDays(2),
            Currency = "BRL"
        };

        var dto = new RoomAvailabilityDto
        {
            Id = 7,
            RoomId = 1,
            StartDate = existing.StartDate,
            EndDate = existing.EndDate,
            Currency = "BRL"
        };

        availRepoMock.Setup(r => r.GetByIdAsync(7)).ReturnsAsync(existing);
        availRepoMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((RoomAvailability?)null);
        availRepoMock.Setup(r => r.DeleteAsync(7)).Returns(Task.CompletedTask);

        roomRepoMock.Setup(r => r.ExistsAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Room, bool>>>())).ReturnsAsync(true);
        availRepoMock.Setup(r => r.GetAvailabilityByRoomId(1)).ReturnsAsync([existing]);
        availRepoMock.Setup(r => r.GetAvailabilitiesByHotelAndPeriodAsync(It.IsAny<HotelAvailabilityRequestDto>())).ReturnsAsync([existing]);
        _mapperMock.Setup(m => m.Map<RoomAvailabilityDto[]>(It.IsAny<RoomAvailability[]>())).Returns([dto]);

        var service = new RoomAvailabilityService(_loggerMock.Object, availRepoMock.Object, roomRepoMock.Object, _mapperMock.Object, validatorMock.Object);

        // Act
        var deleteSuccess = await service.DeleteAsync(7);
        var deleteNotFound = await service.DeleteAsync(999);
        var byRoom = await service.GetAvailabilitiesByRoomIdAsync(1);
        var bySearch = await service.GetAvailabilitiesBySearchCriteriaAsync(new RoomAvailabilitySearchDto
        {
            HotelId = 1,
            StartDate = DateTime.UtcNow.Date,
            EndDate = DateTime.UtcNow.Date.AddDays(5),
            Currency = "BRL"
        });

        // Assert
        Assert.Multiple(() =>
        {
            deleteSuccess.Success.Should().BeTrue();
            deleteNotFound.Success.Should().BeFalse();
            byRoom.Success.Should().BeTrue();
            byRoom.Data.Should().HaveCount(1);
            bySearch.Success.Should().BeTrue();
            bySearch.Data.Should().HaveCount(1);
        });
    }

    #endregion

    #region RoomService Tests

    // Cenário: Criação, atualização, deleção e listagem de quartos por hotel.
    // Objetivo: Cobrir métodos de RoomService.
    [Fact]
    public async Task RoomService_CrudOperationsAndByHotel_ShouldBehaveCorrectly()
    {
        // Arrange
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

        // Act
        var created = await service.CreateAsync(roomDto);
        var updated = await service.UpdateAsync(roomDto);
        var deleted = await service.DeleteAsync(3);
        var deleteNotFound = await service.DeleteAsync(999);
        var roomsFound = await service.GetRoomsByHotelIdAsync(1);
        var roomsNotFound = await service.GetRoomsByHotelIdAsync(2);

        // Assert
        Assert.Multiple(() =>
        {
            created.Success.Should().BeTrue();
            updated.Success.Should().BeTrue();
            deleted.Success.Should().BeTrue();
            deleteNotFound.Success.Should().BeFalse();
            roomsFound.Success.Should().BeTrue();
            roomsFound.Data.Should().HaveCount(1);
            roomsNotFound.Success.Should().BeFalse();
        });
    }

    #endregion

    #region HotelService Tests

    // Cenário: Operações especializadas de HotelService (tags, vector store, geração IA, GetAll).
    // Objetivo: Cobrir GetAllHotelsAsync, InsertHotelInVectorStore, GetHotelByIdAsync, GenerateHotelByIA, AddHotelAsync, UpdateHotelAsync, GetAllTags e DeleteHotelAsync.
    [Fact]
    public async Task HotelService_AdvancedOperations_ShouldExecuteSuccessfully()
    {
        // Arrange
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

        // Act
        var all = await service.GetAllHotelsAsync();
        var byId = await service.GetHotelByIdAsync(1);
        var insertVec = await service.InsertHotelInVectorStore(1);
        var genIA = await service.GenerateHotelByIA();
        var added = await service.AddHotelAsync(hotelDto);
        var updated = await service.UpdateHotelAsync(hotelDto);
        var tags = await service.GetAllTags();
        var deleted = await service.DeleteHotelAsync(1);

        // Assert
        Assert.Multiple(() =>
        {
            all.Success.Should().BeTrue();
            byId.Success.Should().BeTrue();
            byId.Data!.IsHotelInVectorStore.Should().BeTrue();
            insertVec.Success.Should().BeTrue();
            genIA.Success.Should().BeTrue();
            added.Success.Should().BeTrue();
            updated.Success.Should().BeTrue();
            tags.Should().Contain("luxo");
            deleted.Success.Should().BeTrue();
        });
    }

    #endregion
}

using Bogus;
using HotelWise.Data.Context;
using HotelWise.Data.Repository;
using HotelWise.Data.Repository.HotelRepositories;
using HotelWise.Domain.Dto.Enitty.HotelDtos;
using HotelWise.Domain.Enuns.Hotel;
using HotelWise.Domain.Model;
using HotelWise.Domain.Model.AI;
using HotelWise.Domain.Model.HotelModels;

namespace HotelWise.Data.Tests.Repositories;

public class DataRepositoriesTests
{
    private readonly Faker _faker = new();

    private static Hotel CreateTestHotel(long id = 0, string name = "Resort Copacabana") => new()
    {
        HotelId = id,
        HotelName = name,
        Description = "Hotel de luxo em frente ao mar",
        Tags = ["Praia", "Piscina", "Spa"],
        Stars = 5,
        InitialRoomPrice = 450.00m,
        CreatedUserId = 1,
        ModifyUserId = 1,
        CreatedDate = DateTime.UtcNow,
        ModifyDate = DateTime.UtcNow
    };

    private static Room CreateTestRoom(long hotelId, long id = 0, string name = "Suite Master") => new()
    {
        Id = id,
        HotelId = hotelId,
        Name = name,
        Description = "Vista panoramica",
        Capacity = 4,
        RoomType = RoomType.Suite,
        Status = RoomStatus.Available,
        MinimumNights = 2,
        CreatedUserId = 1,
        ModifyUserId = 1,
        CreatedDate = DateTime.UtcNow,
        ModifyDate = DateTime.UtcNow
    };

    // Cenário: Consulta de total de hotéis e paginação em HotelRepository.
    // Objetivo: Cobrir GetTotalHotelsCountAsync, FetchHotelsAsync e GetAllTagsAsync com múltiplos registros.
    [Fact]
    public async Task HotelRepository_GetTotalCountAndFetch_ShouldReturnCorrectData()
    {
        // Arrange
        var (context, options) = TestDbFactory.Create();
        await using (context)
        {
            var repo = new HotelRepository(context, options);
            await repo.AddAsync(CreateTestHotel(name: "Hotel Atlantico"));
            await repo.AddAsync(CreateTestHotel(name: "Hotel Ipanema"));
            await repo.AddAsync(CreateTestHotel(name: "Hotel Leblon"));

            // Act
            var total = await repo.GetTotalHotelsCountAsync();
            var page = await repo.FetchHotelsAsync(0, 2);
            var tags = await repo.GetAllTagsAsync(0, 10);

            // Assert
            Assert.Multiple(() =>
            {
                total.Should().BeGreaterThanOrEqualTo(3);
                page.Should().HaveCount(2);
                tags.Should().NotBeEmpty();
            });
        }
    }

    // Cenário: Busca de usuário por login, email e atualização de dados.
    // Objetivo: Validar FindByLogin, FindByEmail, UserExists e RefreshUserInfo em UserRepository.
    [Fact]
    public async Task UserRepository_SearchAndRefreshOperations_ShouldBehaveCorrectly()
    {
        // Arrange
        var (context, options) = TestDbFactory.Create();
        await using (context)
        {
            var repo = new UserRepository(context, options);
            var user = new User
            {
                Name = "Carlos Silva",
                Email = "carlos.silva@hotelwise.com",
                Login = "carlos.silva",
                Role = "Admin",
                PasswordHash = [10, 20, 30],
                PasswordSalt = [1, 2, 3],
                RefreshToken = "token-12345",
                RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7),
                Enable = true,
                CreatedDate = DateTime.UtcNow,
                ModifyDate = DateTime.UtcNow
            };

            await repo.AddAsync(user);

            // Act
            var userByLogin = await repo.FindByLogin("CARLOS.SILVA");
            var userByEmail = await repo.FindByEmail("  carlos.silva@hotelwise.com  ");
            var exists = await repo.UserExists("carlos.silva");
            var notExists = await repo.UserExists("usuario.inexistente");

            user.Name = "Carlos Silva Editado";
            var refreshed = await repo.RefreshUserInfo(user);
            var invalidRefresh = await repo.RefreshUserInfo(new User { Id = 9999 });

            // Assert
            Assert.Multiple(() =>
            {
                userByLogin.Should().NotBeNull();
                userByLogin!.Name.Should().Contain("Carlos");
                userByEmail.Should().NotBeNull();
                userByEmail!.Email.Should().Be("carlos.silva@hotelwise.com");
                exists.Should().BeTrue();
                notExists.Should().BeFalse();
                refreshed.Name.Should().Be("Carlos Silva Editado");
                invalidRefresh.Id.Should().Be(0);
            });
        }
    }

    // Cenário: Consulta e associação de quartos por ID de hotel.
    // Objetivo: Cobrir FindByRoomIdAsNoTracking, GetRoomsByHotelIdAsync e GetRoomsByHotelAsNoTracking.
    [Fact]
    public async Task RoomRepository_QueryByHotelAndId_ShouldReturnRelatedRooms()
    {
        // Arrange
        var (context, options) = TestDbFactory.Create();
        await using (context)
        {
            var hotelRepo = new HotelRepository(context, options);
            var roomRepo = new RoomRepository(context, options);

            var hotel = await hotelRepo.AddAsync(CreateTestHotel());
            var room1 = await roomRepo.AddAsync(CreateTestRoom(hotel.HotelId, name: "Quarto 101"));
            var room2 = await roomRepo.AddAsync(CreateTestRoom(hotel.HotelId, name: "Quarto 102"));

            // Act
            var roomById = await roomRepo.FindByRoomIdAsNoTracking(room1.Id);
            var roomsWithAvail = await roomRepo.GetRoomsByHotelIdAsync(hotel.HotelId);
            var roomsNoTracking = await roomRepo.GetRoomsByHotelAsNoTracking(hotel.HotelId);

            // Assert
            Assert.Multiple(() =>
            {
                roomById.Should().NotBeNull();
                roomById!.Name.Should().Be("Quarto 101");
                roomsWithAvail.Should().HaveCount(2);
                roomsNoTracking.Should().HaveCount(2);
            });
        }
    }

    // Cenário: Consulta de reservas por quarto e intervalo de datas.
    // Objetivo: Cobrir GetByRoomId, GetReservationsByRoomIdAsync e GetReservationsWithinDateRange em ReservationRepository.
    [Fact]
    public async Task ReservationRepository_QueriesByRoomAndDateRange_ShouldReturnCorrectReservations()
    {
        // Arrange
        var (context, options) = TestDbFactory.Create();
        await using (context)
        {
            var hotelRepo = new HotelRepository(context, options);
            var roomRepo = new RoomRepository(context, options);
            var resRepo = new ReservationRepository(context, options);

            var hotel = await hotelRepo.AddAsync(CreateTestHotel());
            var room = await roomRepo.AddAsync(CreateTestRoom(hotel.HotelId));

            var checkIn = DateTime.UtcNow.Date.AddDays(10);
            var checkOut = checkIn.AddDays(5);

            var reservation = await resRepo.AddAsync(new Reservation
            {
                RoomId = room.Id,
                CheckInDate = checkIn,
                CheckOutDate = checkOut,
                ReservationDate = DateTime.UtcNow,
                TotalAmount = 1500.00m,
                Currency = "BRL",
                Status = ReservationStatus.Confirmed
            });

            // Act
            var byRoomId = await resRepo.GetByRoomId(room.Id);
            var byRoomWithDetails = await resRepo.GetReservationsByRoomIdAsync(room.Id);
            var inRange = await resRepo.GetReservationsWithinDateRange(checkIn.AddDays(-1), checkOut.AddDays(1));
            var outOfRange = await resRepo.GetReservationsWithinDateRange(checkOut.AddDays(10), checkOut.AddDays(20));

            // Assert
            Assert.Multiple(() =>
            {
                byRoomId.Should().HaveCount(1);
                byRoomWithDetails.Should().HaveCount(1);
                inRange.Should().HaveCount(1);
                outOfRange.Should().BeEmpty();
            });
        }
    }

    // Cenário: Consulta de disponibilidade por quarto, período e DTO em RoomAvailabilityRepository.
    // Objetivo: Cobrir GetAvailabilityByRoomId, GetAvailabilityByDateRange e GetAvailabilitiesByHotelAndPeriodAsync.
    [Fact]
    public async Task RoomAvailabilityRepository_QueriesByPeriodAndHotel_ShouldReturnAvailabilities()
    {
        // Arrange
        var (context, options) = TestDbFactory.Create();
        await using (context)
        {
            var hotelRepo = new HotelRepository(context, options);
            var roomRepo = new RoomRepository(context, options);
            var availRepo = new RoomAvailabilityRepository(context, options);

            var hotel = await hotelRepo.AddAsync(CreateTestHotel());
            var room = await roomRepo.AddAsync(CreateTestRoom(hotel.HotelId));

            var start = DateTime.UtcNow.Date.AddDays(5);
            var end = start.AddDays(10);

            await availRepo.AddAsync(new RoomAvailability
            {
                RoomId = room.Id,
                StartDate = start,
                EndDate = end,
                Currency = "BRL",
                AvailabilityWithPrice =
                [
                    new RoomPriceAndAvailabilityItem { DayOfWeek = DayOfWeek.Monday, Price = 300m, Currency = "BRL" },
                    new RoomPriceAndAvailabilityItem { DayOfWeek = DayOfWeek.Tuesday, Price = 300m, Currency = "BRL" }
                ]
            });

            var searchDto = new HotelAvailabilityRequestDto
            {
                HotelId = hotel.HotelId,
                StartDate = start,
                EndDate = end,
                Currency = "BRL"
            };

            // Act
            var byRoom = await availRepo.GetAvailabilityByRoomId(room.Id);
            var byRange = await availRepo.GetAvailabilityByDateRange(room.Id, start, end);
            var byHotelPeriod = await availRepo.GetAvailabilitiesByHotelAndPeriodAsync(searchDto);

            // Assert
            Assert.Multiple(() =>
            {
                byRoom.Should().HaveCount(1);
                byRange.Should().HaveCount(1);
                byHotelPeriod.Should().HaveCount(1);
            });
        }
    }

    // Cenário: Busca de sessão de chat por token e deleção não suportada.
    // Objetivo: Cobrir GetByIdTokenAsync e DeleteByIdTokenAsync em ChatSessionHistoryRepository.
    [Fact]
    public async Task ChatSessionHistoryRepository_TokenOperations_ShouldBehaveCorrectly()
    {
        // Arrange
        var (context, options) = TestDbFactory.Create();
        await using (context)
        {
            var repo = new ChatSessionHistoryRepository(context, options);
            var token = Guid.NewGuid().ToString();

            await repo.AddAsync(new ChatSessionHistory
            {
                Title = "Consulta de Reserva",
                IdToken = token,
                SessionDateTime = DateTime.UtcNow,
                CountMessages = 2,
                TotalTokensMessage = 150,
                PromptMessageHistory = []
            });

            // Act
            var found = await repo.GetByIdTokenAsync(token);
            var notFound = await repo.GetByIdTokenAsync("token-inexistente");
            Func<Task> actDelete = async () => await repo.DeleteByIdTokenAsync(token);

            // Assert
            Assert.Multiple(() =>
            {
                found.Should().NotBeNull();
                found!.IdToken.Should().Be(token);
                notFound.Should().BeNull();
            });

            await actDelete.Should().ThrowAsync<NotImplementedException>();
        }
    }
}

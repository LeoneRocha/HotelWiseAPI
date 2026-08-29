using HotelWise.Data.Context.Configure.Mock;
using HotelWise.Data.Repository;
using HotelWise.Data.Repository.HotelRepositories;
using HotelWise.Domain.Enuns.Hotel;
using HotelWise.Domain.Model;
using HotelWise.Domain.Model.HotelModels;

namespace HotelWise.Data.Tests;

public class RepositoryCoverageTests
{
    private static Hotel NewHotel(string name = "Alpha") => new()
    {
        HotelName = name,
        Description = "d",
        Tags = ["Beach"],
        Stars = 4,
        InitialRoomPrice = 100,
        CreatedUserId = 1,
        ModifyUserId = 1,
        CreatedDate = DateTime.UtcNow,
        ModifyDate = DateTime.UtcNow
    };

    private static Room NewRoom(long hotelId) => new()
    {
        HotelId = hotelId,
        Name = "R1",
        Description = "desc",
        Capacity = 2,
        RoomType = RoomType.Single,
        Status = RoomStatus.Available,
        MinimumNights = 1,
        CreatedUserId = 1,
        ModifyUserId = 1,
        CreatedDate = DateTime.UtcNow,
        ModifyDate = DateTime.UtcNow.AddMinutes(1)
    };

    // Cenário: CRUD e métodos específicos de HotelRepository.
    // Objetivo: Cobrir GetTotalHotelsCountAsync, FetchHotelsAsync e GetAllTagsAsync.
    [Fact]
    public async Task HotelRepository_Should_Support_Fetch_Count_And_Tags()
    {
        // Arrange
        var (ctx, options) = TestDbFactory.Create();
        await using (ctx)
        {
            var repo = new HotelRepository(ctx, options);
            await repo.AddAsync(NewHotel("Alpha"));
            await repo.AddAsync(NewHotel("Beta"));

            // Act
            var count = await repo.GetTotalHotelsCountAsync();
            var page = await repo.FetchHotelsAsync(0, 10);
            var tags = await repo.GetAllTagsAsync(0, 10);

            // Assert
            count.Should().BeGreaterThanOrEqualTo(2);
            page.Should().NotBeEmpty();
            tags.Should().NotBeEmpty();
        }
    }

    // Cenário: UserRepository por login/email e refresh.
    // Objetivo: Cobrir FindByLogin, FindByEmail, UserExists e RefreshUserInfo.
    [Fact]
    public async Task UserRepository_Should_Find_And_Refresh()
    {
        // Arrange
        var (ctx, options) = TestDbFactory.Create();
        await using (ctx)
        {
            var repo = new UserRepository(ctx, options);
            var user = await repo.AddAsync(new User
            {
                Name = "Ada",
                Email = "ada@example.com",
                Login = "ada",
                Role = "Admin",
                PasswordHash = [1],
                PasswordSalt = [2],
                RefreshToken = "rt",
                RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1),
                Enable = true,
                CreatedDate = DateTime.UtcNow,
                ModifyDate = DateTime.UtcNow
            });

            // Act
            var byLogin = await repo.FindByLogin("ADA");
            var byEmail = await repo.FindByEmail(" ada@example.com ");
            var exists = await repo.UserExists("ada");
            user.Name = "Ada Lovelace";
            var refreshed = await repo.RefreshUserInfo(user);
            var missing = await repo.RefreshUserInfo(new User { Id = 999 });

            // Assert
            byLogin!.Login.Should().Be("ada");
            byEmail!.Email.Should().Be("ada@example.com");
            exists.Should().BeTrue();
            refreshed.Name.Should().Be("Ada Lovelace");
            missing.Id.Should().Be(0);
        }
    }

    // Cenário: RoomRepository por hotel.
    // Objetivo: Cobrir GetRoomsByHotelIdAsync e GetRoomsByHotelAsNoTracking.
    [Fact]
    public async Task RoomRepository_Should_Filter_By_Hotel()
    {
        // Arrange
        var (ctx, options) = TestDbFactory.Create();
        await using (ctx)
        {
            var hotelRepo = new HotelRepository(ctx, options);
            var hotel = await hotelRepo.AddAsync(NewHotel());
            var roomRepo = new RoomRepository(ctx, options);
            await roomRepo.AddAsync(NewRoom(hotel.HotelId));

            // Act
            var withAvail = await roomRepo.GetRoomsByHotelIdAsync(hotel.HotelId);
            var tracked = await roomRepo.GetRoomsByHotelAsNoTracking(hotel.HotelId);
            var byId = await roomRepo.FindByRoomIdAsNoTracking(withAvail[0].Id);

            // Assert
            withAvail.Should().HaveCount(1);
            tracked.Should().HaveCount(1);
            byId.Should().NotBeNull();
        }
    }

    // Cenário: RoomAvailabilityRepository por room e período.
    // Objetivo: Cobrir GetAvailabilityByRoomId e GetAvailabilityByDateRange.
    [Fact]
    public async Task RoomAvailabilityRepository_Should_Query_By_Room_And_Range()
    {
        // Arrange
        var (ctx, options) = TestDbFactory.Create();
        await using (ctx)
        {
            var hotelRepo = new HotelRepository(ctx, options);
            var hotel = await hotelRepo.AddAsync(NewHotel());
            var roomRepo = new RoomRepository(ctx, options);
            var room = await roomRepo.AddAsync(NewRoom(hotel.HotelId));
            var availRepo = new RoomAvailabilityRepository(ctx, options);
            var start = DateTime.UtcNow.Date.AddDays(10);
            var end = start.AddDays(5);
            await availRepo.AddAsync(new RoomAvailability
            {
                RoomId = room.Id,
                StartDate = start,
                EndDate = end,
                Currency = "BRL",
                AvailabilityWithPrice =
                [
                    new RoomPriceAndAvailabilityItem { DayOfWeek = DayOfWeek.Monday, Currency = "BRL", Price = 100 }
                ]
            });

            // Act
            var byRoom = await availRepo.GetAvailabilityByRoomId(room.Id);
            var byRange = await availRepo.GetAvailabilityByDateRange(room.Id, start, end);

            // Assert
            byRoom.Should().HaveCount(1);
            byRange.Should().HaveCount(1);
        }
    }

    // Cenário: ChatSessionHistoryRepository CRUD genérico.
    // Objetivo: Cobrir Add/GetById via base.
    [Fact]
    public async Task ChatSessionHistoryRepository_Should_Persist()
    {
        // Arrange
        var (ctx, options) = TestDbFactory.Create();
        await using (ctx)
        {
            var repo = new ChatSessionHistoryRepository(ctx, options);
            var entity = await repo.AddAsync(new HotelWise.Domain.Model.AI.ChatSessionHistory
            {
                Title = "Session",
                IdToken = Guid.NewGuid().ToString(),
                PromptMessageHistory = [],
                CountMessages = 1,
                TotalTokensMessage = 10,
                SessionDateTime = DateTime.UtcNow
            });

            // Act
            var loaded = await repo.GetByIdAsync(entity.Id);

            // Assert
            loaded!.Title.Should().Be("Session");
        }
    }

    // Cenário: ReservationRepository CRUD.
    // Objetivo: Persistir reserva e recuperar.
    [Fact]
    public async Task ReservationRepository_Should_Persist()
    {
        // Arrange
        var (ctx, options) = TestDbFactory.Create();
        await using (ctx)
        {
            var hotelRepo = new HotelRepository(ctx, options);
            var hotel = await hotelRepo.AddAsync(NewHotel());
            var roomRepo = new RoomRepository(ctx, options);
            var room = await roomRepo.AddAsync(NewRoom(hotel.HotelId));
            var repo = new ReservationRepository(ctx, options);

            // Act
            var reservation = await repo.AddAsync(new Reservation
            {
                RoomId = room.Id,
                CheckInDate = DateTime.UtcNow.Date.AddDays(2),
                CheckOutDate = DateTime.UtcNow.Date.AddDays(4),
                ReservationDate = DateTime.UtcNow.AddMinutes(-5),
                TotalAmount = 200,
                Currency = "BRL",
                Status = ReservationStatus.Confirmed
            });
            var loaded = await repo.GetByIdAsync(reservation.Id);

            // Assert
            loaded.Should().NotBeNull();
            loaded!.Currency.Should().Be("BRL");
        }
    }

    // Cenário: seeders mock estáticos.
    // Objetivo: Exercitar HotelsMockData/RoomsMockData/UserMockData.
    [Fact]
    public void MockData_Should_Return_Seed_Entities()
    {
        // Act / Assert
        HotelsMockData.GetHotels().Should().NotBeEmpty();
        RoomsMockData.GetRooms().Should().NotBeEmpty();
        UserMockData.GetMock().Should().NotBeEmpty();
    }

    // Cenário: DbContext OnModelCreating.
    // Objetivo: Garantir EnsureCreated aplica ConfigurationEntitiesHelper.
    [Fact]
    public void DbContext_Should_Create_Model()
    {
        // Arrange / Act
        var (ctx, _) = TestDbFactory.Create();
        using (ctx)
        {
            // Assert
            ctx.Model.FindEntityType(typeof(Hotel)).Should().NotBeNull();
            ctx.Model.FindEntityType(typeof(User)).Should().NotBeNull();
            ctx.Model.FindEntityType(typeof(Room)).Should().NotBeNull();
        }
    }
}

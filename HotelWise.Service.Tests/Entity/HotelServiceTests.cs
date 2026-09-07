using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using HotelWise.Domain.Dto.Enitty.HotelDtos;
using HotelWise.Domain.Dto.IA.SemanticKernel;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Repository;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Service;
using HotelWise.Domain.Model.HotelModels;
using HotelWise.Service.Entity.HotelServices;

namespace HotelWise.Service.Tests.Entity;

public class HotelServiceTests
{
    private readonly Mock<IHotelRepository> _hotelRepository = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<Serilog.ILogger> _logger = new();
    private readonly Mock<IGenerateHotelService> _generateHotelService = new();
    private readonly Mock<IVectorStoreService<HotelVector>> _vectorStore = new();
    private readonly Mock<IValidator<Hotel>> _validator = new();
    private readonly Mock<IApplicationIAConfig> _appConfig = new();

    public HotelServiceTests()
    {
        _appConfig.SetupGet(c => c.RagConfig).Returns(new RagConfig());
        _validator.Setup(v => v.ValidateAsync(It.IsAny<Hotel>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
    }

    private HotelService CreateSut() =>
        new(
            _logger.Object,
            _mapper.Object,
            _appConfig.Object,
            _hotelRepository.Object,
            _generateHotelService.Object,
            _vectorStore.Object,
            _validator.Object);

    [Fact]
    public async Task GetAllHotelsAsync_Should_Return_Mapped_Hotels_Ordered_By_Name()
    {
        var hotels = new Hotel[]
        {
            new() { HotelId = 2, HotelName = "Zulu" },
            new() { HotelId = 1, HotelName = "Alpha" }
        };
        HotelDto[] dtos =
        [
            new HotelDto { HotelId = 2, HotelName = "Zulu" },
            new HotelDto { HotelId = 1, HotelName = "Alpha" }
        ];

        _hotelRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(hotels);
        _mapper.Setup(m => m.Map<HotelDto[]>(hotels)).Returns(dtos);

        var response = await CreateSut().GetAllHotelsAsync();

        response.Success.Should().BeTrue();
        response.Data.Should().NotBeNull();
        response.Data!.Select(h => h.HotelName).Should().Equal("Alpha", "Zulu");
        response.Errors.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllHotelsAsync_Should_Add_Error_When_Repository_Throws()
    {
        _hotelRepository.Setup(r => r.GetAllAsync())
            .ThrowsAsync(new InvalidOperationException("db down"));

        var response = await CreateSut().GetAllHotelsAsync();

        response.Errors.Should().ContainSingle(e => e.Message == "db down");
        response.Data.Should().BeNull();
    }

    [Fact]
    public async Task SyncAllHotelsToVectorStoreAsync_When_No_Hotels_Should_Return_Success_With_Zero()
    {
        _hotelRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Array.Empty<Hotel>());

        var response = await CreateSut().SyncAllHotelsToVectorStoreAsync();

        response.Success.Should().BeTrue();
        response.Data.Should().NotBeNull();
        response.Data!.TotalHotels.Should().Be(0);
        response.Data.SynchronizedCount.Should().Be(0);
        response.Data.FailedCount.Should().Be(0);
        response.Data.AllProcessed.Should().BeTrue();
    }

    [Fact]
    public async Task SyncAllHotelsToVectorStoreAsync_Should_Process_All_Hotels_In_Parallel_And_Return_Success()
    {
        var hotels = new Hotel[]
        {
            new() { HotelId = 1, HotelName = "Alpha", Description = "Desc 1", Tags = ["t1"] },
            new() { HotelId = 2, HotelName = "Beta", Description = "Desc 2", Tags = ["t2"] }
        };

        _hotelRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(hotels);
        _hotelRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(hotels[0]);
        _hotelRepository.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(hotels[1]);
        _mapper.Setup(m => m.Map<HotelDto>(hotels[0])).Returns(new HotelDto { HotelId = 1, HotelName = "Alpha", Description = "Desc 1", Tags = ["t1"] });
        _mapper.Setup(m => m.Map<HotelDto>(hotels[1])).Returns(new HotelDto { HotelId = 2, HotelName = "Beta", Description = "Desc 2", Tags = ["t2"] });
        _vectorStore.Setup(v => v.UpsertDataAsync(It.IsAny<HotelVector>())).Returns(Task.CompletedTask);

        var response = await CreateSut().SyncAllHotelsToVectorStoreAsync();

        response.Success.Should().BeTrue();
        response.Data.Should().NotBeNull();
        response.Data!.TotalHotels.Should().Be(2);
        response.Data.SynchronizedCount.Should().Be(2);
        response.Data.FailedCount.Should().Be(0);
        response.Data.AllProcessed.Should().BeTrue();
        _vectorStore.Verify(v => v.UpsertDataAsync(It.IsAny<HotelVector>()), Times.Exactly(2));
    }

    [Fact]
    public async Task SyncAllHotelsToVectorStoreAsync_When_One_Fails_Should_Report_Failures()
    {
        var hotels = new Hotel[]
        {
            new() { HotelId = 1, HotelName = "Alpha", Description = "Desc 1", Tags = ["t1"] },
            new() { HotelId = 2, HotelName = "Beta", Description = "Desc 2", Tags = ["t2"] }
        };

        _hotelRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(hotels);
        _hotelRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(hotels[0]);
        _hotelRepository.Setup(r => r.GetByIdAsync(2)).ThrowsAsync(new InvalidOperationException("Qdrant error"));
        _mapper.Setup(m => m.Map<HotelDto>(hotels[0])).Returns(new HotelDto { HotelId = 1, HotelName = "Alpha", Description = "Desc 1", Tags = ["t1"] });
        _vectorStore.Setup(v => v.UpsertDataAsync(It.Is<HotelVector>(x => x.DataKey == 1))).Returns(Task.CompletedTask);

        var response = await CreateSut().SyncAllHotelsToVectorStoreAsync();

        response.Success.Should().BeFalse();
        response.Data.Should().NotBeNull();
        response.Data!.TotalHotels.Should().Be(2);
        response.Data.SynchronizedCount.Should().Be(1);
        response.Data.FailedCount.Should().Be(1);
        response.Data.AllProcessed.Should().BeTrue();
        response.Data.Errors.Should().ContainSingle(e => e.Contains("Beta") && e.Contains("Qdrant error"));
    }
}


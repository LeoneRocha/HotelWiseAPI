using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using HotelWise.Domain.Dto.IA;
using HotelWise.Domain.Dto.IA.SemanticKernel;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Service;
using HotelWise.Domain.Interfaces.Entity.IA;
using HotelWise.Domain.Model.AI;
using HotelWise.Service.Entity;
using Serilog;

namespace HotelWise.Service.Tests.Entity;

public class ChatSessionHistoryServiceTests
{
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<ILogger> _loggerMock = new();
    private readonly Mock<IApplicationIAConfig> _configMock = new();
    private readonly Mock<IChatSessionHistoryRepository> _repositoryMock = new();
    private readonly Mock<IGenerateHotelService> _generateHotelServiceMock = new();
    private readonly Mock<IVectorStoreService<HotelVector>> _vectorStoreServiceMock = new();
    private readonly Mock<IValidator<ChatSessionHistory>> _validatorMock = new();

    public ChatSessionHistoryServiceTests()
    {
        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ChatSessionHistory>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
    }

    private ChatSessionHistoryService CreateService() => new(
        _loggerMock.Object,
        _mapperMock.Object,
        _configMock.Object,
        _repositoryMock.Object,
        _generateHotelServiceMock.Object,
        _vectorStoreServiceMock.Object,
        _validatorMock.Object);

    // Cenário: Consulta de sessão de chat por token válido existente.
    // Objetivo: Garantir que GetByIdTokenAsync mapeie corretamente a entidade para DTO.
    [Fact]
    public async Task GetByIdTokenAsync_WhenTokenExists_ShouldReturnMappedDto()
    {
        // Arrange
        var service = CreateService();
        var token = Guid.NewGuid().ToString();
        var entity = new ChatSessionHistory
        {
            Id = 10,
            Title = "Dúvida sobre Resort",
            IdToken = token,
            CountMessages = 3,
            TotalTokensMessage = 180,
            SessionDateTime = DateTime.UtcNow
        };

        var dto = new ChatSessionHistoryDto
        {
            Id = 10,
            Title = "Dúvida sobre Resort",
            IdToken = token,
            CountMessages = 3,
            TotalTokensMessage = 180,
            SessionDateTime = entity.SessionDateTime
        };

        _repositoryMock
            .Setup(r => r.GetByIdTokenAsync(token))
            .ReturnsAsync(entity);

        _mapperMock
            .Setup(m => m.Map<ChatSessionHistoryDto>(entity))
            .Returns(dto);

        // Act
        var result = await service.GetByIdTokenAsync(token);

        // Assert
        Assert.Multiple(() =>
        {
            result.Should().NotBeNull();
            result!.IdToken.Should().Be(token);
            result.Title.Should().Be("Dúvida sobre Resort");
            result.CountMessages.Should().Be(3);
        });
        _repositoryMock.Verify(r => r.GetByIdTokenAsync(token), Times.Once);
    }

    // Cenário: Consulta de sessão de chat por token inexistente.
    // Objetivo: Garantir que GetByIdTokenAsync retorne null.
    [Fact]
    public async Task GetByIdTokenAsync_WhenTokenDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        var service = CreateService();
        var token = "token-inexistente";

        _repositoryMock
            .Setup(r => r.GetByIdTokenAsync(token))
            .ReturnsAsync((ChatSessionHistory?)null);

        // Act
        var result = await service.GetByIdTokenAsync(token);

        // Assert
        result.Should().BeNull();
        _repositoryMock.Verify(r => r.GetByIdTokenAsync(token), Times.Once);
    }

    // Cenário: Exclusão de sessão de chat por token.
    // Objetivo: Garantir que DeleteByIdTokenAsync propague a chamada ao repositório.
    [Fact]
    public async Task DeleteByIdTokenAsync_ShouldInvokeRepositoryDelete()
    {
        // Arrange
        var service = CreateService();
        var token = Guid.NewGuid().ToString();

        _repositoryMock
            .Setup(r => r.DeleteByIdTokenAsync(token))
            .Returns(Task.CompletedTask);

        // Act
        await service.DeleteByIdTokenAsync(token);

        // Assert
        _repositoryMock.Verify(r => r.DeleteByIdTokenAsync(token), Times.Once);
    }
}

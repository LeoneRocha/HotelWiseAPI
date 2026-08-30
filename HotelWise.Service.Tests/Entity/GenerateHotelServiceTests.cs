using HotelWise.Service.Entity;

namespace HotelWise.Service.Tests.Entity;

public class GenerateHotelServiceTests
{
    private readonly Mock<IAIInferenceService> _inference = new();
    private readonly Mock<IApplicationIAConfig> _appConfig = new();

    public GenerateHotelServiceTests()
    {
        _appConfig.SetupGet(c => c.RagConfig).Returns(new RagConfig
        {
            AIChatServiceAdapter = AIChatServiceType.SemanticKernel
        });
    }

    private GenerateHotelService CreateSut() =>
        new(_inference.Object, _appConfig.Object);

    [Fact]
    public async Task GetHotelAsync_Should_Parse_Pipe_Delimited_Ai_Response()
    {
        const string aiText =
            "Hotel Paradise|Beautiful hotel near the beach with great service|Rio de Janeiro|RJ|20000-000|luxury|beach|resort|pool|spa";

        _inference.Setup(i => i.GenerateChatCompletionAsync(
                It.IsAny<PromptMessageVO[]>(),
                InferenceAiAdapterType.SemanticKernel))
            .ReturnsAsync(aiText);

        var hotel = await CreateSut().GetHotelAsync();

        hotel.Should().NotBeNull();
        hotel.HotelName.Should().Be("Hotel Paradise");
        hotel.Description.Should().Contain("Beautiful hotel");
        hotel.City.Should().Be("Rio de Janeiro");
        hotel.StateCode.Should().Be("RJ");
        hotel.ZipCode.Should().Be("20000-000");
        hotel.Tags.Should().NotBeEmpty();
        hotel.Tags.Should().Contain(t => t.Contains("luxury") || t.Contains("beach") || t.Contains("resort"));
    }

    [Fact]
    public async Task GetHotelAsync_WithCommaSeparatedTags_ShouldParseCorrectly()
    {
        const string aiText =
            "Pousada Sol|Pousada aconchegante|Florianopolis|SC|88000-000|praia, piscina, cafe";

        _inference.Setup(i => i.GenerateChatCompletionAsync(
                It.IsAny<PromptMessageVO[]>(),
                InferenceAiAdapterType.SemanticKernel))
            .ReturnsAsync(aiText);

        var hotel = await CreateSut().GetHotelAsync();

        hotel.Should().NotBeNull();
        hotel.HotelName.Should().Be("Pousada Sol");
        hotel.Tags.Should().Contain("praia");
    }

    [Fact]
    public async Task GetHotelsAsync_Should_Generate_Multiple_Hotels()
    {
        const string aiText =
            "Grand Hotel|Descricao completa do hotel|Sao Paulo|SP|01000-000|centro|executivo";

        _inference.Setup(i => i.GenerateChatCompletionAsync(
                It.IsAny<PromptMessageVO[]>(),
                InferenceAiAdapterType.SemanticKernel))
            .ReturnsAsync(aiText);

        var hotels = await CreateSut().GetHotelsAsync(2);

        hotels.Should().NotBeNull();
        hotels.Should().HaveCount(2);
        hotels[0].HotelName.Should().Be("Grand Hotel");
    }

    [Fact]
    public async Task GetHotelAsync_Should_Return_Empty_Hotel_When_Format_Invalid()
    {
        _inference.Setup(i => i.GenerateChatCompletionAsync(
                It.IsAny<PromptMessageVO[]>(),
                InferenceAiAdapterType.SemanticKernel))
            .ReturnsAsync("apenas-um-campo");

        var hotel = await CreateSut().GetHotelAsync();

        hotel.Should().NotBeNull();
        hotel.HotelName.Should().BeEmpty();
    }
}


using HotelWise.Domain.Dto.IA.SemanticKernel;
using HotelWise.Service.Bussines;

namespace HotelWise.Service.Tests.Bussines;

public class HotelResponseProcessorTests
{
    [Fact]
    public void ProcessResponse_Should_Extract_Hotel_Ids_From_Html_Comments()
    {
        // Arrange
        const string markdown = """
            ### Opções
            <!-- ID-Hotel: 1234 -->
            <!-- ID-Hotel: 5678 -->
            _fim_
            """;

        // Act
        HotelInfo[] result = HotelResponseProcessor.ProcessResponse(markdown);

        // Assert
        result.Should().HaveCount(2);
        result.Select(h => h.Id).Should().BeEquivalentTo([1234L, 5678L]);
        result.Should().OnlyContain(h => h.IdType == "Hotel");
    }

    [Fact]
    public void ProcessResponse_Should_Return_Empty_When_No_Ids()
    {
        HotelInfo[] result = HotelResponseProcessor.ProcessResponse("Sem comentários de hotel.");

        result.Should().BeEmpty();
    }

    [Fact]
    public void ProcessResponse_Should_Ignore_Whitespace_Around_Id()
    {
        HotelInfo[] result = HotelResponseProcessor.ProcessResponse("<!--   ID-Hotel:   99   -->");

        result.Should().ContainSingle()
            .Which.Id.Should().Be(99);
    }
}

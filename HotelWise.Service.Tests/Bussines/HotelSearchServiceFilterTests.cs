using HotelWise.Domain.Dto.Enitty.HotelDtos;
using HotelWise.Domain.Dto.IA.SemanticKernel;
using HotelWise.Service.Entity;

namespace HotelWise.Service.Tests.Bussines;

public class HotelSearchServiceFilterTests
{
    [Fact]
    public void FilterHotelsByIAResult_Should_Throw_When_Response_Is_Null()
    {
        var action = () => HotelSearchService.FilterHotelsByIAResult(null!, [new HotelInfo { Id = 1, IdType = "Hotel" }]);

        action.Should().Throw<InvalidOperationException>()
            .WithMessage("*não podem ser nulos*");
    }

    [Fact]
    public void FilterHotelsByIAResult_Should_Throw_When_HotelsVectorResult_Is_Null()
    {
        var response = new HotelSemanticResult { HotelsVectorResult = null! };

        var action = () => HotelSearchService.FilterHotelsByIAResult(
            response,
            [new HotelInfo { Id = 1, IdType = "Hotel" }]);

        action.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void FilterHotelsByIAResult_Should_Throw_When_Interference_Is_Null()
    {
        var response = new HotelSemanticResult
        {
            HotelsVectorResult = [new HotelDto { HotelId = 1, HotelName = "A" }]
        };

        var action = () => HotelSearchService.FilterHotelsByIAResult(response, null!);

        action.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void FilterHotelsByIAResult_Should_Keep_Only_Hotels_Matching_IA_Ids()
    {
        var response = new HotelSemanticResult
        {
            HotelsVectorResult =
            [
                new HotelDto { HotelId = 10, HotelName = "Keep" },
                new HotelDto { HotelId = 20, HotelName = "Drop" },
                new HotelDto { HotelId = 30, HotelName = "Keep2" }
            ]
        };
        HotelInfo[] ia =
        [
            new HotelInfo { Id = 10, IdType = "Hotel" },
            new HotelInfo { Id = 30, IdType = "Hotel" }
        ];

        var filtered = HotelSearchService.FilterHotelsByIAResult(response, ia);

        filtered.HotelsVectorResult.Should().HaveCount(2);
        filtered.HotelsVectorResult.Select(h => h.HotelId).Should().BeEquivalentTo([10L, 30L]);
    }

    [Fact]
    public void FilterHotelsByIAResult_Should_Return_Empty_When_No_Id_Matches()
    {
        var response = new HotelSemanticResult
        {
            HotelsVectorResult = [new HotelDto { HotelId = 1, HotelName = "Only" }]
        };
        HotelInfo[] ia = [new HotelInfo { Id = 999, IdType = "Hotel" }];

        var filtered = HotelSearchService.FilterHotelsByIAResult(response, ia);

        filtered.HotelsVectorResult.Should().BeEmpty();
    }
}

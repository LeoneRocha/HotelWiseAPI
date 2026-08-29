using HotelWise.Core.SDK.Abstractions;
using HotelWise.Core.SDK.Common;
using HotelWise.Core.SDK.Common.Constants;
using HotelWise.Core.SDK.Common.Exceptions;
using HotelWise.Core.SDK.Security;

namespace HotelWise.Core.SDK.Tests.Domain;

public class LoteD2DtoAndConstantsTests
{
    private sealed class SampleDto : EntityDtoBase
    {
    }

    [Fact]
    public void EntityDtoBase_Should_Implement_IEntityDto()
    {
        var dto = new SampleDto { Id = 7, Enable = true };

        dto.Should().BeAssignableTo<IEntityDto>();
        dto.Id.Should().Be(7);
        dto.Enable.Should().BeTrue();
    }

    [Fact]
    public void TokenVO_Should_Expose_Constructor_Values()
    {
        var token = new TokenVO(true, "c", "e", "access", "refresh");

        token.Authenticated.Should().BeTrue();
        token.AccessToken.Should().Be("access");
        token.RefreshToken.Should().Be("refresh");
    }

    [Fact]
    public void TokenConfigurationDto_Should_Implement_ITokenConfigurationDto()
    {
        TokenConfigurationDto cfg = new TokenConfigurationDto
        {
            Audience = "aud",
            Issuer = "iss",
            Secret = "sec",
            Minutes = 30,
            DaysToExpiry = 7
        };

        cfg.Should().BeAssignableTo<ITokenConfigurationDto>();
        cfg.Audience.Should().Be("aud");
        cfg.Minutes.Should().Be(30);
    }

    [Fact]
    public void EntityTypeConfigurationConstants_Should_Resolve_MySql_Text()
    {
        EntityTypeConfigurationConstants.GetTypeTextByTypeDataBase(ETypeDataBase.Mysql)
            .Should().Be("text");
        EntityTypeConfigurationConstants.GetMaxLengthByTypeDataBase(ETypeDataBase.Mysql)
            .Should().Be(65535);
    }

    [Fact]
    public void AppWarningException_Should_Carry_Message()
    {
        var ex = new AppWarningException("aviso");

        ex.Message.Should().Be("aviso");
        ex.Should().BeAssignableTo<Exception>();
    }

    [Fact]
    public void AppConfigConstants_Should_Expose_Json_ContentType()
    {
        AppConfigConstants.ApplicationContentJon.Should().Be("application/json");
    }
}

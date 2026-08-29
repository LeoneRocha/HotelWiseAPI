using HotelWise.Core.SDK.Abstractions;
using HotelWise.Core.SDK.Common;
using HotelWise.Core.SDK.Domain;

namespace HotelWise.Core.SDK.Tests.Domain;

public class EntityBaseTests
{
    private sealed class SampleEntity : EntityBase
    {
    }

    private sealed class SampleNamedEntity : EntityBaseWithNameEmail
    {
    }

    [Fact]
    public void EntityBase_Should_Implement_IEntityBase_And_IEntityBaseLog()
    {
        var entity = new SampleEntity
        {
            Id = 10,
            Enable = true,
            CreatedDate = DateTime.UtcNow,
            ModifyDate = DateTime.UtcNow,
            LastAccessDate = DateTime.UtcNow
        };

        entity.Should().BeAssignableTo<IEntityBase>();
        entity.Should().BeAssignableTo<IEntityBaseLog>();
        entity.Id.Should().Be(10);
        entity.Enable.Should().BeTrue();
    }

    [Fact]
    public void EntityBaseWithNameEmail_Should_Expose_Name_And_Email()
    {
        var entity = new SampleNamedEntity
        {
            Name = "HotelWise",
            Email = "core@hotelwise.local"
        };

        entity.Name.Should().Be("HotelWise");
        entity.Email.Should().Be("core@hotelwise.local");
    }
}

public class ServiceResponseTests
{
    [Fact]
    public void ServiceResponse_Should_Default_To_Success()
    {
        var response = new ServiceResponse<string>();

        response.Success.Should().BeTrue();
        response.Message.Should().BeEmpty();
        response.Errors.Should().BeEmpty();
        response.Unauthorized.Should().BeFalse();
        response.Should().BeAssignableTo<IServiceResponse<string>>();
    }

    [Fact]
    public void ServiceResponse_Should_Hold_ErrorResponse_List()
    {
        var response = new ServiceResponse<int>
        {
            Success = false,
            Message = "falha",
            Errors =
            [
                new ErrorResponse { Name = "Id", Message = "obrigatório", ErrorCode = "REQ" }
            ]
        };

        response.Success.Should().BeFalse();
        response.Errors.Should().ContainSingle(e => e.ErrorCode == "REQ");
    }
}

public class IGenericRepositoryContractTests
{
    [Fact]
    public void IGenericRepository_Should_Be_Open_Generic_Interface()
    {
        var type = typeof(IGenericRepository<>);

        type.IsInterface.Should().BeTrue();
        type.IsGenericTypeDefinition.Should().BeTrue();
        type.Namespace.Should().Be("HotelWise.Core.SDK.Abstractions");
    }
}

using HotelWise.Domain.Interfaces.Entity;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Repository;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Service;
using HotelWise.Service.Configure;
using Microsoft.Extensions.DependencyInjection;

namespace HotelWise.Service.Tests.Configure;

public class ServicesDomainRegistrationTests
{
    [Fact]
    public void AddDependenciesManually_Should_Register_Repositories_And_Services()
    {
        var services = new ServiceCollection();

        ServicesDomainRepository.AddDependenciesManually(services);
        ServicesDomainService.AddDependenciesManually(services);

        services.Should().Contain(d => d.ServiceType == typeof(IHotelRepository));
        services.Should().Contain(d => d.ServiceType == typeof(IUserRepository));
        services.Should().Contain(d => d.ServiceType == typeof(IHotelService));
        services.Should().Contain(d => d.ServiceType == typeof(IUserService));
    }

    [Fact]
    public void AddDependenciesManually_Should_Use_Scoped_Lifetime()
    {
        var services = new ServiceCollection();

        ServicesDomainRepository.AddDependenciesManually(services);
        ServicesDomainService.AddDependenciesManually(services);

        services.Where(d =>
                d.ServiceType == typeof(IHotelRepository) ||
                d.ServiceType == typeof(IUserRepository) ||
                d.ServiceType == typeof(IHotelService) ||
                d.ServiceType == typeof(IUserService))
            .Should().OnlyContain(d => d.Lifetime == ServiceLifetime.Scoped);
    }
}

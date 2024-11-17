using HotelWise.Domain.Mapper;
using Microsoft.Extensions.DependencyInjection;

namespace HotelWise.Service.Configure
{
    public static class ServiceCollectionConfigureAutoMapper
    {
        public static void Configure(IServiceCollection services)
        {
            // Auto Mapper 
            services.AddAutoMapper(typeof(AutoMapperProfile));
        }
    }
}

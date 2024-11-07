using HotelWise.Domain.Interfaces;
using HotelWise.Service.AI;
using HotelWise.Service.Entity;
using Microsoft.Extensions.DependencyInjection;

namespace HotelWise.Service.Configure
{
    public static class ConfigureServicesAI
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IAIInferenceAdapterFactory, AIInferenceAdapterFactory>();
            services.AddScoped<IAIInferenceService, AIInferenceService>();
            services.AddScoped<IGenerateHotelService, GenerateHotelService>();
        }
    }
}
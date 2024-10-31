using FluentValidation.AspNetCore;
using HotelWise.Data.Context;
using HotelWise.Data.Repository;
using HotelWise.Domain.Interfaces;
using HotelWise.Service.Entity;
using Microsoft.EntityFrameworkCore;

namespace HotelWise.API
{
    public static class WebApplicationConfigureServiceCollections
    {
        public static void Configure(IServiceCollection services, IConfiguration configuration)
        { 
            
            
            services.AddScoped<IHotelRepository, HotelRepository>();
            services.AddScoped<IHotelService, HotelService>();


            services.AddControllers();
            services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<HotelValidator>());



        }
    }
}
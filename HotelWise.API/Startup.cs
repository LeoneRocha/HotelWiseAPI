using FluentValidation.AspNetCore;
using HotelWise.Data.Context;
using HotelWise.Data.Repository;
using HotelWise.Domain.Interfaces;
using HotelWise.Domain.Validator;
using HotelWise.Service.Entity;
using Microsoft.EntityFrameworkCore;

namespace HotelWise.API
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<HotelWiseDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IHotelRepository, HotelRepository>();
            services.AddScoped<HotelService>();
            services.AddControllers();
            services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<HotelValidator>());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

}

using Microsoft.Extensions.DependencyInjection;

namespace HotelWise.Service.Configure
{
    public static class ServiceCollectionConfigureCors
    {
        public static void Configure(IServiceCollection services)
        {
            addCors(services);
        }
        private static void addCors(IServiceCollection services)
        {
#pragma warning disable S5122 // Disabling Sonar warning for CORS
            services.AddCors(options => options.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .WithExposedHeaders("Content-Disposition");
            }));
#pragma warning restore S5122

        }
    }
}

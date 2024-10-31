using HotelWise.Data.Context;
using HotelWise.Domain.Helpers;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace HotelWise.API.Configure
{
    public static class WebApplicationConfigureBuilder
    {
        public static (WebApplicationBuilder, Serilog.Core.Logger?) CreateHostBuilder(string[] args)
        {
            Serilog.Core.Logger? _logger;
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            _logger = LogAppHelper.CreateLogger(builder.Configuration);

            //Service Collections.
            WebApplicationConfigureServiceCollections.Configure(builder.Services, builder.Configuration, _logger);

            builder.Host.UseSerilog();
            return (builder, _logger);
        }

        public static void BuildAndRunAPP(WebApplicationBuilder builder, Serilog.Core.Logger? _logger)
        {
            if (_logger != null)
            {
                try
                {
                    LogAppHelper.Set_ASPNETCORE_ENVIRONMENT(builder.Configuration);

                    var app = builder.Build();

                    //Application Builder
                    Configure(app, builder.Environment, builder.Configuration);

                    LogAppHelper.PrintLogInformationVersionProduct(_logger);

                    _logger.Information("Web API Loading at: {time}", DateTime.UtcNow);
                      
                    app.Run();
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Web API Error Loading at: {Message} at: {time}", ex.Message, DateTime.UtcNow);
                }
            }
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration)
        {
            // Configure the HTTP request pipeline.
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // Migrate latest database changes during startup
            addAutoMigrate(app);

            app.UseHttpsRedirection();
              
            app.UseRouting();

            app.UseCors();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "HotelWise.API v1");
                //c.RoutePrefix = string.Empty; // Para acessar o Swagger na raiz do aplicativo
            }
            );

            var option = new RewriteOptions();
            option.AddRedirect("^$", "swagger");

            app.UseRewriter(option);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //HyperMedia
                endpoints.MapControllerRoute("DefaultApi", "{controller=values}/{id?}");
            });

        }


        private static void addAutoMigrate(IApplicationBuilder app)
        {
            // Migrate latest database changes during startup
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<HotelWiseDbContextMysql>())
                {
                    context?.Database.Migrate();
                }
            }
        }
    }
}
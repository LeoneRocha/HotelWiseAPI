using HotelWise.Data.Context;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace HotelWise.API.Configure;

/// <summary>
/// Construtor e inicializador da infraestrutura de hospedagem da aplicação ASP.NET Core e do pipeline HTTP.
/// </summary>
public static class WebApplicationConfigureBuilder
{
    /// <summary>
    /// Cria e configura o <see cref="WebApplicationBuilder"/> carregando configurações de ambiente e inicializando o Serilog.
    /// </summary>
    /// <param name="args">Argumentos da linha de comando.</param>
    /// <returns>Tupla contendo a instância do builder e o logger raiz.</returns>
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

    /// <summary>
    /// Constrói a aplicação Web API, configura o pipeline de requisições e inicia a escuta HTTP/HTTPS.
    /// </summary>
    /// <param name="builder">Instância do WebApplicationBuilder.</param>
    /// <param name="_logger">Logger raiz do Serilog.</param>
    public static void BuildAndRunAPP(WebApplicationBuilder builder, Serilog.Core.Logger? _logger)
    {
        if (_logger == null)
        {
            throw new InvalidOperationException("Logger Serilog não foi inicializado. Verifique a seção Serilog em appsettings.");
        }

        try
        {
            LogAppHelper.Set_ASPNETCORE_ENVIRONMENT(builder.Configuration);

            var app = builder.Build();

            Configure(app, builder.Environment, builder.Configuration);

            LogAppHelper.PrintLogInformationVersionProduct(_logger);

            _logger.Information("Web API Loading at: {Time}", DateTime.UtcNow);

            app.Run();
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Web API Error Loading at: {Message} at: {Time}", ex.Message, DateTime.UtcNow);
            Console.Error.WriteLine($"FATAL STARTUP: {ex}");
            throw new InvalidOperationException(
                "Web API failed during startup (BuildAndRunAPP). See inner exception for details.", ex);
        }
    }

    /// <summary>
    /// Configura o pipeline de middlewares HTTP (middlewares customizados, logging, Swagger, CORS, autenticação e rotas).
    /// </summary>
    /// <param name="app">Construtor da aplicação (IApplicationBuilder).</param>
    /// <param name="env">Ambiente de hospedagem web.</param>
    /// <param name="configuration">Configurações globais da aplicação.</param>
    public static void Configure(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration)
    {
        app.UseMiddleware<CorrelationIdMiddleware>();

        app.UseSerilogRequestLogging(options =>
        {
            options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
            {
                var correlationId = httpContext.Items[CorrelationIdMiddleware.ItemKey]?.ToString()
                    ?? httpContext.TraceIdentifier;
                diagnosticContext.Set("CorrelationId", correlationId);
            };
            options.GetLevel = (httpContext, elapsed, ex) =>
            {
                if (ex != null || httpContext.Response.StatusCode >= 500)
                {
                    return Serilog.Events.LogEventLevel.Error;
                }

                if (httpContext.Response.StatusCode >= 400)
                {
                    return Serilog.Events.LogEventLevel.Warning;
                }

                return Serilog.Events.LogEventLevel.Information;
            };
        });

        app.UseMiddleware<GlobalExceptionMiddleware>();
        app.UseMiddleware<RequestLoggingMiddleware>();

        // Migrate latest database changes during startup
        addAutoMigrate(app);

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseCors("AllowAnyOrigin");

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "HotelWise.API v1");
        });

        var option = new RewriteOptions();
        option.AddRedirect("^$", "swagger");

        app.UseRewriter(option);

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHealthChecks("/health");
            endpoints.MapControllerRoute("DefaultApi", "{controller=values}/{id?}");
        });
    }

    /// <summary>
    /// Aplica as migrações pendentes do Entity Framework Core automaticamente durante a inicialização.
    /// </summary>
    private static void addAutoMigrate(IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
        using var context = serviceScope.ServiceProvider.GetService<HotelWiseDbContextMysql>();
        if (context == null)
        {
            return;
        }

        try
        {
            context.Database.Migrate();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Falha ao aplicar migrations no startup. Verifique ConnectionStrings:DBConnectionMySQL e acesso ao MySQL.");
            throw new InvalidOperationException(
                "Database migration failed during application startup. See inner exception for details.", ex);
        }
    }
}


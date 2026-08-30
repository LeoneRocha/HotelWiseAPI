using HotelWise.Domain.Dto.IA.SemanticKernel;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Service;
using HotelWise.Service.AI;
using HotelWise.Service.Entity;
using Microsoft.Extensions.DependencyInjection;

namespace HotelWise.Service.Configure;

/// <summary>
/// Configuração de injeção de dependência para serviços de Inteligência Artificial do domínio hoteleiro.
/// </summary>
public static class ConfigureServicesAI
{
    /// <summary>
    /// Registra os serviços genéricos de IA do SDK e as implementações específicas do host (gerador de hotéis, vector store e assistente).
    /// </summary>
    /// <param name="services">Coleção de serviços da aplicação.</param>
    public static void ConfigureServices(IServiceCollection services)
    {
        HotelWise.Core.SDK.AI.Configure.ConfigureServicesAI.RegisterGenericAiServices(services);

        services.AddScoped<IGenerateHotelService, GenerateHotelService>();
        services.AddScoped<IVectorStoreService<HotelVector>, HotelVectorStoreService>();
        services.AddScoped<IAssistantService, AssistantService>();
    }
}


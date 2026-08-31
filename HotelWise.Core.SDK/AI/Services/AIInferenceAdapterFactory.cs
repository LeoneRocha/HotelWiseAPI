#if NET8_0_OR_GREATER
using HotelWise.Core.SDK.AI.Abstractions;
using HotelWise.Core.SDK.AI.Adapters;
using HotelWise.Core.SDK.AI.Enums;

namespace HotelWise.Core.SDK.AI.Services;

/// <summary>
/// Fábrica de adapters de inferência LLM.
/// Implementa <see cref="IAIInferenceAdapterFactory"/> resolvendo Groq, Mistral,
/// Ollama ou Semantic Kernel conforme <see cref="InferenceAiAdapterType"/>.
/// </summary>
/// <example>
/// <code>
/// // Registro DI
/// services.AddScoped&lt;IAIInferenceAdapterFactory, AIInferenceAdapterFactory&gt;();
///
/// // Uso
/// var adapter = factory.CreateAdapter(InferenceAiAdapterType.SemanticKernel);
/// string reply = await adapter.GenerateChatCompletionAsync(messages);
/// </code>
/// </example>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Service. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Service.AI.Services.AIInferenceAdapterFactory. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class AIInferenceAdapterFactory : IAIInferenceAdapterFactory
{
    /// <summary>
    /// Configuração agregada de IA.
    /// </summary>
    private readonly IApplicationIAConfig _applicationConfig;

    /// <summary>
    /// Provedor de serviços (necessário para Semantic Kernel).
    /// </summary>
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Inicializa a fábrica com configuração de IA e provedor de serviços.
    /// </summary>
    /// <param name="applicationConfig">Configuração agregada de IA.</param>
    /// <param name="serviceProvider">Provedor de serviços da aplicação.</param>
    public AIInferenceAdapterFactory(IApplicationIAConfig applicationConfig, IServiceProvider serviceProvider)
    {
        _applicationConfig = applicationConfig;
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Cria o adapter de inferência correspondente ao tipo informado.
    /// Tipos desconhecidos caem no fallback <see cref="GroqApiAdapter"/>.
    /// </summary>
    /// <param name="eIAInferenceAdapterType">Tipo do adapter solicitado.</param>
    /// <returns>Instância de <see cref="IAIInferenceAdapter"/>.</returns>
    /// <example>
    /// <code>
    /// IAIInferenceAdapter adapter = factory.CreateAdapter(InferenceAiAdapterType.Ollama);
    /// </code>
    /// </example>
    public IAIInferenceAdapter CreateAdapter(InferenceAiAdapterType eIAInferenceAdapterType)
    {
        return eIAInferenceAdapterType switch
        {
            InferenceAiAdapterType.GroqApi => new GroqApiAdapter(_applicationConfig),
            InferenceAiAdapterType.Mistral => new MistralApiAdapter(_applicationConfig),
            InferenceAiAdapterType.Ollama => new OllamaAdapter(_applicationConfig),
            InferenceAiAdapterType.SemanticKernel => new SemanticKernelAdapter(_applicationConfig, _serviceProvider),
            _ => new GroqApiAdapter(_applicationConfig)
        };
    }
}
#endif

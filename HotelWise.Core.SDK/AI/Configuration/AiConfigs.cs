using HotelWise.Core.SDK.AI.Abstractions;

using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.AI.Configuration;

/// <summary>
/// Base de configuração de serviços de inferência IA.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Domain.AI.Configuration.AiInferenceConfigBase", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Domain.AI.Configuration.AiInferenceConfigBase em SmartCoreHub.Core.SDK.")]
public abstract class AiInferenceConfigBase
    : SmartCoreHub.Core.SDK.Domain.AI.Configuration.AiInferenceConfigBase, IAiInferenceConfigBase
{
}

/// <summary>
/// Configurações para o provedor Azure OpenAI.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Domain.AI.Configuration.AzureOpenAIConfig", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Domain.AI.Configuration.AzureOpenAIConfig em SmartCoreHub.Core.SDK.")]
public class AzureOpenAIConfig : SmartCoreHub.Core.SDK.Domain.AI.Configuration.AzureOpenAIConfig, IAiInferenceConfigBase
{
}

/// <summary>
/// Configurações de embeddings para o provedor Azure OpenAI.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Domain.AI.Configuration.AzureOpenAIEmbeddingsConfig", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Domain.AI.Configuration.AzureOpenAIEmbeddingsConfig em SmartCoreHub.Core.SDK.")]
public class AzureOpenAIEmbeddingsConfig : SmartCoreHub.Core.SDK.Domain.AI.Configuration.AzureOpenAIEmbeddingsConfig, IAiInferenceConfigBase
{
}

/// <summary>
/// Configurações para o provedor OpenAI direto.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Domain.AI.Configuration.OpenAIConfig", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Domain.AI.Configuration.OpenAIConfig em SmartCoreHub.Core.SDK.")]
public class OpenAIConfig : SmartCoreHub.Core.SDK.Domain.AI.Configuration.OpenAIConfig, IAiInferenceConfigBase
{
}

/// <summary>
/// Configurações de embeddings para o provedor OpenAI direto.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Domain.AI.Configuration.OpenAIEmbeddingsConfig", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Domain.AI.Configuration.OpenAIEmbeddingsConfig em SmartCoreHub.Core.SDK.")]
public class OpenAIEmbeddingsConfig : SmartCoreHub.Core.SDK.Domain.AI.Configuration.OpenAIEmbeddingsConfig, IAiInferenceConfigBase
{
}

/// <summary>
/// Configurações para a API do provedor Mistral.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Domain.AI.Configuration.MistralApiConfig", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Domain.AI.Configuration.MistralApiConfig em SmartCoreHub.Core.SDK.")]
public class MistralApiConfig : SmartCoreHub.Core.SDK.Domain.AI.Configuration.MistralApiConfig, IAiInferenceConfigBase
{
}

/// <summary>
/// Configurações de embeddings para a API do provedor Mistral.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Domain.AI.Configuration.MistralApiEmbeddingsConfig", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Domain.AI.Configuration.MistralApiEmbeddingsConfig em SmartCoreHub.Core.SDK.")]
public class MistralApiEmbeddingsConfig : SmartCoreHub.Core.SDK.Domain.AI.Configuration.MistralApiEmbeddingsConfig, IAiInferenceConfigBase
{
}

/// <summary>
/// Configurações para a API do provedor Groq.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Domain.AI.Configuration.GroqApiConfig", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Domain.AI.Configuration.GroqApiConfig em SmartCoreHub.Core.SDK.")]
public class GroqApiConfig : SmartCoreHub.Core.SDK.Domain.AI.Configuration.GroqApiConfig, IAiInferenceConfigBase
{
}

/// <summary>
/// Configurações para o provedor local Ollama.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Domain.AI.Configuration.OllamaConfig", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Domain.AI.Configuration.OllamaConfig em SmartCoreHub.Core.SDK.")]
public class OllamaConfig : SmartCoreHub.Core.SDK.Domain.AI.Configuration.OllamaConfig, IAiInferenceConfigBase
{
}

/// <summary>
/// Configurações para o serviço Azure AI Search (vector store / hybrid search).
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Domain.AI.Configuration.AzureAISearchConfig", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Domain.AI.Configuration.AzureAISearchConfig em SmartCoreHub.Core.SDK.")]
public class AzureAISearchConfig : SmartCoreHub.Core.SDK.Domain.AI.Configuration.AzureAISearchConfig, IAiInferenceConfigBase
{
}

/// <summary>
/// Configurações para o vector store Weaviate.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Domain.AI.Configuration.WeaviateConfig", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Domain.AI.Configuration.WeaviateConfig em SmartCoreHub.Core.SDK.")]
public class WeaviateConfig : SmartCoreHub.Core.SDK.Domain.AI.Configuration.WeaviateConfig, IAiInferenceConfigBase
{
}

/// <summary>
/// Configurações para instâncias do Azure Cosmos DB.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Domain.AI.Configuration.AzureCosmosDBConfig", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Domain.AI.Configuration.AzureCosmosDBConfig em SmartCoreHub.Core.SDK.")]
public class AzureCosmosDBConfig : SmartCoreHub.Core.SDK.Domain.AI.Configuration.AzureCosmosDBConfig
{
}

/// <summary>
/// Configurações para o vector store Qdrant.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Domain.AI.Configuration.QdrantConfig", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Domain.AI.Configuration.QdrantConfig em SmartCoreHub.Core.SDK.")]
public class QdrantConfig : SmartCoreHub.Core.SDK.Domain.AI.Configuration.QdrantConfig
{
}

/// <summary>
/// Configurações para o cache/vector store Redis.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Domain.AI.Configuration.RedisConfig", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Domain.AI.Configuration.RedisConfig em SmartCoreHub.Core.SDK.")]
public class RedisConfig : SmartCoreHub.Core.SDK.Domain.AI.Configuration.RedisConfig
{
}

/// <summary>
/// Configurações de autenticação Azure AD / Microsoft Entra ID.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Domain.AI.Configuration.AzureAdConfig", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Domain.AI.Configuration.AzureAdConfig em SmartCoreHub.Core.SDK.")]
public class AzureAdConfig : SmartCoreHub.Core.SDK.Domain.AI.Configuration.AzureAdConfig, IAzureAdConfig
{
}

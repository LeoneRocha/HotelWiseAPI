using HotelWise.Core.SDK.AI.Abstractions;

namespace HotelWise.Core.SDK.AI.Configuration;

/// <summary>
/// Base de configuração de serviços de inferência IA.
/// </summary>
public abstract class AiInferenceConfigBase
    : SmartCoreHub.Core.SDK.Domain.AI.Configuration.AiInferenceConfigBase, IAiInferenceConfigBase
{
}

/// <summary>
/// Configurações para o provedor Azure OpenAI.
/// </summary>
public class AzureOpenAIConfig : SmartCoreHub.Core.SDK.Domain.AI.Configuration.AzureOpenAIConfig, IAiInferenceConfigBase
{
}

/// <summary>
/// Configurações de embeddings para o provedor Azure OpenAI.
/// </summary>
public class AzureOpenAIEmbeddingsConfig : SmartCoreHub.Core.SDK.Domain.AI.Configuration.AzureOpenAIEmbeddingsConfig, IAiInferenceConfigBase
{
}

/// <summary>
/// Configurações para o provedor OpenAI direto.
/// </summary>
public class OpenAIConfig : SmartCoreHub.Core.SDK.Domain.AI.Configuration.OpenAIConfig, IAiInferenceConfigBase
{
}

/// <summary>
/// Configurações de embeddings para o provedor OpenAI direto.
/// </summary>
public class OpenAIEmbeddingsConfig : SmartCoreHub.Core.SDK.Domain.AI.Configuration.OpenAIEmbeddingsConfig, IAiInferenceConfigBase
{
}

/// <summary>
/// Configurações para a API do provedor Mistral.
/// </summary>
public class MistralApiConfig : SmartCoreHub.Core.SDK.Domain.AI.Configuration.MistralApiConfig, IAiInferenceConfigBase
{
}

/// <summary>
/// Configurações de embeddings para a API do provedor Mistral.
/// </summary>
public class MistralApiEmbeddingsConfig : SmartCoreHub.Core.SDK.Domain.AI.Configuration.MistralApiEmbeddingsConfig, IAiInferenceConfigBase
{
}

/// <summary>
/// Configurações para a API do provedor Groq.
/// </summary>
public class GroqApiConfig : SmartCoreHub.Core.SDK.Domain.AI.Configuration.GroqApiConfig, IAiInferenceConfigBase
{
}

/// <summary>
/// Configurações para o provedor local Ollama.
/// </summary>
public class OllamaConfig : SmartCoreHub.Core.SDK.Domain.AI.Configuration.OllamaConfig, IAiInferenceConfigBase
{
}

/// <summary>
/// Configurações para o serviço Azure AI Search (vector store / hybrid search).
/// </summary>
public class AzureAISearchConfig : SmartCoreHub.Core.SDK.Domain.AI.Configuration.AzureAISearchConfig, IAiInferenceConfigBase
{
}

/// <summary>
/// Configurações para o vector store Weaviate.
/// </summary>
public class WeaviateConfig : SmartCoreHub.Core.SDK.Domain.AI.Configuration.WeaviateConfig, IAiInferenceConfigBase
{
}

/// <summary>
/// Configurações para instâncias do Azure Cosmos DB.
/// </summary>
public class AzureCosmosDBConfig : SmartCoreHub.Core.SDK.Domain.AI.Configuration.AzureCosmosDBConfig
{
}

/// <summary>
/// Configurações para o vector store Qdrant.
/// </summary>
public class QdrantConfig : SmartCoreHub.Core.SDK.Domain.AI.Configuration.QdrantConfig
{
}

/// <summary>
/// Configurações para o cache/vector store Redis.
/// </summary>
public class RedisConfig : SmartCoreHub.Core.SDK.Domain.AI.Configuration.RedisConfig
{
}

/// <summary>
/// Configurações de autenticação Azure AD / Microsoft Entra ID.
/// </summary>
public class AzureAdConfig : SmartCoreHub.Core.SDK.Domain.AI.Configuration.AzureAdConfig, IAzureAdConfig
{
}

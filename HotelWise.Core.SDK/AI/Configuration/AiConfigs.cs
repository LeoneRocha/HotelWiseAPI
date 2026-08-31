using HotelWise.Core.SDK.AI.Abstractions;

namespace HotelWise.Core.SDK.AI.Configuration;

/// <summary>
/// Base de configuração de serviços de inferência IA.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Configuration.AiInferenceConfigBase. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public abstract class AiInferenceConfigBase
    : SmartCoreHub.Core.SDK.Domain.AI.Configuration.AiInferenceConfigBase, IAiInferenceConfigBase
{
}

/// <summary>
/// Configurações para o provedor Azure OpenAI.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Configuration.AzureOpenAIConfig. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class AzureOpenAIConfig : SmartCoreHub.Core.SDK.Domain.AI.Configuration.AzureOpenAIConfig, IAiInferenceConfigBase
{
}

/// <summary>
/// Configurações de embeddings para o provedor Azure OpenAI.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Configuration.AzureOpenAIEmbeddingsConfig. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class AzureOpenAIEmbeddingsConfig : SmartCoreHub.Core.SDK.Domain.AI.Configuration.AzureOpenAIEmbeddingsConfig, IAiInferenceConfigBase
{
}

/// <summary>
/// Configurações para o provedor OpenAI direto.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Configuration.OpenAIConfig. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class OpenAIConfig : SmartCoreHub.Core.SDK.Domain.AI.Configuration.OpenAIConfig, IAiInferenceConfigBase
{
}

/// <summary>
/// Configurações de embeddings para o provedor OpenAI direto.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Configuration.OpenAIEmbeddingsConfig. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class OpenAIEmbeddingsConfig : SmartCoreHub.Core.SDK.Domain.AI.Configuration.OpenAIEmbeddingsConfig, IAiInferenceConfigBase
{
}

/// <summary>
/// Configurações para a API do provedor Mistral.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Configuration.MistralApiConfig. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class MistralApiConfig : SmartCoreHub.Core.SDK.Domain.AI.Configuration.MistralApiConfig, IAiInferenceConfigBase
{
}

/// <summary>
/// Configurações de embeddings para a API do provedor Mistral.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Configuration.MistralApiEmbeddingsConfig. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class MistralApiEmbeddingsConfig : SmartCoreHub.Core.SDK.Domain.AI.Configuration.MistralApiEmbeddingsConfig, IAiInferenceConfigBase
{
}

/// <summary>
/// Configurações para a API do provedor Groq.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Configuration.GroqApiConfig. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class GroqApiConfig : SmartCoreHub.Core.SDK.Domain.AI.Configuration.GroqApiConfig, IAiInferenceConfigBase
{
}

/// <summary>
/// Configurações para o provedor local Ollama.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Configuration.OllamaConfig. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class OllamaConfig : SmartCoreHub.Core.SDK.Domain.AI.Configuration.OllamaConfig, IAiInferenceConfigBase
{
}

/// <summary>
/// Configurações para o serviço Azure AI Search (vector store / hybrid search).
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Configuration.AzureAISearchConfig. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class AzureAISearchConfig : SmartCoreHub.Core.SDK.Domain.AI.Configuration.AzureAISearchConfig, IAiInferenceConfigBase
{
}

/// <summary>
/// Configurações para o vector store Weaviate.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Configuration.WeaviateConfig. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class WeaviateConfig : SmartCoreHub.Core.SDK.Domain.AI.Configuration.WeaviateConfig, IAiInferenceConfigBase
{
}

/// <summary>
/// Configurações para instâncias do Azure Cosmos DB.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Configuration.AzureCosmosDBConfig. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class AzureCosmosDBConfig : SmartCoreHub.Core.SDK.Domain.AI.Configuration.AzureCosmosDBConfig
{
}

/// <summary>
/// Configurações para o vector store Qdrant.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Configuration.QdrantConfig. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class QdrantConfig : SmartCoreHub.Core.SDK.Domain.AI.Configuration.QdrantConfig
{
}

/// <summary>
/// Configurações para o cache/vector store Redis.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Configuration.RedisConfig. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class RedisConfig : SmartCoreHub.Core.SDK.Domain.AI.Configuration.RedisConfig
{
}

/// <summary>
/// Configurações de autenticação Azure AD / Microsoft Entra ID.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Configuration.AzureAdConfig. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class AzureAdConfig : SmartCoreHub.Core.SDK.Domain.AI.Configuration.AzureAdConfig, IAzureAdConfig
{
}

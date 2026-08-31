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

[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Configuration.AzureOpenAIConfig. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class AzureOpenAIConfig : SmartCoreHub.Core.SDK.Domain.AI.Configuration.AzureOpenAIConfig, IAiInferenceConfigBase
{
}

[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Configuration.AzureOpenAIEmbeddingsConfig. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class AzureOpenAIEmbeddingsConfig : SmartCoreHub.Core.SDK.Domain.AI.Configuration.AzureOpenAIEmbeddingsConfig, IAiInferenceConfigBase
{
}

[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Configuration.OpenAIConfig. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class OpenAIConfig : SmartCoreHub.Core.SDK.Domain.AI.Configuration.OpenAIConfig, IAiInferenceConfigBase
{
}

[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Configuration.OpenAIEmbeddingsConfig. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class OpenAIEmbeddingsConfig : SmartCoreHub.Core.SDK.Domain.AI.Configuration.OpenAIEmbeddingsConfig, IAiInferenceConfigBase
{
}

[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Configuration.MistralApiConfig. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class MistralApiConfig : SmartCoreHub.Core.SDK.Domain.AI.Configuration.MistralApiConfig, IAiInferenceConfigBase
{
}

[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Configuration.MistralApiEmbeddingsConfig. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class MistralApiEmbeddingsConfig : SmartCoreHub.Core.SDK.Domain.AI.Configuration.MistralApiEmbeddingsConfig, IAiInferenceConfigBase
{
}

[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Configuration.GroqApiConfig. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class GroqApiConfig : SmartCoreHub.Core.SDK.Domain.AI.Configuration.GroqApiConfig, IAiInferenceConfigBase
{
}

[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Configuration.OllamaConfig. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class OllamaConfig : SmartCoreHub.Core.SDK.Domain.AI.Configuration.OllamaConfig, IAiInferenceConfigBase
{
}

[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Configuration.AzureAISearchConfig. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class AzureAISearchConfig : SmartCoreHub.Core.SDK.Domain.AI.Configuration.AzureAISearchConfig, IAiInferenceConfigBase
{
}

[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Configuration.WeaviateConfig. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class WeaviateConfig : SmartCoreHub.Core.SDK.Domain.AI.Configuration.WeaviateConfig, IAiInferenceConfigBase
{
}

[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Configuration.AzureCosmosDBConfig. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class AzureCosmosDBConfig : SmartCoreHub.Core.SDK.Domain.AI.Configuration.AzureCosmosDBConfig
{
}

[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Configuration.QdrantConfig. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class QdrantConfig : SmartCoreHub.Core.SDK.Domain.AI.Configuration.QdrantConfig
{
}

[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Configuration.RedisConfig. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class RedisConfig : SmartCoreHub.Core.SDK.Domain.AI.Configuration.RedisConfig
{
}

[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Configuration.AzureAdConfig. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class AzureAdConfig : SmartCoreHub.Core.SDK.Domain.AI.Configuration.AzureAdConfig, IAzureAdConfig
{
}

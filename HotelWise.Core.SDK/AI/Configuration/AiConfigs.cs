using System.ComponentModel.DataAnnotations;
using HotelWise.Core.SDK.AI.Abstractions;

namespace HotelWise.Core.SDK.AI.Configuration;

/// <summary>
/// Base de configuração de serviços de inferência IA.
/// Implementa <see cref="IAiInferenceConfigBase"/> com endpoint, chave e modelos
/// compartilhados por provedores de chat e embeddings.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Configuration.AiInferenceConfigBase. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public abstract class AiInferenceConfigBase : IAiInferenceConfigBase
{
    /// <summary>
    /// Nome da seção de configuração no appsettings (definido pelas subclasses).
    /// </summary>
    [Required]
    public static string ConfigSectionName { get; protected set; } = string.Empty;

    /// <summary>
    /// Endpoint HTTP do serviço de chat/completions.
    /// </summary>
    public string Endpoint { get; set; } = string.Empty;

    /// <summary>
    /// Chave de API do provedor.
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    /// Identificador do modelo de chat.
    /// </summary>
    public string ModelId { get; set; } = string.Empty;

    /// <summary>
    /// Identificador da organização (quando exigido pelo provedor).
    /// </summary>
    public string? OrgId { get; set; }

    /// <summary>
    /// Endpoint HTTP do serviço de embeddings.
    /// </summary>
    public string EndpointEmbeddings { get; set; } = string.Empty;

    /// <summary>
    /// Identificador do modelo de embeddings.
    /// </summary>
    public string ModelIdEmbeddings { get; set; } = string.Empty;
}

/// <summary>
/// Configuração do serviço de chat Azure OpenAI (seção <c>AzureOpenAI</c>).
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Configuration.AzureOpenAIConfig. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class AzureOpenAIConfig : AiInferenceConfigBase
{
    /// <summary>
    /// Nome da seção de configuração no appsettings.
    /// </summary>
    public new static string ConfigSectionName => "AzureOpenAI";

    /// <summary>
    /// Nome do deployment de chat no Azure OpenAI.
    /// </summary>
    [Required]
    public string ChatDeploymentName { get; set; } = string.Empty;
}

/// <summary>
/// Configuração do serviço de embeddings Azure OpenAI (seção <c>AzureOpenAIEmbeddings</c>).
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Configuration.AzureOpenAIEmbeddingsConfig. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class AzureOpenAIEmbeddingsConfig : AiInferenceConfigBase
{
    /// <summary>
    /// Nome da seção de configuração no appsettings.
    /// </summary>
    public new static string ConfigSectionName => "AzureOpenAIEmbeddings";

    /// <summary>
    /// Nome do deployment de embeddings no Azure OpenAI.
    /// </summary>
    [Required]
    public string DeploymentName { get; set; } = string.Empty;
}

/// <summary>
/// Configuração do serviço de chat OpenAI (seção <c>OpenAI</c>).
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Configuration.OpenAIConfig. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class OpenAIConfig : AiInferenceConfigBase
{
    /// <summary>
    /// Nome da seção de configuração no appsettings.
    /// </summary>
    public new static string ConfigSectionName => "OpenAI";
}

/// <summary>
/// Configuração do serviço de embeddings OpenAI (seção <c>OpenAIEmbeddings</c>).
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Configuration.OpenAIEmbeddingsConfig. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class OpenAIEmbeddingsConfig : AiInferenceConfigBase
{
    /// <summary>
    /// Nome da seção de configuração no appsettings.
    /// </summary>
    public new static string ConfigSectionName => "OpenAIEmbeddings";
}

/// <summary>
/// Configuração do serviço de chat Mistral API (seção <c>MistralApi</c>).
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Configuration.MistralApiConfig. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class MistralApiConfig : AiInferenceConfigBase
{
    /// <summary>
    /// Nome da seção de configuração no appsettings.
    /// </summary>
    public new static string ConfigSectionName => "MistralApi";
}

/// <summary>
/// Configuração do serviço de embeddings Mistral API (seção <c>MistralApiEmbeddings</c>).
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Configuration.MistralApiEmbeddingsConfig. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class MistralApiEmbeddingsConfig : AiInferenceConfigBase
{
    /// <summary>
    /// Nome da seção de configuração no appsettings.
    /// </summary>
    public new static string ConfigSectionName => "MistralApiEmbeddings";
}

/// <summary>
/// Configuração do serviço de chat Groq API (seção <c>GroqApi</c>).
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Configuration.GroqApiConfig. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class GroqApiConfig : AiInferenceConfigBase
{
    /// <summary>
    /// Nome da seção de configuração no appsettings.
    /// </summary>
    public new static string ConfigSectionName => "GroqApi";
}

/// <summary>
/// Configuração do serviço Ollama (seção <c>OllamaApi</c>), incluindo parâmetros de sampling.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Configuration.OllamaConfig. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class OllamaConfig : AiInferenceConfigBase
{
    /// <summary>
    /// Nome da seção de configuração no appsettings.
    /// </summary>
    public new static string ConfigSectionName => "OllamaApi";

    /// <summary>
    /// Número máximo de tokens a prever na geração.
    /// </summary>
    public int NumPredict { get; set; } = 500;

    /// <summary>
    /// Temperatura de sampling (0 = determinístico).
    /// </summary>
    public float Temperature { get; set; } = 0.0f;

    /// <summary>
    /// Nucleus sampling (top-p).
    /// </summary>
    public float TopP { get; set; } = 1.0f;

    /// <summary>
    /// Semente aleatória para reprodutibilidade, quando suportada.
    /// </summary>
    public int? Seed { get; set; } = 32;
}

/// <summary>
/// Configuração do vector store Azure AI Search (seção <c>AzureAISearch</c>).
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Configuration.AzureAISearchConfig. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class AzureAISearchConfig : AiInferenceConfigBase
{
    /// <summary>
    /// Nome da seção de configuração no appsettings.
    /// </summary>
    public new static string ConfigSectionName => "AzureAISearch";
}

/// <summary>
/// Configuração do vector store Weaviate (seção <c>Weaviate</c>).
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Configuration.WeaviateConfig. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class WeaviateConfig : AiInferenceConfigBase
{
    /// <summary>
    /// Nome da seção de configuração no appsettings.
    /// </summary>
    public new static string ConfigSectionName => "Weaviate";
}

/// <summary>
/// Configuração do Azure Cosmos DB como vector store (MongoDB ou NoSQL).
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Configuration.AzureCosmosDBConfig. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class AzureCosmosDBConfig
{
    /// <summary>
    /// Nome da seção de configuração para Cosmos DB API MongoDB.
    /// </summary>
    public const string MongoDBConfigSectionName = "AzureCosmosDBMongoDB";

    /// <summary>
    /// Nome da seção de configuração para Cosmos DB API NoSQL.
    /// </summary>
    public const string NoSQLConfigSectionName = "AzureCosmosDBNoSQL";

    /// <summary>
    /// Connection string do Cosmos DB.
    /// </summary>
    [Required]
    public string ConnectionString { get; set; } = string.Empty;

    /// <summary>
    /// Nome do banco de dados.
    /// </summary>
    [Required]
    public string DatabaseName { get; set; } = string.Empty;
}

/// <summary>
/// Configuração do vector store Qdrant (seção <c>Qdrant</c>).
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Configuration.QdrantConfig. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class QdrantConfig
{
    /// <summary>
    /// Nome da seção de configuração no appsettings.
    /// </summary>
    public const string ConfigSectionName = "Qdrant";

    /// <summary>
    /// Host do servidor Qdrant.
    /// </summary>
    [Required]
    public string Host { get; set; } = string.Empty;

    /// <summary>
    /// Porta do servidor Qdrant (padrão 6334 gRPC).
    /// </summary>
    public int Port { get; set; } = 6334;

    /// <summary>
    /// Indica se a conexão usa HTTPS.
    /// </summary>
    public bool Https { get; set; }

    /// <summary>
    /// Chave de API do Qdrant, quando autenticado.
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;
}

/// <summary>
/// Configuração do vector store Redis (seção <c>Redis</c>).
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Configuration.RedisConfig. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class RedisConfig
{
    /// <summary>
    /// Nome da seção de configuração no appsettings.
    /// </summary>
    public const string ConfigSectionName = "Redis";

    /// <summary>
    /// String de conexão Redis (host, porta, senha, etc.).
    /// </summary>
    [Required]
    public string ConnectionConfiguration { get; set; } = string.Empty;
}

/// <summary>
/// Configuração Azure AD / Microsoft Entra ID implementando <see cref="IAzureAdConfig"/>.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Configuration.AzureAdConfig. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class AzureAdConfig : IAzureAdConfig
{
    /// <summary>
    /// URL base da instância do Entra ID.
    /// </summary>
    public string Instance { get; set; } = string.Empty;

    /// <summary>
    /// Domínio do diretório Azure AD.
    /// </summary>
    public string Domain { get; set; } = string.Empty;

    /// <summary>
    /// Identificador do tenant.
    /// </summary>
    public string TenantId { get; set; } = string.Empty;

    /// <summary>
    /// Identificador do aplicativo (Client ID).
    /// </summary>
    public string ClientId { get; set; } = string.Empty;

    /// <summary>
    /// Audience esperada nos tokens JWT.
    /// </summary>
    public string Audience { get; set; } = string.Empty;

    /// <summary>
    /// Segredo do cliente.
    /// </summary>
    public string ClientSecret { get; set; } = string.Empty;

    /// <summary>
    /// Caminho de callback após autenticação.
    /// </summary>
    public string CallbackPath { get; set; } = string.Empty;

    /// <summary>
    /// Caminho de callback após sign-out.
    /// </summary>
    public string SignedOutCallbackPath { get; set; } = string.Empty;

    /// <summary>
    /// Escopos OAuth solicitados.
    /// </summary>
    public string Scopes { get; set; } = string.Empty;
}

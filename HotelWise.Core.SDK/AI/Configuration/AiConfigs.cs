using System.ComponentModel.DataAnnotations;
using HotelWise.Core.SDK.AI.Abstractions;

namespace HotelWise.Core.SDK.AI.Configuration;

/// <summary>
/// Base de configuração de serviços de inferência IA.
/// </summary>
public abstract class AiInferenceConfigBase : IAiInferenceConfigBase
{
    [Required]
    public static string ConfigSectionName { get; protected set; } = string.Empty;

    public string Endpoint { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public string ModelId { get; set; } = string.Empty;
    public string? OrgId { get; set; }
    public string EndpointEmbeddings { get; set; } = string.Empty;
    public string ModelIdEmbeddings { get; set; } = string.Empty;
}

public class AzureOpenAIConfig : AiInferenceConfigBase
{
    public new static string ConfigSectionName => "AzureOpenAI";
    [Required]
    public string ChatDeploymentName { get; set; } = string.Empty;
}

public class AzureOpenAIEmbeddingsConfig : AiInferenceConfigBase
{
    public new static string ConfigSectionName => "AzureOpenAIEmbeddings";
    [Required]
    public string DeploymentName { get; set; } = string.Empty;
}

public class OpenAIConfig : AiInferenceConfigBase
{
    public new static string ConfigSectionName => "OpenAI";
}

public class OpenAIEmbeddingsConfig : AiInferenceConfigBase
{
    public new static string ConfigSectionName => "OpenAIEmbeddings";
}

public class MistralApiConfig : AiInferenceConfigBase
{
    public new static string ConfigSectionName => "MistralApi";
}

public class MistralApiEmbeddingsConfig : AiInferenceConfigBase
{
    public new static string ConfigSectionName => "MistralApiEmbeddings";
}

public class GroqApiConfig : AiInferenceConfigBase
{
    public new static string ConfigSectionName => "GroqApi";
}

public class OllamaConfig : AiInferenceConfigBase
{
    public new static string ConfigSectionName => "OllamaApi";
    public int NumPredict { get; set; } = 500;
    public float Temperature { get; set; } = 0.0f;
    public float TopP { get; set; } = 1.0f;
    public int? Seed { get; set; } = 32;
}

public class AzureAISearchConfig : AiInferenceConfigBase
{
    public new static string ConfigSectionName => "AzureAISearch";
}

public class WeaviateConfig : AiInferenceConfigBase
{
    public new static string ConfigSectionName => "Weaviate";
}

public class AzureCosmosDBConfig
{
    public const string MongoDBConfigSectionName = "AzureCosmosDBMongoDB";
    public const string NoSQLConfigSectionName = "AzureCosmosDBNoSQL";
    [Required]
    public string ConnectionString { get; set; } = string.Empty;
    [Required]
    public string DatabaseName { get; set; } = string.Empty;
}

public class QdrantConfig
{
    public const string ConfigSectionName = "Qdrant";
    [Required]
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; } = 6334;
    public bool Https { get; set; }
    public string ApiKey { get; set; } = string.Empty;
}

public class RedisConfig
{
    public const string ConfigSectionName = "Redis";
    [Required]
    public string ConnectionConfiguration { get; set; } = string.Empty;
}

public class AzureAdConfig : IAzureAdConfig
{
    public string Instance { get; set; } = string.Empty;
    public string Domain { get; set; } = string.Empty;
    public string TenantId { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string CallbackPath { get; set; } = string.Empty;
    public string SignedOutCallbackPath { get; set; } = string.Empty;
    public string Scopes { get; set; } = string.Empty;
}

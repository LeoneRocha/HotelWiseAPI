using System.ComponentModel.DataAnnotations;

namespace HotelWise.Domain.Dto.AppConfig;

/// <summary>
/// Azure CosmosDB service settings for use with AzureCosmosDBMongoDB and AzureCosmosDBNoSQL.
/// </summary>
public sealed class AzureCosmosDBConfig
{
    public const string MongoDBConfigSectionName = "AzureCosmosDBMongoDB";
    public const string NoSQLConfigSectionName = "AzureCosmosDBNoSQL";

    [Required]
    public string ConnectionString { get; set; } = string.Empty;

    [Required]
    public string DatabaseName { get; set; } = string.Empty;
}

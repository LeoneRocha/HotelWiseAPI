namespace HotelWise.Domain.Dto.AppConfig.Rag;

/// <summary>
/// ⚠️ Movido para HotelWise.Core.SDK — implementação canônica no pacote Core.
/// </summary>
[Obsolete(
    "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.AI.Configuration.AzureCosmosDBConfig.",
    error: false,
    DiagnosticId = "HW_CORE_SDK_AI")]
public class AzureCosmosDBConfig : HotelWise.Core.SDK.AI.Configuration.AzureCosmosDBConfig
{
    public new const string MongoDBConfigSectionName = HotelWise.Core.SDK.AI.Configuration.AzureCosmosDBConfig.MongoDBConfigSectionName;
    public new const string NoSQLConfigSectionName = HotelWise.Core.SDK.AI.Configuration.AzureCosmosDBConfig.NoSQLConfigSectionName;
}

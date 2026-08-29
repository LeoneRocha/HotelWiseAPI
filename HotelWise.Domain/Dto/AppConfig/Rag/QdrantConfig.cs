namespace HotelWise.Domain.Dto.AppConfig.Rag;

/// <summary>
/// ⚠️ Movido para HotelWise.Core.SDK — implementação canônica no pacote Core.
/// </summary>
[Obsolete(
    "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.AI.Configuration.QdrantConfig.",
    error: false,
    DiagnosticId = "HW_CORE_SDK_AI")]
public class QdrantConfig : HotelWise.Core.SDK.AI.Configuration.QdrantConfig
{
    public new const string ConfigSectionName = HotelWise.Core.SDK.AI.Configuration.QdrantConfig.ConfigSectionName;
}

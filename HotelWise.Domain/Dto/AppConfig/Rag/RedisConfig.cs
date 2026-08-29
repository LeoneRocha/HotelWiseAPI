namespace HotelWise.Domain.Dto.AppConfig.Rag;

/// <summary>
/// ⚠️ Movido para HotelWise.Core.SDK — implementação canônica no pacote Core.
/// </summary>
[Obsolete(
    "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.AI.Configuration.RedisConfig.",
    error: false,
    DiagnosticId = "HW_CORE_SDK_AI")]
public class RedisConfig : HotelWise.Core.SDK.AI.Configuration.RedisConfig
{
    public new const string ConfigSectionName = HotelWise.Core.SDK.AI.Configuration.RedisConfig.ConfigSectionName;
}

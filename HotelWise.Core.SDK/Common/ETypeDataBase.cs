using SchCommon = SmartCoreHub.Core.SDK.Common;

using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.Common;

/// <summary>
/// Enumeração dos tipos de banco de dados suportados pela infraestrutura genérica do SDK.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Common.ETypeDataBase", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Common.ETypeDataBase em SmartCoreHub.Core.SDK.")]
public enum ETypeDataBase
{
    MSsqlServer = (int)SchCommon.ETypeDataBase.MSsqlServer,
    Mysql = (int)SchCommon.ETypeDataBase.Mysql,
    Postgree = (int)SchCommon.ETypeDataBase.Postgree,
    FireBase = (int)SchCommon.ETypeDataBase.FireBase,
}

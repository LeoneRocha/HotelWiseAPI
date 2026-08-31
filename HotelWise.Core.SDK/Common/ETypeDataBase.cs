using SchCommon = SmartCoreHub.Core.SDK.Common;

namespace HotelWise.Core.SDK.Common;

/// <summary>
/// Enumeração dos tipos de banco de dados suportados pela infraestrutura genérica do SDK.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Common. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Common.ETypeDataBase. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public enum ETypeDataBase
{
    MSsqlServer = (int)SchCommon.ETypeDataBase.MSsqlServer,
    Mysql = (int)SchCommon.ETypeDataBase.Mysql,
    Postgree = (int)SchCommon.ETypeDataBase.Postgree,
    FireBase = (int)SchCommon.ETypeDataBase.FireBase,
}

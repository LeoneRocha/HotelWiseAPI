using SchCommon = SmartCoreHub.Core.SDK.Common;

namespace HotelWise.Core.SDK.Common;

/// <summary>
/// Enumeração dos tipos de banco de dados suportados pela infraestrutura genérica do SDK.
/// </summary>
public enum ETypeDataBase
{
    MSsqlServer = (int)SchCommon.ETypeDataBase.MSsqlServer,
    Mysql = (int)SchCommon.ETypeDataBase.Mysql,
    Postgree = (int)SchCommon.ETypeDataBase.Postgree,
    FireBase = (int)SchCommon.ETypeDataBase.FireBase,
}

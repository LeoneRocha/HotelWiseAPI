namespace HotelWise.Core.SDK.Common;

/// <summary>
/// Enumeração dos tipos de banco de dados suportados pela infraestrutura genérica do SDK.
/// Usada para adaptar tipos de coluna, limites de texto e estratégias de configuração EF.
/// </summary>
public enum ETypeDataBase
{
    /// <summary>
    /// Microsoft SQL Server.
    /// </summary>
    MSsqlServer = 0,

    /// <summary>
    /// MySQL.
    /// </summary>
    Mysql = 1,

    /// <summary>
    /// PostgreSQL.
    /// </summary>
    Postgree = 3,

    /// <summary>
    /// Firebase (provedor NoSQL / backend gerenciado).
    /// </summary>
    FireBase = 4,
}

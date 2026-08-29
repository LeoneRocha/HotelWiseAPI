using HotelWise.Core.SDK.Common;

namespace HotelWise.Core.SDK.Common.Constants;

/// <summary>
/// Constantes e helpers de configuração de tipos de coluna EF Core / charset.
/// Centraliza literais de <c>TypeName</c>, idioma padrão e funções auxiliares
/// que adaptam comprimento e tipo de texto conforme o <see cref="ETypeDataBase"/> em uso.
/// </summary>
public static class EntityTypeConfigurationConstants
{
    /// <summary>
    /// Tipo de coluna VARCHAR com comprimento 255.
    /// </summary>
    public const string Type_Varchar_255 = "varchar(255)";

    /// <summary>
    /// Tipo de coluna VARCHAR com comprimento 40.
    /// </summary>
    public const string Type_Varchar_40 = "varchar(40)";

    /// <summary>
    /// Tipo de coluna VARCHAR com comprimento 20.
    /// </summary>
    public const string Type_Varchar_20 = "varchar(20)";

    /// <summary>
    /// Tipo de coluna texto longo para MySQL (<c>text</c>).
    /// </summary>
    public const string Type_Text_MySql = "text";

    /// <summary>
    /// Tipo de coluna texto longo para SQL Server (<c>varchar(max)</c>).
    /// </summary>
    public const string Type_Text_SqlServer = "varchar(max)";

    /// <summary>
    /// Código de cultura/idioma padrão da aplicação (português do Brasil).
    /// </summary>
    public const string Language_Default_PTBR = "pt-BR";

    /// <summary>
    /// Chave de recurso padrão para localização compartilhada (<c>SharedResource</c>).
    /// </summary>
    public const string ApplicationLanguage_ResourceKey_Default = "SharedResource";

    /// <summary>
    /// Obtém o comprimento máximo de texto suportado pelo banco de dados informado.
    /// </summary>
    /// <param name="eTypeDataBase">Tipo de banco de dados alvo.</param>
    /// <returns>
    /// Limite máximo de caracteres: <see cref="int.MaxValue"/> para a maioria dos provedores;
    /// <c>65535</c> para MySQL.
    /// </returns>
    public static int GetMaxLengthByTypeDataBase(ETypeDataBase eTypeDataBase)
    {
        switch (eTypeDataBase)
        {
            case ETypeDataBase.MSsqlServer:
                return int.MaxValue;
            case ETypeDataBase.Mysql:
                return 65535;
            case ETypeDataBase.Postgree:
                return int.MaxValue;
            case ETypeDataBase.FireBase:
                return int.MaxValue;
            default:
                return int.MaxValue;
        }
    }

    /// <summary>
    /// Obtém o nome de tipo de coluna de texto longo adequado ao banco de dados informado.
    /// </summary>
    /// <param name="eTypeDataBase">Tipo de banco de dados alvo.</param>
    /// <returns>
    /// <see cref="Type_Text_MySql"/> para MySQL; <see cref="Type_Text_SqlServer"/> para os demais casos.
    /// </returns>
    public static string GetTypeTextByTypeDataBase(ETypeDataBase eTypeDataBase)
    {
        switch (eTypeDataBase)
        {
            case ETypeDataBase.MSsqlServer:
                return Type_Text_SqlServer;
            case ETypeDataBase.Mysql:
                return Type_Text_MySql;
            case ETypeDataBase.Postgree:
                return Type_Text_SqlServer;
            case ETypeDataBase.FireBase:
                return Type_Text_SqlServer;
            default:
                return Type_Text_SqlServer;
        }
    }
}

using HotelWise.Core.SDK.Common;

namespace HotelWise.Core.SDK.Common.Constants;

/// <summary>
/// Constantes e helpers de configuração de tipos de coluna EF Core / charset.
/// Centraliza literais de <c>TypeName</c>, idioma padrão e funções auxiliares
/// que adaptam comprimento e tipo de texto conforme o <see cref="ETypeDataBase"/> em uso.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Common. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Common.Constants.EntityTypeConfigurationConstants. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public static class EntityTypeConfigurationConstants
{
    /// <summary>Tipo de coluna varchar de 255 caracteres.</summary>
    public const string Type_Varchar_255 = "varchar(255)";

    /// <summary>Tipo de coluna varchar de 40 caracteres.</summary>
    public const string Type_Varchar_40 = "varchar(40)";

    /// <summary>Tipo de coluna varchar de 20 caracteres.</summary>
    public const string Type_Varchar_20 = "varchar(20)";

    /// <summary>Tipo de coluna text para MySQL.</summary>
    public const string Type_Text_MySql = "text";

    /// <summary>Tipo de coluna varchar(max) para SQL Server.</summary>
    public const string Type_Text_SqlServer = "varchar(max)";

    /// <summary>Código de idioma padrão pt-BR.</summary>
    public const string Language_Default_PTBR = "pt-BR";

    /// <summary>Chave padrão de recurso de idioma compartilhado.</summary>
    public const string ApplicationLanguage_ResourceKey_Default = "SharedResource";

    /// <summary>
    /// Retorna o comprimento máximo padrão para campos de texto conforme o tipo de banco de dados.
    /// </summary>
    /// <param name="eTypeDataBase">Tipo de banco de dados.</param>
    /// <returns>Tamanho máximo do campo.</returns>
    public static int GetMaxLengthByTypeDataBase(ETypeDataBase eTypeDataBase) =>
        SmartCoreHub.Core.SDK.Common.Constants.EntityTypeConfigurationConstants.GetMaxLengthByTypeDataBase((SmartCoreHub.Core.SDK.Common.ETypeDataBase)(int)eTypeDataBase);

    /// <summary>
    /// Retorna o literal de tipo de coluna de texto longo conforme o tipo de banco de dados.
    /// </summary>
    /// <param name="eTypeDataBase">Tipo de banco de dados.</param>
    /// <returns>Nome do tipo de coluna no banco.</returns>
    public static string GetTypeTextByTypeDataBase(ETypeDataBase eTypeDataBase) =>
        SmartCoreHub.Core.SDK.Common.Constants.EntityTypeConfigurationConstants.GetTypeTextByTypeDataBase((SmartCoreHub.Core.SDK.Common.ETypeDataBase)(int)eTypeDataBase);
}

using HotelWise.Core.SDK.Common;

using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.Common.Constants;

/// <summary>
/// Constantes e helpers de configuração de tipos de coluna EF Core / charset.
/// Centraliza literais de <c>TypeName</c>, idioma padrão e funções auxiliares
/// que adaptam comprimento e tipo de texto conforme o <see cref="ETypeDataBase"/> em uso.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Common.Constants.EntityTypeConfigurationConstants", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Common.Constants.EntityTypeConfigurationConstants em SmartCoreHub.Core.SDK.")]
public static class EntityTypeConfigurationConstants
{
    /// <summary>Tipo de coluna varchar de 255 caracteres.</summary>
    public const string Type_Varchar_255 =
        SmartCoreHub.Core.SDK.Common.Constants.EntityTypeConfigurationConstants.Type_Varchar_255;

    /// <summary>Tipo de coluna varchar de 40 caracteres.</summary>
    public const string Type_Varchar_40 =
        SmartCoreHub.Core.SDK.Common.Constants.EntityTypeConfigurationConstants.Type_Varchar_40;

    /// <summary>Tipo de coluna varchar de 20 caracteres.</summary>
    public const string Type_Varchar_20 =
        SmartCoreHub.Core.SDK.Common.Constants.EntityTypeConfigurationConstants.Type_Varchar_20;

    /// <summary>Tipo de coluna text para MySQL.</summary>
    public const string Type_Text_MySql =
        SmartCoreHub.Core.SDK.Common.Constants.EntityTypeConfigurationConstants.Type_Text_MySql;

    /// <summary>Tipo de coluna varchar(max) para SQL Server.</summary>
    public const string Type_Text_SqlServer =
        SmartCoreHub.Core.SDK.Common.Constants.EntityTypeConfigurationConstants.Type_Text_SqlServer;

    /// <summary>Código de idioma padrão pt-BR.</summary>
    public const string Language_Default_PTBR =
        SmartCoreHub.Core.SDK.Common.Constants.EntityTypeConfigurationConstants.Language_Default_PTBR;

    /// <summary>Chave padrão de recurso de idioma compartilhado.</summary>
    public const string ApplicationLanguage_ResourceKey_Default =
        SmartCoreHub.Core.SDK.Common.Constants.EntityTypeConfigurationConstants.ApplicationLanguage_ResourceKey_Default;

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

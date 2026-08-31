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
    public const string Type_Varchar_255 = "varchar(255)";

    public const string Type_Varchar_40 = "varchar(40)";

    public const string Type_Varchar_20 = "varchar(20)";

    public const string Type_Text_MySql = "text";

    public const string Type_Text_SqlServer = "varchar(max)";

    public const string Language_Default_PTBR = "pt-BR";

    public const string ApplicationLanguage_ResourceKey_Default = "SharedResource";

    public static int GetMaxLengthByTypeDataBase(ETypeDataBase eTypeDataBase) =>
        SmartCoreHub.Core.SDK.Common.Constants.EntityTypeConfigurationConstants.GetMaxLengthByTypeDataBase((SmartCoreHub.Core.SDK.Common.ETypeDataBase)(int)eTypeDataBase);

    public static string GetTypeTextByTypeDataBase(ETypeDataBase eTypeDataBase) =>
        SmartCoreHub.Core.SDK.Common.Constants.EntityTypeConfigurationConstants.GetTypeTextByTypeDataBase((SmartCoreHub.Core.SDK.Common.ETypeDataBase)(int)eTypeDataBase);
}

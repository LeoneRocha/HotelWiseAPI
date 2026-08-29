using HotelWise.Domain.Enuns;

namespace HotelWise.Domain.Constants
{
    /// <summary>
    /// ⚠️ Movido para HotelWise.Core.SDK — implementação canônica no pacote Core.
    /// </summary>
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.Common.Constants.EntityTypeConfigurationConstants.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_COMMON")]
    public static class EntityTypeConfigurationConstants
    {
        public const string Type_Varchar_255 = HotelWise.Core.SDK.Common.Constants.EntityTypeConfigurationConstants.Type_Varchar_255;
        public const string Type_Varchar_40 = HotelWise.Core.SDK.Common.Constants.EntityTypeConfigurationConstants.Type_Varchar_40;
        public const string Type_Varchar_20 = HotelWise.Core.SDK.Common.Constants.EntityTypeConfigurationConstants.Type_Varchar_20;

        public const string Type_Text_MySql = HotelWise.Core.SDK.Common.Constants.EntityTypeConfigurationConstants.Type_Text_MySql;
        public const string Type_Text_SqlServer = HotelWise.Core.SDK.Common.Constants.EntityTypeConfigurationConstants.Type_Text_SqlServer;

        public const string Language_Default_PTBR = HotelWise.Core.SDK.Common.Constants.EntityTypeConfigurationConstants.Language_Default_PTBR;

        public const string ApplicationLanguage_ResourceKey_Default = HotelWise.Core.SDK.Common.Constants.EntityTypeConfigurationConstants.ApplicationLanguage_ResourceKey_Default;

        public static int GetMaxLengthByTypeDataBase(ETypeDataBase eTypeDataBase) =>
            HotelWise.Core.SDK.Common.Constants.EntityTypeConfigurationConstants.GetMaxLengthByTypeDataBase(
                (HotelWise.Core.SDK.Common.ETypeDataBase)(int)eTypeDataBase);

        public static string GetTypeTextByTypeDataBase(ETypeDataBase eTypeDataBase) =>
            HotelWise.Core.SDK.Common.Constants.EntityTypeConfigurationConstants.GetTypeTextByTypeDataBase(
                (HotelWise.Core.SDK.Common.ETypeDataBase)(int)eTypeDataBase);
    }
}

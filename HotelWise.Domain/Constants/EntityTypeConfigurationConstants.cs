
using HotelWise.Domain.Enuns;

namespace HotelWise.Domain.Constants
{
    public static class EntityTypeConfigurationConstants
    {
        public const string Type_Varchar_255 = "varchar(255)";
        public const string Type_Varchar_40 = "varchar(40)";
        public const string Type_Varchar_20 = "varchar(20)";

        public const string Type_Text_MySql = "text";
        public const string Type_Text_SqlServer = "varchar(max)";

        public const string Language_Default_PTBR = "pt-BR";

        public const string ApplicationLanguage_ResourceKey_Default = "SharedResource";

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
}

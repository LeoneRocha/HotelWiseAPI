namespace HotelWise.Domain.Constants
{
    /// <summary>
    /// ⚠️ Movido para HotelWise.Core.SDK — implementação canônica no pacote Core.
    /// </summary>
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.Common.Constants.ValidatorConstants.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_COMMON")]
    public static class ValidatorConstants
    {
        public const string NameResponseValidate_ValidatePermissionMedical = HotelWise.Core.SDK.Common.Constants.ValidatorConstants.NameResponseValidate_ValidatePermissionMedical;

        public const string GenericErroMessage = HotelWise.Core.SDK.Common.Constants.ValidatorConstants.GenericErroMessage;
        public const string Validade_UserNotFound = HotelWise.Core.SDK.Common.Constants.ValidatorConstants.Validade_UserNotFound;

        public const string Validate_Permission_Medical = HotelWise.Core.SDK.Common.Constants.ValidatorConstants.Validate_Permission_Medical;

        public const string GenericErroMessageKey = HotelWise.Core.SDK.Common.Constants.ValidatorConstants.GenericErroMessageKey;
        public const string Generic_Erro_Message = HotelWise.Core.SDK.Common.Constants.ValidatorConstants.Generic_Erro_Message;

        public const string ValidateErroMessageKey = HotelWise.Core.SDK.Common.Constants.ValidatorConstants.ValidateErroMessageKey;
        public const string ValidateErroMessage_Message = HotelWise.Core.SDK.Common.Constants.ValidatorConstants.ValidateErroMessage_Message;
        public const string ValidateSuccessMessageKey = HotelWise.Core.SDK.Common.Constants.ValidatorConstants.ValidateSuccessMessageKey;
        public const string ValidateSuccessMessage_Message = HotelWise.Core.SDK.Common.Constants.ValidatorConstants.ValidateSuccessMessage_Message;
    }
}

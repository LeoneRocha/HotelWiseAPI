namespace HotelWise.Core.SDK.Common.Constants;

/// <summary>
/// Constantes de chaves e mensagens de validação usadas pelos validadores e respostas de serviço.
/// Inclui identificadores de recurso de localização e textos padrão de erro/sucesso.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Common. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Common.Constants.ValidatorConstants. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public static class ValidatorConstants
{
    public const string NameResponseValidate_ValidatePermissionMedical = "ValidatePermissionMedical";

    public const string GenericErroMessage = "GenericErroMessage";

    public const string Validade_UserNotFound = "User not found.";

    public const string Validate_Permission_Medical = "Validate_Permission_Medical";

    public const string GenericErroMessageKey = "Generic_Erro_Message";

    public const string Generic_Erro_Message = "An error occurred in the process.";

    public const string ValidateErroMessageKey = "Validate_Erro_Message";

    public const string ValidateErroMessage_Message = "The validations did not pass";

    public const string ValidateSuccessMessageKey = "Validate_Success_Message";

    public const string ValidateSuccessMessage_Message = "All validations passed";
}

namespace HotelWise.Core.SDK.Common.Constants;

/// <summary>
/// Constantes de chaves e mensagens de validação usadas pelos validadores e respostas de serviço.
/// Inclui identificadores de recurso de localização e textos padrão de erro/sucesso.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Common. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Common.Constants.ValidatorConstants. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public static class ValidatorConstants
{
    /// <summary>
    /// Nome da resposta de validação de permissão médica.
    /// </summary>
    public const string NameResponseValidate_ValidatePermissionMedical = "ValidatePermissionMedical";

    /// <summary>
    /// Identificador genérico de mensagem de erro.
    /// </summary>
    public const string GenericErroMessage = "GenericErroMessage";

    /// <summary>
    /// Mensagem padrão quando o usuário não é encontrado.
    /// </summary>
    public const string Validade_UserNotFound = "User not found.";

    /// <summary>
    /// Chave de validação de permissão médica.
    /// </summary>
    public const string Validate_Permission_Medical = "Validate_Permission_Medical";

    /// <summary>
    /// Chave de recurso para mensagem genérica de erro.
    /// </summary>
    public const string GenericErroMessageKey = "Generic_Erro_Message";

    /// <summary>
    /// Texto padrão da mensagem genérica de erro de processo.
    /// </summary>
    public const string Generic_Erro_Message = "An error occurred in the process.";

    /// <summary>
    /// Chave de recurso para mensagem de falha de validação.
    /// </summary>
    public const string ValidateErroMessageKey = "Validate_Erro_Message";

    /// <summary>
    /// Texto padrão indicando que as validações não passaram.
    /// </summary>
    public const string ValidateErroMessage_Message = "The validations did not pass";

    /// <summary>
    /// Chave de recurso para mensagem de sucesso de validação.
    /// </summary>
    public const string ValidateSuccessMessageKey = "Validate_Success_Message";

    /// <summary>
    /// Texto padrão indicando que todas as validações passaram.
    /// </summary>
    public const string ValidateSuccessMessage_Message = "All validations passed";
}

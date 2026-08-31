namespace HotelWise.Core.SDK.Common.Constants;

/// <summary>
/// Constantes de chaves e mensagens de validação usadas pelos validadores e respostas de serviço.
/// Inclui identificadores de recurso de localização e textos padrão de erro/sucesso.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Common. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Common.Constants.ValidatorConstants. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public static class ValidatorConstants
{
    /// <summary>Nome de resposta para validação de permissão médica.</summary>
    public const string NameResponseValidate_ValidatePermissionMedical = "ValidatePermissionMedical";

    /// <summary>Chave para mensagem de erro genérica.</summary>
    public const string GenericErroMessage = "GenericErroMessage";

    /// <summary>Mensagem quando o usuário não é encontrado.</summary>
    public const string Validade_UserNotFound = "User not found.";

    /// <summary>Identificador para validação de permissão médica.</summary>
    public const string Validate_Permission_Medical = "Validate_Permission_Medical";

    /// <summary>Chave de recurso para mensagem de erro genérica.</summary>
    public const string GenericErroMessageKey = "Generic_Erro_Message";

    /// <summary>Texto padrão para mensagem de erro genérica de processo.</summary>
    public const string Generic_Erro_Message = "An error occurred in the process.";

    /// <summary>Chave de recurso para mensagem de falha em validações.</summary>
    public const string ValidateErroMessageKey = "Validate_Erro_Message";

    /// <summary>Texto padrão quando validações não são atendidas.</summary>
    public const string ValidateErroMessage_Message = "The validations did not pass";

    /// <summary>Chave de recurso para mensagem de validações atendidas com sucesso.</summary>
    public const string ValidateSuccessMessageKey = "Validate_Success_Message";

    /// <summary>Texto padrão quando todas as validações foram atendidas.</summary>
    public const string ValidateSuccessMessage_Message = "All validations passed";
}

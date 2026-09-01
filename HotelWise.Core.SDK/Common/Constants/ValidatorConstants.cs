using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.Common.Constants;

/// <summary>
/// Constantes de chaves e mensagens de validação usadas pelos validadores e respostas de serviço.
/// Inclui identificadores de recurso de localização e textos padrão de erro/sucesso.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Common.Constants.ValidatorConstants", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Common.Constants.ValidatorConstants em SmartCoreHub.Core.SDK.")]
public static class ValidatorConstants
{
    /// <summary>Nome de resposta para validação de permissão médica.</summary>
    public const string NameResponseValidate_ValidatePermissionMedical =
        SmartCoreHub.Core.SDK.Common.Constants.ValidatorConstants.NameResponseValidate_ValidatePermission;

    /// <summary>Chave para mensagem de erro genérica.</summary>
    public const string GenericErroMessage =
        SmartCoreHub.Core.SDK.Common.Constants.ValidatorConstants.GenericErroMessage;

    /// <summary>Mensagem quando o usuário não é encontrado.</summary>
    public const string Validade_UserNotFound =
        SmartCoreHub.Core.SDK.Common.Constants.ValidatorConstants.Validade_UserNotFound;

    /// <summary>Identificador para validação de permissão médica.</summary>
    public const string Validate_Permission_Medical =
        SmartCoreHub.Core.SDK.Common.Constants.ValidatorConstants.Validate_Permission;

    /// <summary>Chave de recurso para mensagem de erro genérica.</summary>
    public const string GenericErroMessageKey =
        SmartCoreHub.Core.SDK.Common.Constants.ValidatorConstants.GenericErroMessageKey;

    /// <summary>Texto padrão para mensagem de erro genérica de processo.</summary>
    public const string Generic_Erro_Message =
        SmartCoreHub.Core.SDK.Common.Constants.ValidatorConstants.Generic_Erro_Message;

    /// <summary>Chave de recurso para mensagem de falha em validações.</summary>
    public const string ValidateErroMessageKey =
        SmartCoreHub.Core.SDK.Common.Constants.ValidatorConstants.ValidateErroMessageKey;

    /// <summary>Texto padrão quando validações não são atendidas.</summary>
    public const string ValidateErroMessage_Message =
        SmartCoreHub.Core.SDK.Common.Constants.ValidatorConstants.ValidateErroMessage_Message;

    /// <summary>Chave de recurso para mensagem de validações atendidas com sucesso.</summary>
    public const string ValidateSuccessMessageKey =
        SmartCoreHub.Core.SDK.Common.Constants.ValidatorConstants.ValidateSuccessMessageKey;

    /// <summary>Texto padrão quando todas as validações foram atendidas.</summary>
    public const string ValidateSuccessMessage_Message =
        SmartCoreHub.Core.SDK.Common.Constants.ValidatorConstants.ValidateSuccessMessage_Message;
}

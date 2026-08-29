#if NET8_0_OR_GREATER
using FluentValidation.Results;
using HotelWise.Core.SDK.Common;

namespace HotelWise.Core.SDK.Validation;

/// <summary>
/// Mapeamento de resultados FluentValidation (<see cref="ValidationResult"/> /
/// <see cref="ValidationFailure"/>) para o contrato de erro do SDK
/// (<see cref="ErrorResponse"/>), incluindo normalização de códigos de erro.
/// </summary>
public static class HelperValidation
{
    /// <summary>
    /// Converte um <see cref="ValidationResult"/> em array de <see cref="ErrorResponse"/>.
    /// </summary>
    /// <param name="validationResult">Resultado da validação; nulo ou válido retorna vazio.</param>
    /// <returns>Erros mapeados ou array vazio.</returns>
    public static ErrorResponse[] GetErrorsMap(ValidationResult? validationResult)
    {
        if (validationResult == null || validationResult.IsValid) return Array.Empty<ErrorResponse>();

        return validationResult.Errors.Select(ConvertToErrorResponse).ToArray();
    }

    /// <summary>
    /// Converte uma falha FluentValidation em <see cref="ErrorResponse"/>,
    /// extraindo ErrorCode quando a mensagem segue o padrão <c>CODE|mensagem</c>.
    /// </summary>
    /// <param name="errorItem">Falha de validação de origem.</param>
    /// <returns>Erro no formato do SDK.</returns>
    private static ErrorResponse ConvertToErrorResponse(ValidationFailure errorItem)
    {
        var errorAdd = new ErrorResponse
        {
            FullMessage = errorItem.ErrorMessage,
            DefaultMessage = errorItem.ErrorMessage,
            Message = errorItem.ErrorMessage,
            ErrorCode = errorItem.ErrorCode,
            Name = errorItem.PropertyName
        };

        if (errorAdd.Message.Contains('|') && errorAdd.Message.Contains('_'))
        {
            var parts = errorAdd.Message.Split('|');
            errorAdd.ErrorCode = parts[0];
            errorAdd.DefaultMessage = parts.Length > 1 ? parts[1] : errorItem.ErrorMessage;
        }
        else if (!errorAdd.Message.Contains('_'))
        {
            errorAdd.ErrorCode = errorAdd.Message.Replace(" ", "_");
        }

        return errorAdd;
    }

    /// <summary>
    /// Substitui o placeholder <c>[MaxLength]</c> na mensagem pelo código de erro informado.
    /// </summary>
    /// <param name="message">Mensagem original com possível placeholder.</param>
    /// <param name="errorCode">Código a inserir (colchetes e vírgulas são removidos).</param>
    /// <returns>Mensagem traduzida/ajustada.</returns>
    public static string TranslateErroCode(string message, string errorCode)
    {
        if (!string.IsNullOrEmpty(errorCode))
        {
            message = message.Replace("[MaxLength]", errorCode.Replace("[", "").Replace("]", "").Replace(",", ""));
        }
        return message;
    }

    /// <summary>
    /// Converte uma lista de <see cref="ValidationFailure"/> em <see cref="ErrorResponse"/>,
    /// deduplicando por PropertyName.
    /// </summary>
    /// <param name="errors">Lista de falhas FluentValidation.</param>
    /// <returns>Lista de erros no formato do SDK.</returns>
    public static List<ErrorResponse> ConvertValidationFailureListToErroResponse(List<ValidationFailure> errors)
    {
        return errors.DistinctBy(d => d.PropertyName).Select(ConvertToErrorResponse).ToList();
    }
}
#endif

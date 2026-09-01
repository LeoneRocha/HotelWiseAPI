#if NET8_0_OR_GREATER
using FluentValidation.Results;
using SchCommon = SmartCoreHub.Core.SDK.Common;
using SchValidation = SmartCoreHub.Core.SDK.Service.Validation;

namespace HotelWise.Core.SDK.Validation;

/// <summary>
/// Mapeamento FluentValidation → <see cref="SchCommon.ErrorResponse"/> — delega ao SCH.
/// </summary>
public static class HelperValidation
{
    /// <inheritdoc cref="SchValidation.HelperValidation.GetErrorsMap(ValidationResult?)"/>
    public static SchCommon.ErrorResponse[] GetErrorsMap(ValidationResult? validationResult) =>
        SchValidation.HelperValidation.GetErrorsMap(validationResult);

    /// <inheritdoc cref="SchValidation.HelperValidation.TranslateErroCode(string, string)"/>
    public static string TranslateErroCode(string message, string errorCode) =>
        SchValidation.HelperValidation.TranslateErroCode(message, errorCode);

    /// <inheritdoc cref="SchValidation.HelperValidation.ConvertValidationFailureListToErroResponse(System.Collections.Generic.List{ValidationFailure})"/>
    public static List<SchCommon.ErrorResponse> ConvertValidationFailureListToErroResponse(List<ValidationFailure> errors) =>
        SchValidation.HelperValidation.ConvertValidationFailureListToErroResponse(errors);
}
#endif

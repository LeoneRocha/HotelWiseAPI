#if NET8_0_OR_GREATER
using FluentValidation.Results;
using SchDto = SmartCoreHub.Core.SDK.Domain.DTOs.Common;
using SchValidation = SmartCoreHub.Core.SDK.Service.Validation;

using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.Validation;

/// <summary>
/// Mapeamento FluentValidation → <see cref="SchDto.ErrorResponse"/> — delega ao SCH.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Service.Validation.HelperValidation", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Service.Validation.HelperValidation em SmartCoreHub.Core.SDK.")]
public static class HelperValidation
{
    /// <inheritdoc cref="SchValidation.HelperValidation.GetErrorsMap(ValidationResult?)"/>
    public static SchDto.ErrorResponse[] GetErrorsMap(ValidationResult? validationResult) =>
        SchValidation.HelperValidation.GetErrorsMap(validationResult);

    /// <inheritdoc cref="SchValidation.HelperValidation.TranslateErroCode(string, string)"/>
    public static string TranslateErroCode(string message, string errorCode) =>
        SchValidation.HelperValidation.TranslateErroCode(message, errorCode);

    /// <inheritdoc cref="SchValidation.HelperValidation.ConvertValidationFailureListToErroResponse(System.Collections.Generic.List{ValidationFailure})"/>
    public static List<SchDto.ErrorResponse> ConvertValidationFailureListToErroResponse(List<ValidationFailure> errors) =>
        SchValidation.HelperValidation.ConvertValidationFailureListToErroResponse(errors);
}
#endif

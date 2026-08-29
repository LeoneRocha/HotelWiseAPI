using FluentValidation.Results;
using HotelWise.Domain.Dto;

namespace HotelWise.Domain.Helpers
{
    /// <summary>
    /// ⚠️ Movido para HotelWise.Core.SDK — implementação canônica no pacote Core.
    /// </summary>
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.Validation.HelperValidation.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_HELPER")]
    public static class HelperValidation
    {
        public static ErrorResponse[] GetErrorsMap(ValidationResult? validationResult) =>
            MapErrors(HotelWise.Core.SDK.Validation.HelperValidation.GetErrorsMap(validationResult));

        public static string TranslateErroCode(string message, string errorCode) =>
            HotelWise.Core.SDK.Validation.HelperValidation.TranslateErroCode(message, errorCode);

        public static List<ErrorResponse> ConvertValidationFailureListToErroResponse(List<ValidationFailure> errors) =>
            MapErrors(HotelWise.Core.SDK.Validation.HelperValidation.ConvertValidationFailureListToErroResponse(errors).ToArray()).ToList();

        private static ErrorResponse[] MapErrors(HotelWise.Core.SDK.Common.ErrorResponse[] errors) =>
            errors.Select(e => new ErrorResponse
            {
                Name = e.Name,
                Message = e.Message,
                ErrorCode = e.ErrorCode,
                DefaultMessage = e.DefaultMessage,
                FullMessage = e.FullMessage
            }).ToArray();
    }
}

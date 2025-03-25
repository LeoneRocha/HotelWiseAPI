using FluentValidation.Results;
using HotelWise.Domain.Dto;

namespace HotelWise.Domain.Helpers
{
    public static class HelperValidation
    {
        public static ErrorResponse[] GetErrorsMap(FluentValidation.Results.ValidationResult? validationResult)
        {
            if (validationResult == null || validationResult.IsValid) return Array.Empty<ErrorResponse>();

            return validationResult.Errors.Select(ConvertToErrorResponse).ToArray();
        }

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
                // Remove todos os espaços e substitui por "_"
                errorAdd.ErrorCode = errorAdd.Message.Replace(" ", "_");
            }

            return errorAdd;
        }


        public static string TranslateErroCode(string message, string errorCode)
        {
            if (!string.IsNullOrEmpty(errorCode))
            {
                message = message.Replace("[MaxLength]", errorCode.Replace("[", "").Replace("]", "").Replace(",", ""));

            }
            return message;
        }

        public static List<ErrorResponse> ConvertValidationFailureListToErroResponse(List<ValidationFailure> errors)
        {
            return errors.DistinctBy(d => d.PropertyName).Select(er => ConvertToErrorResponse(er)).ToList();
        }
    }
}

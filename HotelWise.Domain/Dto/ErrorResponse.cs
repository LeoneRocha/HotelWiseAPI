namespace HotelWise.Domain.Dto
{
    public class ErrorResponse
    {
        public string Name { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;
        public string ErrorCode { get; set; } = string.Empty;
    }

}

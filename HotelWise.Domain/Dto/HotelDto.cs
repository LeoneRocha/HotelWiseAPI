namespace HotelWise.Domain.Model
{
    public class HotelDto : Hotel
    {  
        public double Score { get; set; }
    }

    public class AskAssistantResponse
    {
        public string Response { get; set; } = string.Empty;
    }
}
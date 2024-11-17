namespace HotelWise.Domain.Model
{
    public class HotelDto : Hotel
    {
        public bool IsHotelInVectorStore { get; set; }
        public double Score { get; set; }
    } 
}
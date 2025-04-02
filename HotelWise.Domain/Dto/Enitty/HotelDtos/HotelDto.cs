using HotelWise.Domain.Model.HotelModels;

namespace HotelWise.Domain.Dto.Enitty.HotelDtos
{
    public class HotelDto : Hotel
    {
        public bool IsHotelInVectorStore { get; set; }
        public double Score { get; set; }
    }
}
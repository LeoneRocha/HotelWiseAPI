using HotelWise.Domain.Model;

namespace HotelWise.Domain.Interfaces
{
    public interface IGenerateHotelService
    {
        Task<Hotel[]> GetHotelsAsync(int numberGerate);
    }
}
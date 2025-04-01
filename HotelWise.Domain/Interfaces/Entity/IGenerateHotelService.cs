using HotelWise.Domain.Model.HotelModels;

namespace HotelWise.Domain.Interfaces.Entity
{
    public interface IGenerateHotelService
    {
        Task<Hotel[]> GetHotelsAsync(int numberGerate);
        Task<Hotel> GetHotelAsync();
         
    }
}
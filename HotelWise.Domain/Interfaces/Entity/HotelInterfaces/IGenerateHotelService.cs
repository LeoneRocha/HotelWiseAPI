using HotelWise.Domain.Model.HotelModels;

namespace HotelWise.Domain.Interfaces.Entity.HotelInterfaces
{
    public interface IGenerateHotelService
    {
        Task<Hotel[]> GetHotelsAsync(int numberGerate);
        Task<Hotel> GetHotelAsync();

    }
}
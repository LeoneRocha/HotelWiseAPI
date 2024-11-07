using HotelWise.Domain.Interfaces;

namespace HotelWise.Domain.AI.Models
{
    public class MixtralModelStrategy : IModelStrategy
    {
        public string GetModel()
        {
            return "mixtral-8x7b-32768";
        }
    } 
}
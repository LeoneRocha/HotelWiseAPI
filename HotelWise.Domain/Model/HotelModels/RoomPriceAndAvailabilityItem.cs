using HotelWise.Domain.Enuns.Hotel;

namespace HotelWise.Domain.Model.HotelModels
{
    public class RoomPriceAndAvailabilityItem
    { 
        public DayOfWeek DayOfWeek { get; set; } // Dia da semana
        public decimal Price { get; set; } // Preço para a data
        public int QuantityAvailable { get; set; } // Quantidade disponível
        public string Currency { get; set; } = "USD"; // Moeda (exemplo: USD, BRL)
        public RoomAvailabilityStatus Status { get; set; } = RoomAvailabilityStatus.Available; // Status inicial 
    } 
} 
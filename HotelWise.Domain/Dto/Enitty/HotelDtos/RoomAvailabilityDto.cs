using HotelWise.Domain.Model.HotelModels;

namespace HotelWise.Domain.Dto.Enitty.HotelDtos
{

    public class RoomAvailabilityDto
    {
        public long Id { get; set; } // ID único da disponibilidade
        public long RoomId { get; set; } // ID do quarto ao qual a disponibilidade pertence
        public DateTime StartDate { get; set; } // Data inicial do período de disponibilidade
        public DateTime EndDate { get; set; } // Data final do período de disponibilidade
        public RoomPriceAndAvailabilityItem[] AvailabilityWithPrice { get; set; } = []; // Lista de preços e disponibilidade detalhada
    }
}
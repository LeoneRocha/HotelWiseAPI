using HotelWise.Domain.Helpers;
using HotelWise.Domain.Model.AI;

namespace HotelWise.Domain.Dto.IA
{
    public class ChatSessionHistoryDto : ChatSessionHistory
    {
        public DateTime UpdateDate { get; set; } = DataHelper.GetDateTimeNow();
    }
}
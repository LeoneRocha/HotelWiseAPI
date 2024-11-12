namespace HotelWise.Domain.Dto
{
    public class SearchCriteria
    {
        public int MaxHotelRetrive { get; set; }
        public string SearchTextCriteria { get; set; } = string.Empty;
    }
}
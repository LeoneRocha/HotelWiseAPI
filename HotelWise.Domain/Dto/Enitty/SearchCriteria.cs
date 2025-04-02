namespace HotelWise.Domain.Dto.Enitty
{
    public class SearchCriteria
    {
        public int MaxHotelRetrieve { get; set; }
        public string SearchTextCriteria { get; set; } = string.Empty;
        public string[] TagsCriteria { get; set; } = [];
    }
}
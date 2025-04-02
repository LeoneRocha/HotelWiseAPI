namespace HotelWise.Domain.Interfaces.AppConfig
{
    public interface ITokenConfigurationDto
    {
        string Audience { get; set; }
        string Issuer { get; set; }
        string Secret { get; set; }
        int Minutes { get; set; }
        int DaysToExpiry { get; set; }
    }
}

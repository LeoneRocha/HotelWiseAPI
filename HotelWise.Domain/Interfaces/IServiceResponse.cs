namespace HotelWise.Domain.Interfaces
{
    public interface IServiceResponse<T>
    {
        T? Data { get; set; }
        bool Success { get; set; }
        string Message { get; set; }
    }
}

namespace HotelWise.Domain.Interfaces.IA
{
    public interface IDataVector
    {
        ulong DataKey { get; set; }
        ReadOnlyMemory<float> Embedding { get; set; }
        double Score { get; set; }
    }
}

namespace HotelWise.Core.SDK.AI.Abstractions;

/// <summary>
/// Contrato de dado vetorial genérico.
/// </summary>
public interface IDataVector
{
    ulong DataKey { get; set; }
    ReadOnlyMemory<float> Embedding { get; set; }
    double Score { get; set; }
    List<string> Tags { get; set; }
}

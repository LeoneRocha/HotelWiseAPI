using HotelWise.Domain.Model.HotelModels;

namespace HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Service;

/// <summary>
/// Contrato de serviço para geração sintética ou automática de dados de hotéis por IA ou mocks.
/// </summary>
public interface IGenerateHotelService
{
    /// <summary>
    /// Gera um lote de entidades de hotéis sintéticos.
    /// </summary>
    /// <param name="numberGerate">Quantidade de hotéis a serem gerados.</param>
    /// <returns>Array de hotéis gerados.</returns>
    Task<Hotel[]> GetHotelsAsync(int numberGerate);

    /// <summary>
    /// Gera uma única entidade de hotel sintético com dados plausíveis de acomodação.
    /// </summary>
    /// <returns>Instância de <see cref="Hotel"/> gerada.</returns>
    Task<Hotel> GetHotelAsync();
}
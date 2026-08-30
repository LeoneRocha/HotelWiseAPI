using HotelWise.Domain.Dto.Enitty.HotelDtos;

namespace HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Service;

/// <summary>
/// Contrato de serviço para gerenciamento e operações de negócio de estabelecimentos hoteleiros.
/// </summary>
public interface IHotelService : IGenericService<HotelDto>
{
    /// <summary>
    /// Indexa o hotel correspondente na base vetorial (Vector Store) para permitir busca por similaridade semântica.
    /// </summary>
    /// <param name="id">Identificador único do hotel.</param>
    /// <returns>Resposta indicando se a indexação vetorial foi concluída com sucesso.</returns>
    Task<ServiceResponse<bool>> InsertHotelInVectorStore(long id);

    /// <summary>
    /// Cadastra um novo hotel no sistema.
    /// </summary>
    /// <param name="hotelDto">Dados do hotel a ser adicionado.</param>
    /// <returns>Resposta com indicador de sucesso.</returns>
    Task<ServiceResponse<bool>> AddHotelAsync(HotelDto hotelDto);

    /// <summary>
    /// Remove um hotel pelo seu identificador.
    /// </summary>
    /// <param name="id">Identificador do hotel.</param>
    /// <returns>Resposta com indicador de sucesso.</returns>
    Task<ServiceResponse<bool>> DeleteHotelAsync(long id);

    /// <summary>
    /// Obtém a lista completa de todos os hotéis cadastrados.
    /// </summary>
    /// <returns>Resposta contendo o array de hotéis.</returns>
    Task<ServiceResponse<HotelDto[]>> GetAllHotelsAsync();

    /// <summary>
    /// Obtém os dados de um hotel específico por meio de seu identificador.
    /// </summary>
    /// <param name="id">Identificador do hotel.</param>
    /// <returns>Resposta contendo o DTO do hotel ou <c>null</c> se não encontrado.</returns>
    Task<ServiceResponse<HotelDto?>> GetHotelByIdAsync(long id);

    /// <summary>
    /// Atualiza os dados de um hotel existente.
    /// </summary>
    /// <param name="hotelDto">DTO com as informações atualizadas do hotel.</param>
    /// <returns>Resposta com indicador de sucesso.</returns>
    Task<ServiceResponse<bool>> UpdateHotelAsync(HotelDto hotelDto);

    /// <summary>
    /// Cria e persiste um novo hotel gerado automaticamente através de modelos de inteligência artificial.
    /// </summary>
    /// <returns>Resposta contendo os dados do hotel recém-gerado.</returns>
    Task<ServiceResponse<HotelDto>> GenerateHotelByIA();

    /// <summary>
    /// Obtém a lista de todas as tags distintas cadastradas entre todos os hotéis.
    /// </summary>
    /// <returns>Array contendo os nomes das tags encontradas.</returns>
    Task<string[]> GetAllTags();
}
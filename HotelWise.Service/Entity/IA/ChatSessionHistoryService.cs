using AutoMapper;
using FluentValidation;
using HotelWise.Domain.Dto.IA;
using HotelWise.Domain.Dto.IA.SemanticKernel;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Service;
using HotelWise.Domain.Interfaces.Entity.IA;
using HotelWise.Domain.Model.AI;

namespace HotelWise.Service.Entity;

/// <summary>
/// Serviço de aplicação para gerenciamento de histórico de conversas do assistente inteligente.
/// </summary>
public class ChatSessionHistoryService : GenericEntityServiceBase<ChatSessionHistory, ChatSessionHistoryDto>, IChatSessionHistoryService
{
    private readonly IChatSessionHistoryRepository _entitylRepository;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="ChatSessionHistoryService"/>.
    /// </summary>
    /// <param name="logger">Logger estruturado.</param>
    /// <param name="mapper">Mapeador AutoMapper.</param>
    /// <param name="applicationConfig">Configuração de IA.</param>
    /// <param name="entitylRepository">Repositório de histórico de sessões.</param>
    /// <param name="generateHotelService">Serviço de geração sintética de hotéis.</param>
    /// <param name="hotelVectorStoreService">Serviço de armazenamento vetorial.</param>
    /// <param name="_entityValidator">Validador FluentValidation de histórico de chat.</param>
    public ChatSessionHistoryService(
        Serilog.ILogger logger,
        IMapper mapper,
        IApplicationIAConfig applicationConfig,
        IChatSessionHistoryRepository entitylRepository,
        IGenerateHotelService generateHotelService,
        IVectorStoreService<HotelVector> hotelVectorStoreService,
        IValidator<ChatSessionHistory> _entityValidator)
        : base(entitylRepository, mapper, logger, _entityValidator)
    {
        _entitylRepository = entitylRepository;
    }

    /// <summary>
    /// Remove permanentemente o histórico de conversas pelo identificador GUID da sessão.
    /// </summary>
    /// <param name="token">GUID identificador da sessão.</param>
    public async Task DeleteByIdTokenAsync(string token)
    {
        await _entitylRepository.DeleteByIdTokenAsync(token);
    }

    /// <summary>
    /// Recupera o histórico de mensagens de uma sessão de conversa pelo seu token GUID.
    /// </summary>
    /// <param name="token">GUID identificador da sessão.</param>
    /// <returns>DTO contendo o histórico ou <c>null</c> se não encontrado.</returns>
    public async Task<ChatSessionHistoryDto?> GetByIdTokenAsync(string token)
    {
        var result = await _entitylRepository.GetByIdTokenAsync(token);
        if (result == null) { return null; }
        var resultDto = _mapper.Map<ChatSessionHistoryDto>(result);
        return resultDto;
    }
}
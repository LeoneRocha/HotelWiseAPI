#if NET8_0_OR_GREATER
using AutoMapper;
using FluentValidation;
using HotelWise.Core.SDK.Abstractions;

using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.Services;

/// <summary>
/// Serviço genérico de entidade — herda SCH (<see cref="SmartCoreHub.Core.SDK.Domain.Abstractions.IGenericService{TDto}"/>).
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Service.Services.Generic.GenericEntityServiceBase", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Service.Services.Generic.GenericEntityServiceBase em SmartCoreHub.Core.SDK.")]
public abstract class GenericEntityServiceBase<T, TDto>
    : SmartCoreHub.Core.SDK.Service.Services.Generic.GenericEntityServiceBase<T, TDto>
    where T : class, new()
    where TDto : class, new()
{
    /// <summary>
    /// Inicializa uma nova instância de <see cref="GenericEntityServiceBase{T, TDto}"/>.
    /// </summary>
    protected GenericEntityServiceBase(
        IGenericRepository<T> repository,
        IMapper mapper,
        Serilog.ILogger logger,
        IValidator<T> entityValidator)
        : base(repository, mapper, logger, entityValidator)
    {
    }
}
#endif

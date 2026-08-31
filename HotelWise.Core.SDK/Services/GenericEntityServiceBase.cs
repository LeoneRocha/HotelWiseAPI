#if NET8_0_OR_GREATER
using AutoMapper;
using FluentValidation;
using HotelWise.Core.SDK.Abstractions;
using HotelWise.Core.SDK.Common;

namespace HotelWise.Core.SDK.Services;

/// <summary>
/// Serviço genérico de entidade — casca sobre
/// <see cref="SmartCoreHub.Core.SDK.Service.Services.Generic.GenericEntityServiceBase{T, TDto}"/>.
/// Reexpõe <see cref="ServiceResponse{TDto}"/> HW para hosts que fazem override.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Service. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Service.Services.Generic.GenericEntityServiceBase. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public abstract class GenericEntityServiceBase<T, TDto>
    : SmartCoreHub.Core.SDK.Service.Services.Generic.GenericEntityServiceBase<T, TDto>,
      IGenericService<TDto>
    where T : class, new()
    where TDto : class, new()
{
    protected GenericEntityServiceBase(
        IGenericRepository<T> repository,
        IMapper mapper,
        Serilog.ILogger logger,
        IValidator<T> entityValidator)
        : base(repository, mapper, logger, entityValidator)
    {
    }

    /// <summary>Reexpõe CreateAsync com <see cref="ServiceResponse{TDto}"/> HW (hosts com override).</summary>
    public new virtual async Task<ServiceResponse<TDto>> CreateAsync(TDto entityDto)
        => ToHw(await base.CreateAsync(entityDto));

    /// <summary>Reexpõe UpdateAsync com <see cref="ServiceResponse{TDto}"/> HW (hosts com override).</summary>
    public new virtual async Task<ServiceResponse<TDto>> UpdateAsync(TDto entityDto)
        => ToHw(await base.UpdateAsync(entityDto));

    /// <summary>Reexpõe Validate com <see cref="ServiceResponse{TDto}"/> HW.</summary>
    public new virtual async Task<ServiceResponse<TDto>> Validate(T item)
        => ToHw(await base.Validate(item));

    private static ServiceResponse<TDto> ToHw(SmartCoreHub.Core.SDK.Common.ServiceResponse<TDto> sch)
    {
        var hw = new ServiceResponse<TDto>
        {
            Data = sch.Data,
            Success = sch.Success,
            Message = sch.Message,
            Unauthorized = sch.Unauthorized,
            Errors = new List<ErrorResponse>()
        };
        if (sch.Errors is { Count: > 0 })
        {
            foreach (var e in sch.Errors)
            {
                hw.Errors.Add(new ErrorResponse
                {
                    Name = e.Name,
                    Message = e.Message,
                    ErrorCode = e.ErrorCode,
                    DefaultMessage = e.DefaultMessage,
                    FullMessage = e.FullMessage
                });
            }
        }
        return hw;
    }
}
#endif

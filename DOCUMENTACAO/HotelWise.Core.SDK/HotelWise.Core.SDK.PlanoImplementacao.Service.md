# Plano de Implementação — Onda 3: HotelWise.Service → Core.SDK

**Versão:** 1.0.0  
**Data:** 2026-08-28  
**Plano Geral:** [HotelWise.Core.SDK.PlanoImplementacao.md](./HotelWise.Core.SDK.PlanoImplementacao.md)  
**Especificação:** [HotelWise.Core.SDK.Especificacao.Service.md](./HotelWise.Core.SDK.Especificacao.Service.md)  
**Pré-requisito:** Onda 1 (Domain) e Onda 2 (Data) concluídas.

---

## Resumo

| Métrica | Valor |
| :--- | :--- |
| Arquivos a portar | **11** |
| Arquivos mantidos no host | **14** |
| Lotes sequenciais | **2** (Lote S1 e Lote S2) |
| Estimativa | **2 dias** (1 dev) |

---

## Lote S1 — Serviços Genéricos Base e Autenticação JWT

**Dependências:** Onda 1 (Domain) e Onda 2 (Data) concluídas  
**Arquivos a portar:** 3

### 1. Tarefas Detalhadas

| # | Ação | Arquivo Origem no Host | Destino no Core.SDK | DiagnosticId |
| :--- | :--- | :--- | :--- | :--- |
| 1.1 | Portar classe canônica | `Entity/Generic/GenericEntityServiceBase.cs` | `Services/GenericEntityServiceBase.cs` | `HW_CORE_SDK_SERVICE` |
| 1.2 | Portar classe canônica | `Generic/GenericServiceBase.cs` | `AI/Services/GenericVectorStoreServiceBase.cs` | `HW_CORE_SDK_AI` |
| 1.3 | Portar classe canônica | `Security/TokenService.cs` | `Security/TokenService.cs` | `HW_CORE_SDK_SECURITY` |
| 1.4 | Adicionar `ProjectReference` | `HotelWise.Service/HotelWise.Service.csproj` | Referência para `HotelWise.Core.SDK` | — |
| 1.5 | Criar Shims `[Obsolete]` | 3 arquivos originais em `HotelWise.Service` | Shims finos delegando ao Core | `HW_CORE_SDK_*` |
| 1.6 | Atualizar herança nos serviços de domínio | 5 serviços de domínio em `HotelWise.Service` | Herdar de `GenericEntityServiceBase` canônico | — |
| 1.7 | Validar compilação | `HotelWise.Service.csproj` | Build verde | — |
| 1.8 | Testes Canônicos | `HotelWise.Core.SDK.Tests/` | `GenericEntityServiceBaseTests`, `TokenServiceTests` | — |

---

### 2. Implementações Canônicas no Core.SDK

#### `GenericEntityServiceBase<T, TDto>`
No arquivo `HotelWise.Core.SDK/Services/GenericEntityServiceBase.cs`:

```csharp
namespace HotelWise.Core.SDK.Services
{
    using AutoMapper;
    using FluentValidation;
    using HotelWise.Core.SDK.Abstractions;
    using HotelWise.Core.SDK.Common;
    using HotelWise.Core.SDK.Validation;
    using Serilog;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public abstract class GenericEntityServiceBase<T, TDto> : IGenericService<TDto>
        where T : class, new()
        where TDto : class, new()
    {
        protected readonly IGenericRepository<T> _repository;
        protected readonly IMapper _mapper;
        protected readonly ILogger _logger;
        protected readonly IValidator<T> _entityValidator;
        protected long UserId { get; private set; }

        protected GenericEntityServiceBase(
            IGenericRepository<T> repository, 
            IMapper mapper, 
            ILogger logger, 
            IValidator<T> entityValidator)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _entityValidator = entityValidator;
        }

        public void SetUserId(long id) => UserId = id;

        public virtual async Task<ServiceResponse<List<TDto>>> GetAllAsync()
        {
            var response = new ServiceResponse<List<TDto>>();
            try
            {
                var entities = await _repository.GetAllAsync();
                response.Data = _mapper.Map<List<TDto>>(entities);
                response.Success = true;
                response.Message = "Operação realizada com sucesso.";
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erro ao obter todos os registros.");
                response.Success = false;
                response.Errors = new List<ErrorResponse> { new ErrorResponse { Message = ex.Message } };
            }
            return response;
        }

        public virtual async Task<ServiceResponse<TDto?>> GetByIdAsync(long id)
        {
            var response = new ServiceResponse<TDto?>();
            try
            {
                var entity = await _repository.GetByIdAsync(id);
                response.Data = _mapper.Map<TDto?>(entity);
                response.Success = entity != null;
                response.Message = entity != null ? "Registro encontrado." : "Registro não encontrado.";
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erro ao obter registro por id {Id}.", id);
                response.Success = false;
                response.Errors = new List<ErrorResponse> { new ErrorResponse { Message = ex.Message } };
            }
            return response;
        }

        public virtual async Task<ServiceResponse<TDto>> CreateAsync(TDto item)
        {
            var response = new ServiceResponse<TDto>();
            try
            {
                var entity = _mapper.Map<T>(item);
                if (_entityValidator != null)
                {
                    var validationResult = await _entityValidator.ValidateAsync(entity);
                    if (!validationResult.IsValid)
                    {
                        response.Success = false;
                        response.Errors = HelperValidation.MapValidationErrors(validationResult);
                        return response;
                    }
                }

                var createdEntity = await _repository.AddAsync(entity);
                response.Data = _mapper.Map<TDto>(createdEntity);
                response.Success = true;
                response.Message = "Registro criado com sucesso.";
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erro ao criar registro.");
                response.Success = false;
                response.Errors = new List<ErrorResponse> { new ErrorResponse { Message = ex.Message } };
            }
            return response;
        }

        public virtual async Task<ServiceResponse<TDto>> UpdateAsync(TDto item)
        {
            var response = new ServiceResponse<TDto>();
            try
            {
                var entity = _mapper.Map<T>(item);
                if (_entityValidator != null)
                {
                    var validationResult = await _entityValidator.ValidateAsync(entity);
                    if (!validationResult.IsValid)
                    {
                        response.Success = false;
                        response.Errors = HelperValidation.MapValidationErrors(validationResult);
                        return response;
                    }
                }

                var updatedEntity = await _repository.UpdateAsync(entity);
                response.Data = _mapper.Map<TDto>(updatedEntity);
                response.Success = true;
                response.Message = "Registro atualizado com sucesso.";
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erro ao atualizar registro.");
                response.Success = false;
                response.Errors = new List<ErrorResponse> { new ErrorResponse { Message = ex.Message } };
            }
            return response;
        }

        public virtual async Task<ServiceResponse<bool>> DeleteAsync(long id)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                await _repository.DeleteAsync(id);
                response.Data = true;
                response.Success = true;
                response.Message = "Registro excluído com sucesso.";
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erro ao excluir registro {Id}.", id);
                response.Success = false;
                response.Data = false;
                response.Errors = new List<ErrorResponse> { new ErrorResponse { Message = ex.Message } };
            }
            return response;
        }

        public virtual async Task<ServiceResponse<int>> CountAsync()
        {
            var response = new ServiceResponse<int>();
            try
            {
                response.Data = await _repository.CountAsync();
                response.Success = true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erro ao contar registros.");
                response.Success = false;
                response.Errors = new List<ErrorResponse> { new ErrorResponse { Message = ex.Message } };
            }
            return response;
        }
    }
}
```

---

### 3. Padrão de Shim no Host (`HotelWise.Service`)

No arquivo `HotelWise.Service/Entity/Generic/GenericEntityServiceBase.cs`:

```csharp
namespace HotelWise.Service.Entity.Generic
{
    using AutoMapper;
    using FluentValidation;
    using HotelWise.Core.SDK.Abstractions;
    using CoreServices = HotelWise.Core.SDK.Services;

    // ⚠️ Movido para HotelWise.Core.SDK — implementação canônica no pacote Core.
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.Services.GenericEntityServiceBase<T, TDto>.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_SERVICE")]
    public abstract class GenericEntityServiceBase<T, TDto> : CoreServices.GenericEntityServiceBase<T, TDto>
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
    }
}
```

---

## Lote S2 — Fábricas de IA, Configuração Semantic Kernel e DI Extensions

**Dependências:** Lote S1 concluído  
**Arquivos a portar:** 8

### 1. Tarefas Detalhadas

| # | Ação | Arquivo Origem no Host | Destino no Core.SDK | DiagnosticId |
| :--- | :--- | :--- | :--- | :--- |
| 2.1 | Portar factory | `AI/AIInferenceAdapterFactory.cs` | `AI/Services/AIInferenceAdapterFactory.cs` | `HW_CORE_SDK_AI` |
| 2.2 | Portar service | `AI/AIInferenceService.cs` | `AI/Services/AIInferenceService.cs` | `HW_CORE_SDK_AI` |
| 2.3 | Portar factory | `AI/VectorStoreAdapterFactory.cs` | `AI/Services/VectorStoreAdapterFactory.cs` | `HW_CORE_SDK_AI` |
| 2.4 | Portar configure | `Configure/SemanticKernelProviderConfigure.cs` | `AI/Configure/SemanticKernelProviderConfigure.cs` | `HW_CORE_SDK_AI` |
| 2.5 | Portar configure | `Configure/ConfigureServicesAI.cs` | `AI/Configure/ConfigureServicesAI.cs` | `HW_CORE_SDK_AI` |
| 2.6 | Portar extensão DI | `Configure/ServiceCollectionConfigureCors.cs` | `Extensions/ServiceCollectionConfigureCors.cs` | `HW_CORE_SDK_DI` |
| 2.7 | Portar extensão DI | `Configure/ServiceCollectionConfigureAppSettings.cs` | `Extensions/ServiceCollectionConfigureAppSettings.cs` | `HW_CORE_SDK_DI` |
| 2.8 | Portar extensão DI | `Configure/ServiceCollectionConfigureAutoMapper.cs` | `Extensions/ServiceCollectionConfigureAutoMapper.cs` | `HW_CORE_SDK_DI` |
| 2.9 | Criar Shims `[Obsolete]` | 8 arquivos correspondentes no host | Delegações finas para o Core | `HW_CORE_SDK_*` |
| 2.10 | Validar compilação | `HotelWise.Service.csproj` e solução | Build verde | — |
| 2.11 | Testes Canônicos | `HotelWise.Core.SDK.Tests/` | `AIInferenceAdapterFactoryTests`, `VectorStoreAdapterFactoryTests` | — |

---

### 2. Itens Mantidos Intactos no Host (`HotelWise.Service`)

- **Serviços de Domínio:** `HotelService`, `RoomService`, `ReservationService`, `RoomAvailabilityService`, `UserService`.
- **Serviços de IA de Domínio:** `ChatSessionHistoryService`, `GenerateHotelService`, `AssistantService`, `HotelVectorStoreService`.
- **Lógica de Negócio e Prompts:** `HotelSearchService`, `HotelResponseProcessor`, `StayMatePromptGenerator`.
- **DI Wire-up de Domínio:** `ServicesDomainRepository`, `ServicesDomainService`, `ServiceCollectionConfigureServicesDomain`.

---

## 3. Testes Canônicos (`HotelWise.Core.SDK.Tests`)

| Arquivo de Teste | Escopo | Metodologia |
| :--- | :--- | :--- |
| `GenericEntityServiceBaseTests.cs` | Fluxo CRUD completo com mocks de `IGenericRepository<T>`, `IMapper`, `ILogger`, `IValidator<T>` | Moq + FluentAssertions |
| `GenericEntityServiceBaseTests.cs` | Validação positiva (sucesso) vs validação negativa (erros mapeados em `Errors`) | XUnit `[Fact]` |
| `GenericEntityServiceBaseTests.cs` | Resiliência e tratamento de exceção estruturado | Verificação de `ServiceResponse.Success == false` |
| `TokenServiceTests.cs` | Emissão de JWT token, validação de claims, expiração e assinatura | Mock de `ITokenConfigurationDto` |
| `AIInferenceAdapterFactoryTests.cs` | Instanciação correta dos adaptadores (`Groq`, `Mistral`, `Ollama`, `SemanticKernel`) por enum | `[Theory]` parametrizado |
| `VectorStoreAdapterFactoryTests.cs` | Instanciação correta dos adaptadores de VectorStore (`Qdrant`, `Memory`) | `[Theory]` parametrizado |

---

## 4. Critérios de Aceite da Onda 3

1. ✅ `GenericEntityServiceBase<T, TDto>`, `TokenService`, fábricas de IA e extensões de DI residem em `HotelWise.Core.SDK`.
2. ✅ Shims `[Obsolete]` com `DiagnosticId` apropriado adicionados a todos os 11 arquivos originais do host.
3. ✅ Todos os 14 serviços mantidos no host compilam e funcionam herdando do SDK.
4. ✅ `dotnet build HotelWise.Service/HotelWise.Service.csproj` compila com **0 erros**.
5. ✅ Suíte de testes atinge cobertura $\ge 90\%$.
6. ✅ Gate para Onda 4 (API) liberado.

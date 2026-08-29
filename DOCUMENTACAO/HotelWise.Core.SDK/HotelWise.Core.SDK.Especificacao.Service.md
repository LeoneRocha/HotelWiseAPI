# Especificação Técnica — HotelWise.Core.SDK (Módulo Service)

**Versão:** 2.0.0  
**Data:** 2026-08-28  
**Projeto de Origem:** `HotelWise.Service` (`HotelWise.Service.csproj`, TFM `net10.0`)  
**Projeto de Destino:** `HotelWise.Core.SDK`  
**Documento Principal:** [HotelWise.Core.SDK.Levantamento.md](./HotelWise.Core.SDK.Levantamento.md)

---

## 1. Papel do Módulo na Arquitetura

O projeto `HotelWise.Service` concentra orquestração de CRUD, integração com provedores de IA, geração de tokens JWT e configuração de DI. Referencia `HotelWise.Data` e `HotelWise.Domain`.

O objetivo é extrair os serviços genéricos reutilizáveis, o `TokenService`, as fábricas/adapters de IA e extensões de DI para o Core.SDK, mantendo no host os serviços de domínio hoteleiro.

### Dependências NuGet relevantes do .csproj

| Pacote | Impacto no Core.SDK |
| :--- | :--- |
| `AutoMapper` | Mapeamento DTO ↔ Entity em `GenericEntityServiceBase` |
| `FluentValidation` | Validação em `GenericEntityServiceBase` |
| `Microsoft.EntityFrameworkCore` | Referência indireta via repositórios |
| `Microsoft.SemanticKernel.Connectors.MistralAI` | Conector SK específico |
| `Microsoft.SemanticKernel.Connectors.Ollama` | Conector SK específico |
| `Azure.Storage.Blobs`, `Azure.Storage.Queues` | Cloud storage |
| `Azure.ResourceManager.Authorization` | Gestão de autorização Azure |

---

## 2. Inventário Completo — Arquivos .cs (excluindo /obj/)

> Total: **25 arquivos** fonte no projeto.

---

### 2.1 Portar + Obsoletar → Core.SDK (11 arquivos)

#### Serviços Genéricos Base (2 arquivos)

| Arquivo | Tipo | Destino Core.SDK | DiagnosticId |
| :--- | :--- | :--- | :--- |
| `Entity/Generic/GenericEntityServiceBase.cs` | `GenericEntityServiceBase<T, TDto>` | `Services/` | `HW_CORE_SDK_SERVICE` |
| `Generic/GenericServiceBase.cs` | `GenericVectorStoreServiceBase` | `AI/Services/` | `HW_CORE_SDK_AI` |

#### Segurança JWT (1 arquivo)

| Arquivo | Tipo | Destino Core.SDK | DiagnosticId |
| :--- | :--- | :--- | :--- |
| `Security/TokenService.cs` | `TokenService` | `Security/` | `HW_CORE_SDK_SECURITY` |

#### Fábricas e Serviços de IA (3 arquivos)

| Arquivo | Tipo | Destino Core.SDK | DiagnosticId |
| :--- | :--- | :--- | :--- |
| `AI/AIInferenceAdapterFactory.cs` | `AIInferenceAdapterFactory` | `AI/Services/` | `HW_CORE_SDK_AI` |
| `AI/AIInferenceService.cs` | `AIInferenceService` | `AI/Services/` | `HW_CORE_SDK_AI` |
| `AI/VectorStoreAdapterFactory.cs` | `VectorStoreAdapterFactory` | `AI/Services/` | `HW_CORE_SDK_AI` |

#### Configuração Semantic Kernel (2 arquivos)

| Arquivo | Tipo | Destino Core.SDK | DiagnosticId |
| :--- | :--- | :--- | :--- |
| `Configure/SemanticKernelProviderConfigure.cs` | `SemanticKernelProviderConfigure` | `AI/Configure/` | `HW_CORE_SDK_AI` |
| `Configure/ConfigureServicesAI.cs` | `ConfigureServicesAI` | `AI/Configure/` | `HW_CORE_SDK_AI` |

#### Extensões de DI Genéricas (3 arquivos)

| Arquivo | Tipo | Destino Core.SDK | DiagnosticId |
| :--- | :--- | :--- | :--- |
| `Configure/ServiceCollectionConfigureCors.cs` | `ServiceCollectionConfigureCors` | `Extensions/` | `HW_CORE_SDK_DI` |
| `Configure/ServiceCollectionConfigureAppSettings.cs` | `ServiceCollectionConfigureAppSettings` | `Extensions/` | `HW_CORE_SDK_DI` |
| `Configure/ServiceCollectionConfigureAutoMapper.cs` | `ServiceCollectionConfigureAutoMapper` | `Extensions/` | `HW_CORE_SDK_DI` |

---

### 2.2 Manter no Host (14 arquivos)

#### Serviços de Domínio Hoteleiro (5 arquivos)

| Arquivo | Tipo | Motivo |
| :--- | :--- | :--- |
| `Entity/HotelServices/HotelService.cs` | `HotelService` | Lógica de negócio de hotéis, geração de vetores |
| `Entity/HotelServices/RoomService.cs` | `RoomService` | Gestão de quartos |
| `Entity/HotelServices/ReservationService.cs` | `ReservationService` | Lógica de reservas |
| `Entity/HotelServices/RoomAvailabilityService.cs` | `RoomAvailabilityService` | Disponibilidade e precificação |
| `Entity/UserService.cs` | `UserService` | Autenticação e gestão de usuários |

#### Serviços de IA de Domínio (3 arquivos)

| Arquivo | Tipo | Motivo |
| :--- | :--- | :--- |
| `Entity/IA/ChatSessionHistoryService.cs` | `ChatSessionHistoryService` | Sessões de conversação |
| `Entity/GenerateHotelService.cs` | `GenerateHotelService` | Geração sintética de hotéis via IA |
| `AI/AssistantService.cs` | `AssistantService` | Orquestração do agente StayMate |

#### Serviço de Vector Store de Domínio (1 arquivo)

| Arquivo | Tipo | Motivo |
| :--- | :--- | :--- |
| `AI/HotelVectorStoreService.cs` | `HotelVectorStoreService` | Indexação vetorial de hotéis no Qdrant/Memory |

#### Lógica de Negócio e Prompts (3 arquivos)

| Arquivo | Tipo | Motivo |
| :--- | :--- | :--- |
| `Bussines/HotelSearchService.cs` | `HotelSearchService` | Busca semântica de hotéis |
| `Bussines/HotelResponseProcessor.cs` | `HotelResponseProcessor` | Formatação de respostas IA |
| `Bussines/StayMatePromptGenerator.cs` | `StayMatePromptGenerator` | System prompts do agente StayMate |

#### DI Wire-up de Domínio (3 arquivos)

| Arquivo | Tipo | Motivo |
| :--- | :--- | :--- |
| `Configure/ServicesDomainRepository.cs` | `ServicesDomainRepository` | Registro de repositórios de domínio |
| `Configure/ServicesDomainService.cs` | `ServicesDomainService` | Registro de serviços de domínio |
| `Configure/ServiceCollectionConfigureServicesDomain.cs` | `ServiceCollectionConfigureServicesDomain` | Orquestrador DI do host |

---

## 3. Detalhamento Técnico — `GenericEntityServiceBase<T, TDto>`

### Responsabilidades
- CRUD completo: `GetAllAsync`, `GetByIdAsync`, `FindAsync`, `CreateAsync`, `AddRangeAsync`, `UpdateAsync`, `UpdateRangeAsync`, `DeleteAsync`, `CountAsync`, `FetchAsync`
- Validação automática via `FluentValidation.IValidator<T>` → `Validate(T item)`
- Mapeamento entidade ↔ DTO via `AutoMapper.IMapper`
- Respostas encapsuladas em `ServiceResponse<TDto>`
- Contexto do chamador via `SetUserId(long id)`
- Tratamento de exceções com `Serilog.ILogger`

### Estrutura Canônica no Core.SDK
```csharp
namespace HotelWise.Core.SDK.Services
{
    using AutoMapper;
    using FluentValidation;
    using HotelWise.Core.SDK.Abstractions;
    using HotelWise.Core.SDK.Common;

    public abstract class GenericEntityServiceBase<T, TDto> : IGenericService<TDto>
        where T : class, new()
        where TDto : class, new()
    {
        protected readonly IGenericRepository<T> _repository;
        protected readonly IMapper _mapper;
        protected readonly Serilog.ILogger _logger;
        protected readonly IValidator<T> _entityValidator;
        protected long UserId { get; private set; }

        protected GenericEntityServiceBase(
            IGenericRepository<T> repository,
            IMapper mapper,
            Serilog.ILogger logger,
            IValidator<T> entityValidator)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _entityValidator = entityValidator;
        }

        public void SetUserId(long id) => UserId = id;

        // Implementações dos métodos assíncronos com try/catch, log e validação...
    }
}
```

---

## 4. Detalhamento Técnico — `TokenService`

Implementa `ITokenService` (definido em `Interfaces/AppConfig/`), centralizando geração de tokens JWT:

```csharp
namespace HotelWise.Core.SDK.Security
{
    using System.Security.Claims;

    public class TokenService : ITokenService
    {
        private readonly ITokenConfigurationDto _tokenConfigurations;

        public TokenService(ITokenConfigurationDto tokenConfigurations)
        {
            _tokenConfigurations = tokenConfigurations
                ?? throw new ArgumentNullException(nameof(tokenConfigurations));
        }

        public string GenerateAccessToken(IEnumerable<Claim> claims) { /* ... */ }
        public string GenerateRefreshToken() { /* ... */ }
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token) { /* ... */ }
    }
}
```

---

## 5. Detalhamento Técnico — Fábricas de IA

### `AIInferenceAdapterFactory` + `AIInferenceService`
Seleção dinâmica de provedor LLM em runtime:
- `InferenceAiAdapterType.GroqApi` → `GroqApiAdapter`
- `InferenceAiAdapterType.Mistral` → `MistralApiAdapter`
- `InferenceAiAdapterType.Ollama` → `OllamaAdapter`
- `InferenceAiAdapterType.SemanticKernel` → `SemanticKernelAdapter`

### `VectorStoreAdapterFactory`
Instancia `IVectorStoreAdapter<TVector>` baseado em `VectorStoreType` (`Qdrant`, `Memory`, etc.).

---

## 6. Padrão de Shim no Host

```csharp
namespace HotelWise.Service.Entity.Generic
{
    using AutoMapper;
    using FluentValidation;
    using HotelWise.Core.SDK.Abstractions;    // IGenericRepository<T>
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

> **Nota:** A referência `HotelWise.Domain.Interfaces.Entity.HotelWise.Domain.Interfaces.Entity` (namespace aninhado duplicado) do arquivo original **não** é replicada no shim. O shim usa `HotelWise.Core.SDK.Abstractions.IGenericRepository<T>` diretamente.

---

## 7. Adaptação dos Serviços de Domínio

```csharp
using AutoMapper;
using FluentValidation;
using HotelWise.Core.SDK.Services;    // GenericEntityServiceBase<T, TDto>
using HotelWise.Domain.Dto.Enitty.HotelDtos;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Repository;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Service;
using HotelWise.Domain.Model.HotelModels;

namespace HotelWise.Service.Entity.HotelServices
{
    public class HotelService : GenericEntityServiceBase<Hotel, HotelDto>, IHotelService
    {
        private readonly IHotelRepository _hotelRepository;

        public HotelService(
            IHotelRepository hotelRepository,
            IMapper mapper,
            Serilog.ILogger logger,
            IValidator<Hotel> entityValidator)
            : base(hotelRepository, mapper, logger, entityValidator)
        {
            _hotelRepository = hotelRepository;
        }

        // Métodos específicos do domínio de hotéis...
    }
}
```

---

## 8. Plano de Testes (`HotelWise.Core.SDK.Tests`)

| Suite | Foco | Estratégia |
| :--- | :--- | :--- |
| `GenericEntityServiceBaseTests.cs` | CRUD com mocks de `IGenericRepository<T>`, `IMapper`, `ILogger`, `IValidator<T>` | Moq + FluentAssertions |
| `GenericEntityServiceBaseTests.cs` | `CreateAsync` com validação OK vs falha | Caso positivo/negativo |
| `GenericEntityServiceBaseTests.cs` | Tratamento de exceções | Exception flow |
| `TokenServiceTests.cs` | Emissão JWT, expiração, claims, assinatura válida | Mock de `ITokenConfigurationDto` |
| `AIInferenceAdapterFactoryTests.cs` | Criação de instâncias corretas por `InferenceAiAdapterType` | Parametrizado por enum |
| `VectorStoreAdapterFactoryTests.cs` | Instância correta por `VectorStoreType` | Parametrizado por enum |

---

## 9. Checklist de Implementação

- [ ] Criar `GenericEntityServiceBase<T, TDto>` em `HotelWise.Core.SDK/Services/`
- [ ] Criar `GenericVectorStoreServiceBase` em `HotelWise.Core.SDK/AI/Services/`
- [ ] Criar `TokenService` em `HotelWise.Core.SDK/Security/`
- [ ] Criar `AIInferenceAdapterFactory`, `AIInferenceService`, `VectorStoreAdapterFactory` em `HotelWise.Core.SDK/AI/Services/`
- [ ] Criar `SemanticKernelProviderConfigure`, `ConfigureServicesAI` em `HotelWise.Core.SDK/AI/Configure/`
- [ ] Criar extensões DI em `HotelWise.Core.SDK/Extensions/`
- [ ] Adicionar `[Obsolete]` e shims em **11 arquivos** de `HotelWise.Service`
- [ ] Adicionar `ProjectReference` para `HotelWise.Core.SDK` em `HotelWise.Service.csproj`
- [ ] Atualizar `using`s nos 14 serviços de domínio mantidos no host
- [ ] Implementar suíte de testes com cobertura ≥ 90%
- [ ] `dotnet build HotelWise.Service/HotelWise.Service.csproj` verde

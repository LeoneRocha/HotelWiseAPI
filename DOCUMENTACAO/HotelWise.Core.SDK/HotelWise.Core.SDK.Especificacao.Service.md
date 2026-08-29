# Especificação Técnica — HotelWise.Core.SDK (Módulo Service)

**Versão:** 1.0.0  
**Data:** 2026-08-28  
**Projeto de Origem:** `HotelWise.Service` (`HotelWise.Service.csproj`)  
**Projeto de Destino:** `HotelWise.Core.SDK`  
**Documento Principal:** [HotelWise.Core.SDK.Levantamento.md](./HotelWise.Core.SDK.Levantamento.md)

---

## 1. Papel do Módulo na Arquitetura

O projeto `HotelWise.Service` concentra as regras de negócio, a orquestração de CRUD genérico para DTOs, a integração com provedores de Inteligência Artificial e Semantic Kernel, geração de tokens JWT e a configuração central de Injeção de Dependências (DI) da solução.

O objetivo desta especificação é definir a extração dos serviços genéricos reutilizáveis (`GenericEntityServiceBase<T, TDto>`, `GenericVectorStoreServiceBase`), o serviço de autenticação JWT (`TokenService`), as fábricas e adaptadores de IA (`AIInferenceAdapterFactory`, `VectorStoreAdapterFactory`, `AIInferenceService`) e extensões de DI para o `HotelWise.Core.SDK`, mantendo no host apenas os serviços que implementam regras exclusivas do domínio hoteleiro e do assistente virtual StayMate.

---

## 2. Inventário de Tipos de `HotelWise.Service`

### 2.1 Tipos para Portar + Obsoletar (Canônicos no Core.SDK)

| Tipo Original | Caminho no Host | Caminho no Core.SDK | DiagnosticId |
| :--- | :--- | :--- | :--- |
| `GenericEntityServiceBase<T, TDto>` | `Entity/Generic/GenericEntityServiceBase.cs` | `Services/GenericEntityServiceBase.cs` | `HW_CORE_SDK_SERVICE` |
| `GenericVectorStoreServiceBase` | `Generic/GenericServiceBase.cs` | `AI/Services/GenericVectorStoreServiceBase.cs` | `HW_CORE_SDK_AI` |
| `AIInferenceAdapterFactory` | `AI/AIInferenceAdapterFactory.cs` | `AI/Services/AIInferenceAdapterFactory.cs` | `HW_CORE_SDK_AI` |
| `AIInferenceService` | `AI/AIInferenceService.cs` | `AI/Services/AIInferenceService.cs` | `HW_CORE_SDK_AI` |
| `VectorStoreAdapterFactory` | `AI/VectorStoreAdapterFactory.cs` | `AI/Services/VectorStoreAdapterFactory.cs` | `HW_CORE_SDK_AI` |
| `TokenService` | `Security/TokenService.cs` | `Security/TokenService.cs` | `HW_CORE_SDK_SECURITY` |
| `SemanticKernelProviderConfigure` | `Configure/SemanticKernelProviderConfigure.cs` | `AI/Configure/SemanticKernelProviderConfigure.cs` | `HW_CORE_SDK_AI` |
| `ConfigureServicesAI` | `Configure/ConfigureServicesAI.cs` | `AI/Configure/ConfigureServicesAI.cs` | `HW_CORE_SDK_AI` |
| `ServiceCollectionConfigureCors` | `Configure/ServiceCollectionConfigureCors.cs` | `Extensions/ServiceCollectionConfigureCors.cs` | `HW_CORE_SDK_DI` |
| `ServiceCollectionConfigureAppSettings` | `Configure/ServiceCollectionConfigureAppSettings.cs` | `Extensions/ServiceCollectionConfigureAppSettings.cs` | `HW_CORE_SDK_DI` |
| `ServiceCollectionConfigureAutoMapper` | `Configure/ServiceCollectionConfigureAutoMapper.cs` | `Extensions/ServiceCollectionConfigureAutoMapper.cs` | `HW_CORE_SDK_DI` |

---

### 2.2 Tipos para Manter no Host (`HotelWise.Service`)

| Tipo / Pasta | Caminho no Host | Motivo da Permanência |
| :--- | :--- | :--- |
| `HotelService` | `Entity/HotelServices/HotelService.cs` | Lógica de negócio, catálogo de hotéis e geração de vetores. |
| `RoomService` | `Entity/HotelServices/RoomService.cs` | Gestão de quartos e comodidades de hotéis. |
| `ReservationService` | `Entity/HotelServices/ReservationService.cs` | Lógica de reserva, cálculo de datas e status. |
| `RoomAvailabilityService` | `Entity/HotelServices/RoomAvailabilityService.cs` | Motor de busca de disponibilidade e precificação dinâmica. |
| `UserService` | `Entity/UserService.cs` | Autenticação de credenciais, validação de usuário e geração de claims. |
| `ChatSessionHistoryService` | `Entity/IA/ChatSessionHistoryService.cs` | Persistência de sessões de conversação do usuário. |
| `GenerateHotelService` | `Entity/GenerateHotelService.cs` | Geração sintética de dados hoteleiros via IA. |
| `HotelSearchService` | `Bussines/HotelSearchService.cs` | Busca semântica e vetorizada de hotéis usando critérios de negócio. |
| `HotelResponseProcessor` | `Bussines/HotelResponseProcessor.cs` | Processamento de resposta de IA formatada para o assistente de hotel. |
| `StayMatePromptGenerator` | `Bussines/StayMatePromptGenerator.cs` | Construção de system prompts do agente StayMate. |
| `AssistantService` | `AI/AssistantService.cs` | Orquestração do agente conversacional de hotelaria. |
| `HotelVectorStoreService` | `AI/HotelVectorStoreService.cs` | Indexação vetorial específica de hotéis no Qdrant/Memory. |
| `ServicesDomainRepository` | `Configure/ServicesDomainRepository.cs` | Registro no DI dos repositórios concretos de domínio. |
| `ServicesDomainService` | `Configure/ServicesDomainService.cs` | Registro no DI dos serviços de domínio hoteleiro. |
| `ServiceCollectionConfigureServicesDomain` | `Configure/ServiceCollectionConfigureServicesDomain.cs` | Orquestrador de injeção de dependência do host. |

---

## 3. Detalhamento Técnico das Extrações

### 3.1 `GenericEntityServiceBase<T, TDto>`

#### Responsabilidades
- Fornece operações completas de CRUD (`GetAllAsync`, `GetByIdAsync`, `FindAsync`, `CreateAsync`, `AddRangeAsync`, `UpdateAsync`, `UpdateRangeAsync`, `DeleteAsync`, `CountAsync`, `FetchAsync`).
- Realiza validação assíncrona automática via `FluentValidation.IValidator<T>` através do método `Validate(T item)`.
- Mapeia entidades para DTOs e vice-versa via `AutoMapper.IMapper`.
- Retorna respostas encapsuladas e padronizadas no `ServiceResponse<TDto>`.
- Gerencia contexto do usuário chamador via `SetUserId(long id)`.
- Trata e registra exceções estruturadas via `Serilog.ILogger`.

#### Estrutura Canônica no Core.SDK
```csharp
namespace HotelWise.Core.SDK.Services
{
    using AutoMapper;
    using FluentValidation;
    using HotelWise.Core.SDK.Abstractions;
    using HotelWise.Core.SDK.Common;
    using HotelWise.Core.SDK.Validation;
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

        // Implementações completas dos métodos assíncronos protegidos com try/catch e log...
    }
}
```

---

### 3.2 `TokenService`

O serviço `TokenService` implementa `ITokenService` e centraliza a geração de tokens JWT autenticados utilizando a chave de assinatura, issuer e audience configurados via `ITokenConfigurationDto` e `SecurityHelper`:

```csharp
namespace HotelWise.Core.SDK.Security
{
    using HotelWise.Core.SDK.Abstractions;
    using HotelWise.Core.SDK.Common;
    using System;

    public class TokenService : ITokenService
    {
        private readonly ITokenConfigurationDto _tokenConfigurations;

        public TokenService(ITokenConfigurationDto tokenConfigurations)
        {
            _tokenConfigurations = tokenConfigurations ?? throw new ArgumentNullException(nameof(tokenConfigurations));
        }

        public TokenVO GenerateToken(UserLoginDto userLoginDto)
        {
            // Validações e geração de JWT token...
        }
    }
}
```

---

### 3.3 Módulo de Inferência de IA e Fábricas de Vetores

#### `AIInferenceAdapterFactory` & `AIInferenceService`
Permitem selecionar dinamicamente o provedor de inferência LLM em tempo de execução:
- `InferenceAiAdapterType.GroqApi` $\rightarrow$ `GroqApiAdapter`
- `InferenceAiAdapterType.Mistral` $\rightarrow$ `MistralApiAdapter`
- `InferenceAiAdapterType.Ollama` $\rightarrow$ `OllamaAdapter`
- `InferenceAiAdapterType.SemanticKernel` $\rightarrow$ `SemanticKernelAdapter`

#### `VectorStoreAdapterFactory`
Instancia adaptadores de `IVectorStoreAdapter<TVector>` baseados na configuração RAG selecionada (`VectorStoreType.Qdrant`, `VectorStoreType.Memory`, etc.).

---

## 4. Padrão de Shim no Host (`HotelWise.Service`)

O arquivo original em `HotelWise.Service/Entity/Generic/GenericEntityServiceBase.cs` será mantido com `[Obsolete]` e herança direta da classe canônica do Core:

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

> **Nota:** A referência `HotelWise.Domain.Interfaces.Entity.HotelWise.Domain.Interfaces.Entity` (namespace aninhado duplicado) que existia no arquivo original **não** deve ser replicada no shim. O shim usa `HotelWise.Core.SDK.Abstractions.IGenericRepository<T>` diretamente.

---

## 5. Serviços de Domínio no Host

Os serviços concretos de negócio (`HotelService`, `RoomService`, `ReservationService`, etc.) continuam em `HotelWise.Service`, herdando a base genérica canônica do SDK:

```csharp
using AutoMapper;
using FluentValidation;
using HotelWise.Core.SDK.Services;
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

## 6. Plano de Testes Canônicos (`HotelWise.Core.SDK.Tests`)

1. **`GenericEntityServiceBaseTests.cs`:**
   - Teste de fluxo completo com mocks de `IGenericRepository<T>`, `IMapper`, `ILogger` e `IValidator<T>`.
   - Teste do método `CreateAsync`: caso de validação bem-sucedida vs caso de falha de validação (erros mapeados).
   - Teste do método `UpdateAsync`, `DeleteAsync`, `GetAllAsync`, `GetByIdAsync`.
   - Teste de resiliência e tratamento de exceções com `LogAndThrow`.
2. **`TokenServiceTests.cs`:**
   - Validação da emissão de tokens JWT com expiração, claims corretas e assinatura válida.
3. **`AIInferenceAdapterFactoryTests.cs`:**
   - Verificação da criação de instâncias de adaptadores corretos de acordo com o enum `InferenceAiAdapterType`.

---

## 7. Checklist de Implementação

- [ ] Criar classe canônica `GenericEntityServiceBase<T, TDto>` em `HotelWise.Core.SDK/Services/`.
- [ ] Criar `GenericVectorStoreServiceBase`, `AIInferenceAdapterFactory` e `VectorStoreAdapterFactory` em `HotelWise.Core.SDK/AI/Services/`.
- [ ] Criar `TokenService` em `HotelWise.Core.SDK/Security/`.
- [ ] Migrar extensões de configuração de DI genéricas para `HotelWise.Core.SDK/Extensions/`.
- [ ] Adicionar anotações `[Obsolete]` e shims nos arquivos correspondentes em `HotelWise.Service`.
- [ ] Adicionar `ProjectReference` para `HotelWise.Core.SDK` em `HotelWise.Service.csproj`.
- [ ] Atualizar `using`s nos serviços de domínio (`HotelService`, `RoomService`, etc.).
- [ ] Implementar suíte de testes em `HotelWise.Core.SDK.Tests` com cobertura $\ge 90\%$.
- [ ] Executar `dotnet build HotelWise.Service/HotelWise.Service.csproj` e verificar compilação sem erros.

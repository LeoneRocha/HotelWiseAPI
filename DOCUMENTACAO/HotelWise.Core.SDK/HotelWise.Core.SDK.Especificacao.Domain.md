# Especificação Técnica — HotelWise.Core.SDK (Módulo Domain)

**Versão:** 1.0.0  
**Data:** 2026-08-28  
**Projeto de Origem:** `HotelWise.Domain` (`HotelWise.Domain.csproj`)  
**Projeto de Destino:** `HotelWise.Core.SDK`  
**Documento Principal:** [HotelWise.Core.SDK.Levantamento.md](./HotelWise.Core.SDK.Levantamento.md)

---

## 1. Papel do Módulo na Arquitetura

O projeto `HotelWise.Domain` é o núcleo de modelos, abstrações, contratos, DTOs, validadores, utilitários, conectores de IA e middlewares HTTP da aplicação HotelWise.

Por concentrar a maior parte das estruturas transversais da solução, o `HotelWise.Domain` representa a **primeira onda de migração** após o scaffolding inicial do `HotelWise.Core.SDK`. Sem a prévia extração e disponibilização dos tipos canônicos do Domain no SDK, as camadas `Data`, `Service` e `API` não podem ser migradas com segurança.

---

## 2. Inventário Completo e Classificação dos Tipos

### 2.1 Entidades Base e Contratos (Portar + Obsoletar)

| Tipo Original | Caminho no Host | Caminho no Core.SDK | DiagnosticId |
| :--- | :--- | :--- | :--- |
| `EntityBase` | `Model/EntityBase.cs` | `Domain/EntityBase.cs` | `HW_CORE_SDK_DOMAIN` |
| `EntityBaseWithNameEmail` | `Model/EntityBaseWithNameEmail.cs` | `Domain/EntityBaseWithNameEmail.cs` | `HW_CORE_SDK_DOMAIN` |
| `IEntityBase` | `Interfaces/Base/IEntityBase.cs` | `Abstractions/IEntityBase.cs` | `HW_CORE_SDK_DOMAIN` |
| `IEntityBaseLog` | `Interfaces/Base/IEntityBaseLog.cs` | `Abstractions/IEntityBaseLog.cs` | `HW_CORE_SDK_DOMAIN` |
| `IEntityFieldBaseLog` | `Interfaces/Base/IEntityFieldBaseLog.cs` | `Abstractions/IEntityFieldBaseLog.cs` | `HW_CORE_SDK_DOMAIN` |
| `IEntityDto` | `Interfaces/Base/IEntityDto.cs` | `Abstractions/IEntityDto.cs` | `HW_CORE_SDK_DOMAIN` |
| `IGenericRepository<T>` | `Interfaces/Base/IGenericRepository.cs` | `Abstractions/IGenericRepository.cs` | `HW_CORE_SDK_REPO` |
| `IGenericService<TDto>` | `Interfaces/Base/IGenericService.cs` | `Abstractions/IGenericService.cs` | `HW_CORE_SDK_SERVICE` |
| `IServiceResponse` | `Interfaces/Base/IServiceResponse.cs` | `Abstractions/IServiceResponse.cs` | `HW_CORE_SDK_COMMON` |

> **Nota de Correção:** O arquivo original `IGenericRepository.cs` no host continha declaração aninhada duplicada (`namespace HotelWise.Domain.Interfaces.Entity { namespace HotelWise.Domain.Interfaces.Entity { ... } }`). A versão canônica no Core.SDK corrigirá a estrutura para `namespace HotelWise.Core.SDK.Abstractions`.

---

### 2.2 DTOs Transversais e Response Pattern (Portar + Obsoletar)

| Tipo Original | Caminho no Host | Caminho no Core.SDK | DiagnosticId |
| :--- | :--- | :--- | :--- |
| `ServiceResponse<T>` | `Dto/ServiceResponse.cs` | `Common/ServiceResponse.cs` | `HW_CORE_SDK_COMMON` |
| `ErrorResponse` | `Dto/ErrorResponse.cs` | `Common/ErrorResponse.cs` | `HW_CORE_SDK_COMMON` |
| `EntityDtoBase` | `Dto/Base/EntityDtoBase.cs` | `Common/EntityDtoBase.cs` | `HW_CORE_SDK_COMMON` |
| `SecurityDto` | `Dto/SecurityDto.cs` | `Common/SecurityDto.cs` | `HW_CORE_SDK_COMMON` |
| `CultureDisplayDto` | `Dto/CultureDisplayDto.cs` | `Common/CultureDisplayDto.cs` | `HW_CORE_SDK_COMMON` |
| `TimeZoneDisplayDto` | `Dto/TimeZoneDisplayDto.cs` | `Common/TimeZoneDisplayDto.cs` | `HW_CORE_SDK_COMMON` |
| `RepositoryInfo` | `Dto/RepositoryInfo.cs` | `Common/RepositoryInfo.cs` | `HW_CORE_SDK_COMMON` |
| `AppInformationVersionProductDto` | `Dto/AppInformationVersionProductDto.cs` | `Common/AppInformationVersionProductDto.cs` | `HW_CORE_SDK_COMMON` |
| `TokenConfigurationDto` | `Dto/AppConfig/TokenConfigurationDto.cs` | `Security/TokenConfigurationDto.cs` | `HW_CORE_SDK_SECURITY` |
| `TokenVO` | `Dto/AppConfig/TokenVO.cs` | `Security/TokenVO.cs` | `HW_CORE_SDK_SECURITY` |

---

### 2.3 Utilitários e Helpers (Portar + Obsoletar)

| Tipo Original | Caminho no Host | Caminho no Core.SDK | DiagnosticId |
| :--- | :--- | :--- | :--- |
| `DataHelper` | `Helpers/DataHelper.cs` | `Helpers/DataHelper.cs` | `HW_CORE_SDK_HELPER` |
| `CultureDateTimeHelper` | `Helpers/CultureDateTimeHelper.cs` | `Helpers/CultureDateTimeHelper.cs` | `HW_CORE_SDK_HELPER` |
| `TimeFormatter` | `Helpers/TimeFormatter.cs` | `Helpers/TimeFormatter.cs` | `HW_CORE_SDK_HELPER` |
| `MarkdownHelper` | `Helpers/MarkdownHelper.cs` | `Helpers/MarkdownHelper.cs` | `HW_CORE_SDK_HELPER` |
| `HtmlHelper` | `Helpers/HtmlHelper.cs` | `Helpers/HtmlHelper.cs` | `HW_CORE_SDK_HELPER` |
| `HelperValidation` | `Helpers/HelperValidation.cs` | `Validation/HelperValidation.cs` | `HW_CORE_SDK_HELPER` |
| `SecurityHelper` | `Helpers/SecurityHelper.cs` | `Security/SecurityHelper.cs` | `HW_CORE_SDK_SECURITY` |
| `SecurityHelperApi` | `Helpers/SecurityHelperApi.cs` | `Security/SecurityHelperApi.cs` | `HW_CORE_SDK_SECURITY` |
| `ServiceCollectionHelper` | `Helpers/ServiceCollectionHelper.cs` | `Extensions/ServiceCollectionHelper.cs` | `HW_CORE_SDK_HELPER` |
| `EnumExtensions` | `Helpers/EnumExtensions.cs` | `Extensions/EnumExtensions.cs` | `HW_CORE_SDK_HELPER` |
| `LogAppHelper` | `Helpers/LogAppHelper.cs` | `Logging/LogAppHelper.cs` | `HW_CORE_SDK_LOGGING` |
| `ConfigurationAppSettingsHelper` | `Helpers/ConfigurationAppSettingsHelper.cs` | `Helpers/ConfigurationAppSettingsHelper.cs` | `HW_CORE_SDK_HELPER` |

---

### 2.4 Middlewares e Exceções (Portar + Obsoletar)

| Tipo Original | Caminho no Host | Caminho no Core.SDK | DiagnosticId |
| :--- | :--- | :--- | :--- |
| `CorrelationIdMiddleware` | `CustomMiddleware/CorrelationIdMiddleware.cs` | `Infrastructure/Middleware/CorrelationIdMiddleware.cs` | `HW_CORE_SDK_MIDDLEWARE` |
| `GlobalExceptionMiddleware` | `CustomMiddleware/GlobalExceptionMiddleware.cs` | `Infrastructure/Middleware/GlobalExceptionMiddleware.cs` | `HW_CORE_SDK_MIDDLEWARE` |
| `RequestLoggingMiddleware` | `CustomMiddleware/RequestLoggingMiddleware.cs` | `Infrastructure/Middleware/RequestLoggingMiddleware.cs` | `HW_CORE_SDK_MIDDLEWARE` |
| `AppWarningException` | `AppException/AppWarningException.cs` | `Common/Exceptions/AppWarningException.cs` | `HW_CORE_SDK_COMMON` |
| `AppConfigConstants`, `ValidatorConstants`, `AzureADEntraIDConstants`, `EntityTypeConfigurationConstants` | `Constants/` | `Common/Constants/` | `HW_CORE_SDK_COMMON` |
| `ChatCompletionValidatorsConstants` | `Constants/IA/` | `AI/Constants/` | `HW_CORE_SDK_AI` |

---

### 2.5 Módulo de IA, Vector Store e Semantic Kernel (Portar + Obsoletar)

| Tipo Original | Caminho no Host | Caminho no Core.SDK | DiagnosticId |
| :--- | :--- | :--- | :--- |
| `GenericVectorStoreAdapter<TVector>` | `AI/Adapter/GenericVectorStoreAdapter.cs` | `AI/Adapters/GenericVectorStoreAdapter.cs` | `HW_CORE_SDK_AI` |
| `GroqApiAdapter`, `MistralApiAdapter`, `OllamaAdapter`, `SemanticKernelAdapter` | `AI/Adapter/*` | `AI/Adapters/*` | `HW_CORE_SDK_AI` |
| `IAIInferenceAdapter`, `IAIInferenceAdapterFactory`, `IAIInferenceService`, `IAssistantService`, `IDataVector` | `Interfaces/IA/*` | `AI/Abstractions/*` | `HW_CORE_SDK_AI` |
| `IVectorStoreAdapter`, `IVectorStoreAdapterFactory`, `IVectorStoreService` | `Interfaces/SemanticKernel/*` | `AI/Abstractions/*` | `HW_CORE_SDK_AI` |
| `ChatSessionHelper`, `TokenCounterHelper`, `EmbeddingHelper` | `Helpers/AI/*`, `Helpers/EmbeddingHelper.cs` | `AI/Helpers/*` | `HW_CORE_SDK_AI` |
| DTOs de Configuração RAG (`ApplicationIAConfig`, `RagConfig`, `QdrantConfig`, etc.) | `Dto/AppConfig/Rag/*` | `AI/Configuration/*` | `HW_CORE_SDK_AI` |
| `PromptMessageVO`, `DataVectorBase` | `Dto/IA/SemanticKernel/*` | `AI/DTO/*` | `HW_CORE_SDK_AI` |
| Enums de IA (`AIChatServiceType`, `AIEmbeddingServiceType`, `InferenceAiAdapterType`, `RoleAiPromptsType`, `VectorStoreType`) | `Enuns/IA/*` | `AI/Enums/*` | `HW_CORE_SDK_AI` |
| Validadores de Prompts (`PromptMessageValidator`, `HistoryPromptsValidator`, `AskAssistantRequestValidator`) | `Validator/AI/*` | `AI/Validation/*` | `HW_CORE_SDK_AI` |

---

### 2.6 Tipos para Manter no Host (`HotelWise.Domain`)

| Categoria | Tipos Mantidos | Motivo |
| :--- | :--- | :--- |
| **Modelos de Negócio** | `Hotel`, `Room`, `Reservation`, `RoomAvailability`, `RoomPriceAndAvailabilityItem`, `User`, `ChatSessionHistory` | Entidades específicas do domínio de hotelaria e aplicação. |
| **Interfaces de Domínio** | `IHotelRepository`, `IRoomRepository`, `IReservationRepository`, `IRoomAvailabilityRepository`, `IUserRepository`, `IChatSessionHistoryRepository`, `IHotelService`, `IRoomService`, `IReservationService`, `IRoomAvailabilityService`, `IUserService`, `IChatSessionHistoryService`, `IGenerateHotelService`, `IHotelSearchService` | Contratos específicos de serviços e repositórios do produto. |
| **DTOs de Domínio** | `HotelDto`, `RoomDto`, `ReservationDto`, `RoomAvailabilityDto`, `RoomAvailabilitySearchDto`, `HotelAvailabilityRequestDto`, `UserLoginDto`, `SearchCriteria`, `GetUserAuthenticatedDto`, `AskAssistantResponse`, `ChatSessionHistoryDto`, `HotelInfo`, `HotelSemanticResult`, `HotelVector` | Estruturas de entrada/saída vinculadas a entidades de produto. |
| **Enums de Domínio** | `PaymentMethod`, `ReservationStatus`, `RoomAvailabilityStatus`, `RoomStatus`, `RoomType` | Regras e status do domínio hoteleiro. |
| **Validadores Fluent** | `HotelValidator`, `RoomValidator`, `ReservationValidator`, `RoomAvailabilityValidator`, `UserValidator`, `ChatSessionHistoryValidator` | Regras de validação específicas de cada entidade. |
| **AutoMapper Profiles** | `AutoMapperProfile.cs` | Mapeamentos entre entidades concretas de hotelaria e seus DTOs. |

---

## 3. Padrão de Shims no Host

### Exemplo 1: `EntityBase.cs`
```csharp
namespace HotelWise.Domain.Model
{
    using CoreDomain = HotelWise.Core.SDK.Domain;

    // Movido para HotelWise.Core.SDK — implementação canônica no pacote Core.
    [Obsolete("Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.Domain.EntityBase.", error: false, DiagnosticId = "HW_CORE_SDK_DOMAIN")]
    public abstract class EntityBase : CoreDomain.EntityBase
    {
    }
}
```

### Exemplo 2: `ServiceResponse.cs`
```csharp
namespace HotelWise.Domain.Dto
{
    using CoreCommon = HotelWise.Core.SDK.Common;

    // Movido para HotelWise.Core.SDK — implementação canônica no pacote Core.
    [Obsolete("Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.Common.ServiceResponse<T>.", error: false, DiagnosticId = "HW_CORE_SDK_COMMON")]
    public class ServiceResponse<T> : CoreCommon.ServiceResponse<T>
    {
    }
}
```

### Exemplo 3: `MarkdownHelper.cs`
```csharp
namespace HotelWise.Domain.Helpers
{
    // Movido para HotelWise.Core.SDK — implementação canônica no pacote Core.
    [Obsolete("Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.Helpers.MarkdownHelper.", error: false, DiagnosticId = "HW_CORE_SDK_HELPER")]
    public static class MarkdownHelper
    {
        public static string RemoveMarkdown(string markdownText) =>
            HotelWise.Core.SDK.Helpers.MarkdownHelper.RemoveMarkdown(markdownText);

        public static bool HasMarkdown(string text) =>
            HotelWise.Core.SDK.Helpers.MarkdownHelper.HasMarkdown(text);

        public static string ConvertToHtmlIfMarkdown(string markdownText) =>
            HotelWise.Core.SDK.Helpers.MarkdownHelper.ConvertToHtmlIfMarkdown(markdownText);
    }
}
```

---

## 4. Plano de Testes Canônicos (`HotelWise.Core.SDK.Tests`)

1. **`DataHelperTests.cs` & `CultureDateTimeHelperTests.cs`:**
   - Validação de conversão de tempo, aplicação de fusos horários (`E. South America Standard Time`), formatação pt-BR e UTC.
2. **`MarkdownHelperTests.cs` & `HtmlHelperTests.cs`:**
   - Teste de remoção de markdown, detecção de sintaxe markdown e conversão para HTML.
3. **`SecurityHelperTests.cs`:**
   - Teste de geração e validação de hash PBKDF2/HMACSHA512 (`CreatePasswordHash`, `VerifyPasswordHash`).
   - Teste de geração e validação de tokens JWT com claims tipadas.
4. **`ServiceCollectionHelperTests.cs`:**
   - Teste de reflexão dinâmica para registro automático de interfaces/implementações em `IServiceCollection`.
5. **`HelperValidationTests.cs`:**
   - Teste de mapeamento de falhas do FluentValidation para `ErrorResponse[]`.
6. **`TokenCounterHelperTests.cs` & `ChatSessionHelperTests.cs`:**
   - Teste de contagem aproximada de tokens e cálculo de comprimento de vetores de dados.
7. **`MiddlewareTests.cs`:**
   - Teste do `CorrelationIdMiddleware` (injeção e propagação de header), `GlobalExceptionMiddleware` e `RequestLoggingMiddleware`.

---

## 5. Checklist de Implementação

- [ ] Migrar todas as abstrações base para `HotelWise.Core.SDK/Abstractions/`.
- [ ] Migrar `EntityBase` e `EntityBaseWithNameEmail` para `HotelWise.Core.SDK/Domain/`.
- [ ] Migrar DTOs base (`ServiceResponse`, `ErrorResponse`, etc.) para `HotelWise.Core.SDK/Common/`.
- [ ] Migrar helpers gerais e de segurança para `HotelWise.Core.SDK/Helpers/` e `HotelWise.Core.SDK/Security/`.
- [ ] Migrar módulo de IA, adaptadores e helpers para `HotelWise.Core.SDK/AI/`.
- [ ] Migrar middlewares HTTP para `HotelWise.Core.SDK/Infrastructure/Middleware/`.
- [ ] Adicionar shims `[Obsolete]` em todos os arquivos correspondentes em `HotelWise.Domain`.
- [ ] Adicionar `ProjectReference` para `HotelWise.Core.SDK` em `HotelWise.Domain.csproj`.
- [ ] Implementar suíte de testes correspondente em `HotelWise.Core.SDK.Tests` com cobertura $\ge 90\%$.
- [ ] Executar `dotnet build HotelWise.Domain/HotelWise.Domain.csproj` e verificar compilação sem erros.

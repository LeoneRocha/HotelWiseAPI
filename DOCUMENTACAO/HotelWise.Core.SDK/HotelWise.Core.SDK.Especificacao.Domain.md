# Especificação Técnica — HotelWise.Core.SDK (Módulo Domain)

**Versão:** 2.0.0  
**Data:** 2026-08-28  
**Projeto de Origem:** `HotelWise.Domain` (`HotelWise.Domain.csproj`, TFM `net10.0`)  
**Projeto de Destino:** `HotelWise.Core.SDK`  
**Documento Principal:** [HotelWise.Core.SDK.Levantamento.md](./HotelWise.Core.SDK.Levantamento.md)

---

## 1. Papel do Módulo na Arquitetura

O projeto `HotelWise.Domain` é o núcleo transversal da solução — concentra modelos, abstrações, contratos, DTOs, validadores, utilitários, conectores de IA, adaptadores LLM e middlewares HTTP. Referencia o `GroqApiLibrary` (client HTTP para Groq API).

Por concentrar a maior parte das estruturas genéricas, o `HotelWise.Domain` representa a **primeira onda de migração** (Onda 1). Sem a prévia extração dos tipos canônicos do Domain para o SDK, as camadas Data, Service e API não podem ser migradas.

### Dependências NuGet relevantes do .csproj

| Pacote | Impacto no Core.SDK |
| :--- | :--- |
| `AutoMapper` | Usado em DTOs e mapeamentos |
| `FluentValidation` | Validadores — Core terá dependência condicional |
| `Microsoft.SemanticKernel.*` | Adaptadores SK — dependência pesada (TFM condicional) |
| `Serilog.*` | Logging — dependência do Core |
| `Microsoft.AspNetCore.Authentication.JwtBearer` | Segurança JWT |
| `HtmlAgilityPack`, `Markdig` | HtmlHelper e MarkdownHelper |
| `OllamaSharp`, `Mistral.SDK` | Adaptadores LLM |
| `CommunityToolkit.VectorData.*` | Vector stores (Qdrant, InMemory) |
| `Azure.Data.Tables`, `Azure.Storage.Blobs` | Cloud abstractions |
| `DocumentFormat.OpenXml`, `PDFsharp*`, `QuestPDF` | Geração de relatórios |

---

## 2. Inventário Completo — Arquivos .cs (excluindo /obj/)

> Total: **130 arquivos** fonte no projeto. Abaixo, a classificação exaustiva de cada um.

---

### 2.1 Portar + Obsoletar → Core.SDK

#### Entidades Base e Contratos (9 arquivos)

| Arquivo | Tipo | Destino Core.SDK | DiagnosticId |
| :--- | :--- | :--- | :--- |
| `Model/EntityBase.cs` | `EntityBase` | `Domain/` | `HW_CORE_SDK_DOMAIN` |
| `Model/EntityBaseWithNameEmail.cs` | `EntityBaseWithNameEmail` | `Domain/` | `HW_CORE_SDK_DOMAIN` |
| `Interfaces/Base/IEntityBase.cs` | `IEntityBase` | `Abstractions/` | `HW_CORE_SDK_DOMAIN` |
| `Interfaces/Base/IEntityBaseLog.cs` | `IEntityBaseLog` | `Abstractions/` | `HW_CORE_SDK_DOMAIN` |
| `Interfaces/Base/IEntityFieldBaseLog.cs` | `IEntityFieldBaseLog` | `Abstractions/` | `HW_CORE_SDK_DOMAIN` |
| `Interfaces/Base/IEntityDto.cs` | `IEntityDto` | `Abstractions/` | `HW_CORE_SDK_DOMAIN` |
| `Interfaces/Base/IGenericRepository.cs` ¹ | `IGenericRepository<T>` | `Abstractions/` | `HW_CORE_SDK_REPO` |
| `Interfaces/Base/IGenericService.cs` | `IGenericService<TDto>` | `Abstractions/` | `HW_CORE_SDK_SERVICE` |
| `Interfaces/Base/IServiceResponse.cs` | `IServiceResponse` | `Abstractions/` | `HW_CORE_SDK_COMMON` |

> ¹ **Bug conhecido:** O arquivo original contém `namespace HotelWise.Domain.Interfaces.Entity { namespace HotelWise.Domain.Interfaces.Entity { ... } }` (namespace aninhado duplicado). A versão canônica no Core.SDK deve corrigir para `namespace HotelWise.Core.SDK.Abstractions`.

#### Interfaces de Configuração — AppConfig (6 arquivos)

| Arquivo | Tipo | Destino Core.SDK | DiagnosticId |
| :--- | :--- | :--- | :--- |
| `Interfaces/AppConfig/ITokenConfigurationDto.cs` | `ITokenConfigurationDto` | `Abstractions/` | `HW_CORE_SDK_SECURITY` |
| `Interfaces/AppConfig/ITokenService.cs` | `ITokenService` | `Abstractions/` | `HW_CORE_SDK_SECURITY` |
| `Interfaces/AppConfig/IAiInferenceConfigBase.cs` | `IAiInferenceConfigBase` | `AI/Abstractions/` | `HW_CORE_SDK_AI` |
| `Interfaces/AppConfig/IApplicationIAConfig.cs` | `IApplicationIAConfig` | `AI/Abstractions/` | `HW_CORE_SDK_AI` |
| `Interfaces/AppConfig/IAzureAdConfig.cs` | `IAzureAdConfig` | `AI/Abstractions/` | `HW_CORE_SDK_AI` |
| `Interfaces/AppConfig/IRagConfig.cs` | `IRagConfig` | `AI/Abstractions/` | `HW_CORE_SDK_AI` |

#### DTOs Transversais e Response Pattern (10 arquivos)

| Arquivo | Tipo | Destino Core.SDK | DiagnosticId |
| :--- | :--- | :--- | :--- |
| `Dto/ServiceResponse.cs` | `ServiceResponse<T>` | `Common/` | `HW_CORE_SDK_COMMON` |
| `Dto/ErrorResponse.cs` | `ErrorResponse` | `Common/` | `HW_CORE_SDK_COMMON` |
| `Dto/Base/EntityDtoBase.cs` | `EntityDtoBase` | `Common/` | `HW_CORE_SDK_COMMON` |
| `Dto/SecurityDto.cs` | `SecurityDto` | `Common/` | `HW_CORE_SDK_COMMON` |
| `Dto/CultureDisplayDto.cs` | `CultureDisplayDto` | `Common/` | `HW_CORE_SDK_COMMON` |
| `Dto/TimeZoneDisplayDto.cs` | `TimeZoneDisplayDto` | `Common/` | `HW_CORE_SDK_COMMON` |
| `Dto/RepositoryInfo.cs` | `RepositoryInfo` | `Common/` | `HW_CORE_SDK_COMMON` |
| `Dto/AppInformationVersionProductDto.cs` | `AppInformationVersionProductDto` | `Common/` | `HW_CORE_SDK_COMMON` |
| `Dto/AppConfig/TokenConfigurationDto.cs` | `TokenConfigurationDto` | `Security/` | `HW_CORE_SDK_SECURITY` |
| `Dto/AppConfig/TokenVO.cs` | `TokenVO` | `Security/` | `HW_CORE_SDK_SECURITY` |

#### Helpers e Utilitários (15 arquivos)

| Arquivo | Tipo | Destino Core.SDK | DiagnosticId |
| :--- | :--- | :--- | :--- |
| `Helpers/DataHelper.cs` | `DataHelper` | `Helpers/` | `HW_CORE_SDK_HELPER` |
| `Helpers/CultureDateTimeHelper.cs` | `CultureDateTimeHelper` | `Helpers/` | `HW_CORE_SDK_HELPER` |
| `Helpers/TimeFormatter.cs` | `TimeFormatter` | `Helpers/` | `HW_CORE_SDK_HELPER` |
| `Helpers/MarkdownHelper.cs` | `MarkdownHelper` | `Helpers/` | `HW_CORE_SDK_HELPER` |
| `Helpers/HtmlHelper.cs` | `HtmlHelper` | `Helpers/` | `HW_CORE_SDK_HELPER` |
| `Helpers/HelperValidation.cs` | `HelperValidation` | `Validation/` | `HW_CORE_SDK_HELPER` |
| `Helpers/SecurityHelper.cs` | `SecurityHelper` | `Security/` | `HW_CORE_SDK_SECURITY` |
| `Helpers/SecurityHelperApi.cs` | `SecurityHelperApi` | `Security/` | `HW_CORE_SDK_SECURITY` |
| `Helpers/ServiceCollectionHelper.cs` | `ServiceCollectionHelper` | `Extensions/` | `HW_CORE_SDK_HELPER` |
| `Helpers/EnumExtensions.cs` | `EnumExtensions` | `Extensions/` | `HW_CORE_SDK_HELPER` |
| `Helpers/ConfigurationAppSettingsHelper.cs` | `ConfigurationAppSettingsHelper` | `Helpers/` | `HW_CORE_SDK_HELPER` |
| `Helpers/LogAppHelper.cs` | `LogAppHelper` | `Logging/` | `HW_CORE_SDK_LOGGING` |
| `Helpers/EmbeddingHelper.cs` | `EmbeddingHelper` | `AI/Helpers/` | `HW_CORE_SDK_AI` |
| `Helpers/AI/ChatSessionHelper.cs` | `ChatSessionHelper` | `AI/Helpers/` | `HW_CORE_SDK_AI` |
| `Helpers/AI/TokenCounterHelper.cs` | `TokenCounterHelper` | `AI/Helpers/` | `HW_CORE_SDK_AI` |

#### Middlewares, Exceções e Constantes (8 arquivos)

| Arquivo | Tipo | Destino Core.SDK | DiagnosticId |
| :--- | :--- | :--- | :--- |
| `CustomMiddleware/CorrelationIdMiddleware.cs` | `CorrelationIdMiddleware` | `Infrastructure/Middleware/` | `HW_CORE_SDK_MIDDLEWARE` |
| `CustomMiddleware/GlobalExceptionMiddleware.cs` | `GlobalExceptionMiddleware` | `Infrastructure/Middleware/` | `HW_CORE_SDK_MIDDLEWARE` |
| `CustomMiddleware/RequestLoggingMiddleware.cs` | `RequestLoggingMiddleware` | `Infrastructure/Middleware/` | `HW_CORE_SDK_MIDDLEWARE` |
| `AppException/AppWarningException.cs` | `AppWarningException` | `Common/Exceptions/` | `HW_CORE_SDK_COMMON` |
| `Constants/AppConfigConstants.cs` | `AppConfigConstants` | `Common/Constants/` | `HW_CORE_SDK_COMMON` |
| `Constants/ValidatorConstants.cs` | `ValidatorConstants` | `Common/Constants/` | `HW_CORE_SDK_COMMON` |
| `Constants/AzureADEntraIDConstants.cs` | `AzureADEntraIDConstants` | `Common/Constants/` | `HW_CORE_SDK_COMMON` |
| `Constants/EntityTypeConfigurationConstants.cs` | `EntityTypeConfigurationConstants` | `Common/Constants/` | `HW_CORE_SDK_COMMON` |

#### Enums genéricos (6 arquivos)

| Arquivo | Tipo | Destino Core.SDK | DiagnosticId |
| :--- | :--- | :--- | :--- |
| `Enuns/ETypeDataBase.cs` | `ETypeDataBase` | `Common/` | `HW_CORE_SDK_COMMON` |
| `Enuns/IA/AIChatServiceType.cs` | `AIChatServiceType` | `AI/Enums/` | `HW_CORE_SDK_AI` |
| `Enuns/IA/AIEmbeddingServiceType.cs` | `AIEmbeddingServiceType` | `AI/Enums/` | `HW_CORE_SDK_AI` |
| `Enuns/IA/InferenceAiAdapterType.cs` | `InferenceAiAdapterType` | `AI/Enums/` | `HW_CORE_SDK_AI` |
| `Enuns/IA/RoleAiPromptsType.cs` | `RoleAiPromptsType` | `AI/Enums/` | `HW_CORE_SDK_AI` |
| `Enuns/IA/VectorStoreType.cs` | `VectorStoreType` | `AI/Enums/` | `HW_CORE_SDK_AI` |

#### Adaptadores de IA (5 arquivos)

| Arquivo | Tipo | Destino Core.SDK | DiagnosticId |
| :--- | :--- | :--- | :--- |
| `AI/Adapter/GenericVectorStoreAdapter.cs` | `GenericVectorStoreAdapter<T>` | `AI/Adapters/` | `HW_CORE_SDK_AI` |
| `AI/Adapter/GroqApiAdapter.cs` | `GroqApiAdapter` | `AI/Adapters/` | `HW_CORE_SDK_AI` |
| `AI/Adapter/MistralApiAdapter.cs` | `MistralApiAdapter` | `AI/Adapters/` | `HW_CORE_SDK_AI` |
| `AI/Adapter/OllamaAdapter.cs` | `OllamaAdapter` | `AI/Adapters/` | `HW_CORE_SDK_AI` |
| `AI/Adapter/SemanticKernelAdapter.cs` | `SemanticKernelAdapter` | `AI/Adapters/` | `HW_CORE_SDK_AI` |

#### Interfaces de IA e Semantic Kernel (8 arquivos)

| Arquivo | Tipo | Destino Core.SDK | DiagnosticId |
| :--- | :--- | :--- | :--- |
| `Interfaces/IA/IAIInferenceAdapter.cs` | `IAIInferenceAdapter` | `AI/Abstractions/` | `HW_CORE_SDK_AI` |
| `Interfaces/IA/IAIInferenceAdapterFactory.cs` | `IAIInferenceAdapterFactory` | `AI/Abstractions/` | `HW_CORE_SDK_AI` |
| `Interfaces/IA/IAIInferenceService.cs` | `IAIInferenceService` | `AI/Abstractions/` | `HW_CORE_SDK_AI` |
| `Interfaces/IA/IAssistantService.cs` | `IAssistantService` | `AI/Abstractions/` | `HW_CORE_SDK_AI` |
| `Interfaces/IA/IDataVector.cs` | `IDataVector` | `AI/Abstractions/` | `HW_CORE_SDK_AI` |
| `Interfaces/SemanticKernel/IVectorStoreAdapter.cs` | `IVectorStoreAdapter<T>` | `AI/Abstractions/` | `HW_CORE_SDK_AI` |
| `Interfaces/SemanticKernel/IVectorStoreAdapterFactory.cs` | `IVectorStoreAdapterFactory` | `AI/Abstractions/` | `HW_CORE_SDK_AI` |
| `Interfaces/SemanticKernel/IVectorStoreService.cs` | `IVectorStoreService` | `AI/Abstractions/` | `HW_CORE_SDK_AI` |

#### DTOs de IA genéricos (3 arquivos)

| Arquivo | Tipo(s) | Destino Core.SDK | DiagnosticId |
| :--- | :--- | :--- | :--- |
| `Dto/IA/SemanticKernel/PromptMessageVO.cs` | `PromptMessageVO` | `AI/DTO/` | `HW_CORE_SDK_AI` |
| `Dto/IA/SemanticKernel/DataVectorBase.cs` | `DataVectorBase` | `AI/DTO/` | `HW_CORE_SDK_AI` |
| `Dto/IA/AskAssistantResponse.cs` | `AskAssistantResponse` + `AskAssistantRequest` ² | `AI/DTO/` | `HW_CORE_SDK_AI` |

> ² O arquivo contém **duas classes**. Ambas são genéricas (não referenciam entidades de domínio) e devem ser portadas ao Core.

#### DTOs de Configuração RAG (18 arquivos)

| Arquivo | Tipo | Destino Core.SDK |
| :--- | :--- | :--- |
| `Dto/AppConfig/Rag/AiInferenceConfigBase.cs` | `AiInferenceConfigBase` | `AI/Configuration/` |
| `Dto/AppConfig/Rag/ApplicationIAConfig.cs` | `ApplicationIAConfig` | `AI/Configuration/` |
| `Dto/AppConfig/Rag/AzureAdConfig.cs` | `AzureAdConfig` | `AI/Configuration/` |
| `Dto/AppConfig/Rag/AzureAISearchConfig.cs` | `AzureAISearchConfig` | `AI/Configuration/` |
| `Dto/AppConfig/Rag/AzureCosmosDBConfig.cs` | `AzureCosmosDBConfig` | `AI/Configuration/` |
| `Dto/AppConfig/Rag/AzureOpenAIConfig.cs` | `AzureOpenAIConfig` | `AI/Configuration/` |
| `Dto/AppConfig/Rag/AzureOpenAIEmbeddingsConfig.cs` | `AzureOpenAIEmbeddingsConfig` | `AI/Configuration/` |
| `Dto/AppConfig/Rag/GroqApiConfig.cs` | `GroqApiConfig` | `AI/Configuration/` |
| `Dto/AppConfig/Rag/MistralApiConfig.cs` | `MistralApiConfig` | `AI/Configuration/` |
| `Dto/AppConfig/Rag/MistralApiEmbeddingsConfig.cs` | `MistralApiEmbeddingsConfig` | `AI/Configuration/` |
| `Dto/AppConfig/Rag/OllamaConfig.cs` | `OllamaConfig` | `AI/Configuration/` |
| `Dto/AppConfig/Rag/OpenAIConfig.cs` | `OpenAIConfig` | `AI/Configuration/` |
| `Dto/AppConfig/Rag/OpenAIEmbeddingsConfig.cs` | `OpenAIEmbeddingsConfig` | `AI/Configuration/` |
| `Dto/AppConfig/Rag/QdrantConfig.cs` | `QdrantConfig` | `AI/Configuration/` |
| `Dto/AppConfig/Rag/RagConfig.cs` | `RagConfig` | `AI/Configuration/` |
| `Dto/AppConfig/Rag/RedisConfig.cs` | `RedisConfig` | `AI/Configuration/` |
| `Dto/AppConfig/Rag/SearchSettings.cs` | `SearchSettings` | `AI/Configuration/` |
| `Dto/AppConfig/Rag/WeaviateConfig.cs` | `WeaviateConfig` | `AI/Configuration/` |

> DiagnosticId comum: `HW_CORE_SDK_AI`

#### Constantes e Validators de IA (4 arquivos)

| Arquivo | Tipo | Destino Core.SDK | DiagnosticId |
| :--- | :--- | :--- | :--- |
| `Constants/IA/ChatCompletionValidatorsConstants.cs` | `ChatCompletionValidatorsConstants` | `AI/Constants/` | `HW_CORE_SDK_AI` |
| `Validator/AI/AskAssistantRequestValidator.cs` | `AskAssistantRequestValidator` | `AI/Validation/` | `HW_CORE_SDK_AI` |
| `Validator/AI/PromptMessageValidator.cs` | `PromptMessageValidator` | `AI/Validation/` | `HW_CORE_SDK_AI` |
| `Validator/AI/HistoryPromptsValidator.cs` | `HistoryPromptsValidator` | `AI/Validation/` | `HW_CORE_SDK_AI` |

**Subtotal Portar + Obsoletar: 92 arquivos**

---

### 2.2 Manter no Host (`HotelWise.Domain`)

#### Modelos de Negócio (7 arquivos)

| Arquivo | Tipo |
| :--- | :--- |
| `Model/HotelModels/Hotel.cs` | `Hotel` |
| `Model/HotelModels/Room.cs` | `Room` |
| `Model/HotelModels/Reservation.cs` | `Reservation` |
| `Model/HotelModels/RoomAvailability.cs` | `RoomAvailability` |
| `Model/HotelModels/RoomPriceAndAvailabilityItem.cs` | `RoomPriceAndAvailabilityItem` |
| `Model/User.cs` | `User` |
| `Model/AI/ChatSessionHistory.cs` | `ChatSessionHistory` |

#### DTOs de Domínio (10 arquivos)

| Arquivo | Tipo | Motivo |
| :--- | :--- | :--- |
| `Dto/Enitty/HotelDtos/HotelDto.cs` | `HotelDto` | Mapeamento de entidade Hotel |
| `Dto/Enitty/HotelDtos/RoomDto.cs` | `RoomDto` | Mapeamento de entidade Room |
| `Dto/Enitty/HotelDtos/ReservationDto.cs` | `ReservationDto` | Mapeamento de entidade Reservation |
| `Dto/Enitty/HotelDtos/RoomAvailabilityDto.cs` | `RoomAvailabilityDto` | Mapeamento de entidade RoomAvailability |
| `Dto/Enitty/HotelDtos/RoomAvailabilitySearchDto.cs` | `RoomAvailabilitySearchDto` | Critérios de busca de disponibilidade |
| `Dto/Enitty/HotelDtos/HotelAvailabilityRequestDto.cs` | `HotelAvailabilityRequestDto` | Request de busca de disponibilidade |
| `Dto/Enitty/UserLoginDto.cs` | `UserLoginDto` | Credenciais de login |
| `Dto/Enitty/SearchCriteria.cs` | `SearchCriteria` | Critérios de busca genérica de entidades |
| `Dto/GetUserAuthenticatedDto.cs` | `GetUserAuthenticatedDto` ³ | Resultado de autenticação com `TokenVO` |
| `Dto/IA/ChatSessionHistoryDto.cs` | `ChatSessionHistoryDto` | DTO de histórico de chat |

> ³ Contém campo `MedicalId` (resíduo legado do SmartDigitalPsico). Permanece no host.

#### DTOs IA de Domínio (3 arquivos)

| Arquivo | Tipo | Motivo |
| :--- | :--- | :--- |
| `Dto/IA/SemanticKernel/HotelInfo.cs` | `HotelInfo` | Dados de hotel para busca semântica |
| `Dto/IA/SemanticKernel/HotelSemanticResult.cs` | `HotelSemanticResult` | Resultado de busca semântica de hotéis |
| `Dto/IA/SemanticKernel/HotelVector.cs` | `HotelVector` ⁴ | Vetor de embedding de hotel |

> ⁴ `HotelVector` herda de `DataVectorBase` (que vai para o Core). Permanece como especialização de domínio.

#### Enums de Domínio (5 arquivos)

| Arquivo | Tipo |
| :--- | :--- |
| `Enuns/Hotel/PaymentMethod.cs` | `PaymentMethod` |
| `Enuns/Hotel/ReservationStatus.cs` | `ReservationStatus` |
| `Enuns/Hotel/RoomAvailabilityStatus.cs` | `RoomAvailabilityStatus` |
| `Enuns/Hotel/RoomStatus.cs` | `RoomStatus` |
| `Enuns/Hotel/RoomType.cs` | `RoomType` |

#### Interfaces de Domínio (14 arquivos)

| Arquivo | Tipo |
| :--- | :--- |
| `Interfaces/Entity/HotelInterfaces/Repository/IHotelRepository.cs` | `IHotelRepository` |
| `Interfaces/Entity/HotelInterfaces/Repository/IRoomRepository.cs` | `IRoomRepository` |
| `Interfaces/Entity/HotelInterfaces/Repository/IReservationRepository.cs` | `IReservationRepository` |
| `Interfaces/Entity/HotelInterfaces/Repository/IRoomAvailabilityRepository.cs` | `IRoomAvailabilityRepository` |
| `Interfaces/Entity/IUserRepository.cs` | `IUserRepository` |
| `Interfaces/Entity/IA/IChatSessionHistoryRepository.cs` | `IChatSessionHistoryRepository` |
| `Interfaces/Entity/HotelInterfaces/Service/IHotelService.cs` | `IHotelService` |
| `Interfaces/Entity/HotelInterfaces/Service/IRoomService.cs` | `IRoomService` |
| `Interfaces/Entity/HotelInterfaces/Service/IReservationService.cs` | `IReservationService` |
| `Interfaces/Entity/HotelInterfaces/Service/IRoomAvailabilityService.cs` | `IRoomAvailabilityService` |
| `Interfaces/Entity/HotelInterfaces/Service/IGenerateHotelService.cs` | `IGenerateHotelService` |
| `Interfaces/Entity/HotelInterfaces/Service/IHotelSearchService.cs` | `IHotelSearchService` |
| `Interfaces/Entity/IUserService.cs` | `IUserService` |
| `Interfaces/Entity/IA/IChatSessionHistoryService.cs` | `IChatSessionHistoryService` |

#### Validators de Domínio (5 arquivos)

| Arquivo | Tipo |
| :--- | :--- |
| `Validator/HotelValidators/HotelValidator.cs` | `HotelValidator` |
| `Validator/HotelValidators/RoomValidator.cs` | `RoomValidator` |
| `Validator/HotelValidators/ReservationValidator.cs` | `ReservationValidator` |
| `Validator/HotelValidators/RoomAvailabilityValidator.cs` | `RoomAvailabilityValidator` |
| `Validator/UserValidator.cs` | `UserValidator` |

#### Validator de Domínio IA (1 arquivo)

| Arquivo | Tipo | Motivo |
| :--- | :--- | :--- |
| `Validator/AI/ChatSessionHistoryValidator.cs` | `ChatSessionHistoryValidator` | Valida `ChatSessionHistory` — entidade de domínio |

#### AutoMapper (1 arquivo)

| Arquivo | Tipo | Motivo |
| :--- | :--- | :--- |
| `Mapper/AutoMapperProfile.cs` | `AutoMapperProfile` | Mapeamentos entre entidades e DTOs de produto |

**Subtotal Manter no Host: 46 arquivos**

---

## 3. Padrão de Shims no Host

### Exemplo 1: `EntityBase.cs` (classe abstrata)
```csharp
namespace HotelWise.Domain.Model
{
    using CoreDomain = HotelWise.Core.SDK.Domain;

    // ⚠️ Movido para HotelWise.Core.SDK — implementação canônica no pacote Core.
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.Domain.EntityBase.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_DOMAIN")]
    public abstract class EntityBase : CoreDomain.EntityBase
    {
    }
}
```

### Exemplo 2: `ServiceResponse<T>` (classe genérica)
```csharp
namespace HotelWise.Domain.Dto
{
    // ⚠️ Movido para HotelWise.Core.SDK
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.Common.ServiceResponse<T>.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_COMMON")]
    public class ServiceResponse<T> : HotelWise.Core.SDK.Common.ServiceResponse<T>
    {
    }
}
```

### Exemplo 3: `MarkdownHelper` (classe estática — delegação)
```csharp
namespace HotelWise.Domain.Helpers
{
    // ⚠️ Movido para HotelWise.Core.SDK
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.Helpers.MarkdownHelper.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_HELPER")]
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

### Exemplo 4: `IAiInferenceConfigBase` (interface — type-forwarding)
```csharp
namespace HotelWise.Domain.Interfaces.AppConfig
{
    // ⚠️ Movido para HotelWise.Core.SDK
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.AI.Abstractions.IAiInferenceConfigBase.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_AI")]
    public interface IAiInferenceConfigBase : HotelWise.Core.SDK.AI.Abstractions.IAiInferenceConfigBase
    {
    }
}
```

---

## 4. Plano de Testes Canônicos (`HotelWise.Core.SDK.Tests`)

| Suite | Foco | Cobertura Alvo |
| :--- | :--- | :--- |
| `EntityBaseTests.cs` | Igualdade, propriedades de `EntityBase` e `EntityBaseWithNameEmail` | ≥ 95% |
| `ServiceResponseTests.cs` | Criação, sucesso/erro, encapsulamento de `ErrorResponse[]` | ≥ 95% |
| `DataHelperTests.cs` | Conversão de tempo, timezone, formatação | ≥ 90% |
| `CultureDateTimeHelperTests.cs` | Fusos horários, cultura pt-BR, UTC | ≥ 90% |
| `MarkdownHelperTests.cs` | Remoção/detecção de markdown, conversão HTML | ≥ 90% |
| `HtmlHelperTests.cs` | Sanitização e processamento HTML | ≥ 90% |
| `SecurityHelperTests.cs` | Hash PBKDF2/HMACSHA512, geração/validação JWT | ≥ 90% |
| `ServiceCollectionHelperTests.cs` | Reflexão e registro automático em `IServiceCollection` | ≥ 90% |
| `HelperValidationTests.cs` | Mapeamento FluentValidation → ErrorResponse | ≥ 90% |
| `EnumExtensionsTests.cs` | Conversão e formatação de enums | ≥ 90% |
| `MiddlewareTests.cs` | `CorrelationIdMiddleware`, `GlobalExceptionMiddleware`, `RequestLoggingMiddleware` | ≥ 85% |
| `TokenCounterHelperTests.cs` | Contagem de tokens e dimensões de vetores | ≥ 90% |
| `ChatSessionHelperTests.cs` | Gerenciamento de sessões de chat | ≥ 90% |
| `AIConfigTests.cs` | Validação de preenchimento e serialização de configs RAG | ≥ 85% |

---

## 5. Checklist de Implementação

- [ ] Migrar abstrações base (`IEntityBase`, `IGenericRepository<T>`, etc.) → `Abstractions/`
- [ ] Migrar interfaces de AppConfig (`ITokenConfigurationDto`, `ITokenService`, `IAiInferenceConfigBase`, `IApplicationIAConfig`, `IAzureAdConfig`, `IRagConfig`) → `Abstractions/` e `AI/Abstractions/`
- [ ] Migrar `EntityBase`, `EntityBaseWithNameEmail` → `Domain/`
- [ ] Migrar DTOs base (`ServiceResponse`, `ErrorResponse`, `EntityDtoBase`, `SecurityDto`, etc.) → `Common/`
- [ ] Migrar `ETypeDataBase` → `Common/`
- [ ] Migrar `TokenConfigurationDto`, `TokenVO` → `Security/`
- [ ] Migrar helpers gerais → `Helpers/`, `Security/`, `Extensions/`, `Validation/`, `Logging/`
- [ ] Migrar middlewares → `Infrastructure/Middleware/`
- [ ] Migrar constantes → `Common/Constants/` e `AI/Constants/`
- [ ] Migrar adaptadores IA, interfaces SK, enums IA → `AI/*`
- [ ] Migrar 18 DTOs de configuração RAG → `AI/Configuration/`
- [ ] Migrar validators genéricos de IA → `AI/Validation/`
- [ ] Migrar `AskAssistantResponse` + `AskAssistantRequest` → `AI/DTO/`
- [ ] Adicionar shims `[Obsolete]` em **92 arquivos** do host
- [ ] Adicionar `ProjectReference` para `HotelWise.Core.SDK` em `HotelWise.Domain.csproj`
- [ ] Implementar suíte de testes com cobertura ≥ 90%
- [ ] `dotnet build HotelWise.Domain/HotelWise.Domain.csproj` verde

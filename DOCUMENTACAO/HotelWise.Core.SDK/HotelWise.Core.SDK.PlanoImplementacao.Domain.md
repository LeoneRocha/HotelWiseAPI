# Plano de Implementação — Onda 1: HotelWise.Domain → Core.SDK

**Versão:** 1.0.0  
**Data:** 2026-08-28  
**Plano Geral:** [HotelWise.Core.SDK.PlanoImplementacao.md](./HotelWise.Core.SDK.PlanoImplementacao.md)  
**Especificação:** [HotelWise.Core.SDK.Especificacao.Domain.md](./HotelWise.Core.SDK.Especificacao.Domain.md)  
**Pré-requisito:** Fase 0 (Scaffold) concluída — shell Core.SDK compila na solution

---

## Resumo

| Métrica | Valor |
| :--- | :--- |
| Arquivos a portar | **92** |
| Arquivos mantidos no host | **46** |
| Lotes sequenciais | **7** |
| Estimativa | **5–6 dias** (1 dev) |

---

## Lote D1 — Fundamentos (Entidades Base + Contratos)

**Dependências:** Nenhuma (fundação do Core)  
**Arquivos:** 9

### Tarefas

| # | Ação | Arquivo Origem | Destino Core.SDK |
| :--- | :--- | :--- | :--- |
| 1 | Portar interface | `Interfaces/Base/IEntityBase.cs` | `Abstractions/IEntityBase.cs` |
| 2 | Portar interface | `Interfaces/Base/IEntityBaseLog.cs` | `Abstractions/IEntityBaseLog.cs` |
| 3 | Portar interface | `Interfaces/Base/IEntityFieldBaseLog.cs` | `Abstractions/IEntityFieldBaseLog.cs` |
| 4 | Portar interface | `Interfaces/Base/IEntityDto.cs` | `Abstractions/IEntityDto.cs` |
| 5 | Portar interface ¹ | `Interfaces/Base/IGenericRepository.cs` | `Abstractions/IGenericRepository.cs` |
| 6 | Portar interface | `Interfaces/Base/IGenericService.cs` | `Abstractions/IGenericService.cs` |
| 7 | Portar interface | `Interfaces/Base/IServiceResponse.cs` | `Abstractions/IServiceResponse.cs` |
| 8 | Portar classe abstrata | `Model/EntityBase.cs` | `Domain/EntityBase.cs` |
| 9 | Portar classe abstrata | `Model/EntityBaseWithNameEmail.cs` | `Domain/EntityBaseWithNameEmail.cs` |
| 10 | Shim `[Obsolete]` nos 9 arquivos originais | — | — |
| 11 | Build Core.SDK + `HotelWise.Domain` | — | Verde |

> ¹ Corrigir namespace aninhado duplicado: `namespace HotelWise.Domain.Interfaces.Entity { namespace HotelWise.Domain.Interfaces.Entity { ... } }` → `namespace HotelWise.Core.SDK.Abstractions`

### Critério de aceite
- `dotnet build HotelWise.Core.SDK` verde
- `dotnet build HotelWise.Domain` verde (com shims)
- Tipos `EntityBase`, `IGenericRepository<T>` consumíveis pelo namespace `HotelWise.Core.SDK.*`

---

## Lote D2 — DTOs Transversais + Constantes + Enum ETypeDataBase

**Dependências:** D1 (EntityDtoBase depende de IEntityDto)  
**Arquivos:** 21

### Tarefas

| # | Ação | Arquivo Origem | Destino Core.SDK |
| :--- | :--- | :--- | :--- |
| 1 | Portar DTO | `Dto/ServiceResponse.cs` | `Common/ServiceResponse.cs` |
| 2 | Portar DTO | `Dto/ErrorResponse.cs` | `Common/ErrorResponse.cs` |
| 3 | Portar DTO | `Dto/Base/EntityDtoBase.cs` | `Common/EntityDtoBase.cs` |
| 4 | Portar DTO | `Dto/SecurityDto.cs` | `Common/SecurityDto.cs` |
| 5 | Portar DTO | `Dto/CultureDisplayDto.cs` | `Common/CultureDisplayDto.cs` |
| 6 | Portar DTO | `Dto/TimeZoneDisplayDto.cs` | `Common/TimeZoneDisplayDto.cs` |
| 7 | Portar DTO | `Dto/RepositoryInfo.cs` | `Common/RepositoryInfo.cs` |
| 8 | Portar DTO | `Dto/AppInformationVersionProductDto.cs` | `Common/AppInformationVersionProductDto.cs` |
| 9 | Portar DTO | `Dto/AppConfig/TokenConfigurationDto.cs` | `Security/TokenConfigurationDto.cs` |
| 10 | Portar DTO | `Dto/AppConfig/TokenVO.cs` | `Security/TokenVO.cs` |
| 11 | Portar interface | `Interfaces/AppConfig/ITokenConfigurationDto.cs` | `Abstractions/ITokenConfigurationDto.cs` |
| 12 | Portar interface | `Interfaces/AppConfig/ITokenService.cs` | `Abstractions/ITokenService.cs` |
| 13 | Portar enum | `Enuns/ETypeDataBase.cs` | `Common/ETypeDataBase.cs` |
| 14 | Portar constante | `Constants/AppConfigConstants.cs` | `Common/Constants/AppConfigConstants.cs` |
| 15 | Portar constante | `Constants/ValidatorConstants.cs` | `Common/Constants/ValidatorConstants.cs` |
| 16 | Portar constante | `Constants/AzureADEntraIDConstants.cs` | `Common/Constants/AzureADEntraIDConstants.cs` |
| 17 | Portar constante | `Constants/EntityTypeConfigurationConstants.cs` | `Common/Constants/EntityTypeConfigurationConstants.cs` |
| 18 | Portar exceção | `AppException/AppWarningException.cs` | `Common/Exceptions/AppWarningException.cs` |
| 19 | Shim `[Obsolete]` nos 18 arquivos originais | — | — |
| 20 | Build Core.SDK + Domain | — | Verde |

### Critério de aceite
- `ServiceResponse<T>`, `ErrorResponse`, `TokenVO` consumíveis via `HotelWise.Core.SDK.Common`
- `ITokenConfigurationDto`, `ITokenService` em `HotelWise.Core.SDK.Abstractions`
- Build verde de Core e Domain

---

## Lote D3 — Helpers e Utilitários

**Dependências:** D2 (alguns helpers referenciam ErrorResponse/AppWarningException)  
**Arquivos:** 15

### Tarefas

| # | Ação | Arquivo Origem | Destino Core.SDK |
| :--- | :--- | :--- | :--- |
| 1 | Portar helper | `Helpers/DataHelper.cs` | `Helpers/DataHelper.cs` |
| 2 | Portar helper | `Helpers/CultureDateTimeHelper.cs` | `Helpers/CultureDateTimeHelper.cs` |
| 3 | Portar helper | `Helpers/TimeFormatter.cs` | `Helpers/TimeFormatter.cs` |
| 4 | Portar helper | `Helpers/MarkdownHelper.cs` | `Helpers/MarkdownHelper.cs` |
| 5 | Portar helper | `Helpers/HtmlHelper.cs` | `Helpers/HtmlHelper.cs` |
| 6 | Portar helper | `Helpers/HelperValidation.cs` | `Validation/HelperValidation.cs` |
| 7 | Portar helper | `Helpers/SecurityHelper.cs` | `Security/SecurityHelper.cs` |
| 8 | Portar helper | `Helpers/SecurityHelperApi.cs` | `Security/SecurityHelperApi.cs` |
| 9 | Portar helper | `Helpers/ServiceCollectionHelper.cs` | `Extensions/ServiceCollectionHelper.cs` |
| 10 | Portar helper | `Helpers/EnumExtensions.cs` | `Extensions/EnumExtensions.cs` |
| 11 | Portar helper | `Helpers/ConfigurationAppSettingsHelper.cs` | `Helpers/ConfigurationAppSettingsHelper.cs` |
| 12 | Portar helper | `Helpers/LogAppHelper.cs` | `Logging/LogAppHelper.cs` |
| 13 | Shim `[Obsolete]` nos 12 arquivos (delegação para classes estáticas) | — | — |
| 14 | Build Core.SDK + Domain | — | Verde |
| 15 | Testes: `DataHelperTests`, `SecurityHelperTests`, `MarkdownHelperTests`, `HelperValidationTests` | — | Verde |

### Nota sobre shims de classes estáticas
Classes estáticas não podem herdar — cada método público deve ser redirecionado individualmente no shim:
```csharp
public static DateTime GetDateTimeNow() =>
    HotelWise.Core.SDK.Helpers.DataHelper.GetDateTimeNow();
```

### Critério de aceite
- Todos os 12 helpers compilam no Core
- Shims estáticos delegam corretamente
- Testes de helpers passam com cobertura ≥ 90%

---

## Lote D4 — Middlewares HTTP

**Dependências:** D3 (middlewares podem usar LogAppHelper, SecurityHelper)  
**Arquivos:** 3

### Tarefas

| # | Ação | Arquivo Origem | Destino Core.SDK |
| :--- | :--- | :--- | :--- |
| 1 | Portar middleware | `CustomMiddleware/CorrelationIdMiddleware.cs` | `Infrastructure/Middleware/CorrelationIdMiddleware.cs` |
| 2 | Portar middleware | `CustomMiddleware/GlobalExceptionMiddleware.cs` | `Infrastructure/Middleware/GlobalExceptionMiddleware.cs` |
| 3 | Portar middleware | `CustomMiddleware/RequestLoggingMiddleware.cs` | `Infrastructure/Middleware/RequestLoggingMiddleware.cs` |
| 4 | Shim `[Obsolete]` nos 3 arquivos originais | — | — |
| 5 | Build Core.SDK + Domain | — | Verde |
| 6 | Testes: `MiddlewareTests` (CorrelationId injection, exception handling) | — | Verde |

### Nota sobre dependências de middleware
Os middlewares dependem de `Microsoft.AspNetCore.Http` — disponível apenas em `net10.0`/`net8.0`. Usar `#if` condicional:
```csharp
#if NET8_0_OR_GREATER
namespace HotelWise.Core.SDK.Infrastructure.Middleware
{
    // ... implementação
}
#endif
```

### Critério de aceite
- Middlewares compilam no TFM `net10.0`/`net8.0`
- Shims no Domain delegam ao Core

---

## Lote D5 — IA: Interfaces, Enums e Constantes

**Dependências:** D2 (enums IA usados em configs)  
**Arquivos:** 20

### Tarefas

| # | Ação | Arquivo Origem | Destino Core.SDK |
| :--- | :--- | :--- | :--- |
| 1–4 | Portar interfaces AppConfig IA | `Interfaces/AppConfig/IAiInferenceConfigBase.cs`, `IApplicationIAConfig.cs`, `IAzureAdConfig.cs`, `IRagConfig.cs` | `AI/Abstractions/` |
| 5–8 | Portar interfaces IA | `Interfaces/IA/IAIInferenceAdapter.cs`, `IAIInferenceAdapterFactory.cs`, `IAIInferenceService.cs`, `IAssistantService.cs`, `IDataVector.cs` | `AI/Abstractions/` |
| 9–11 | Portar interfaces SK | `Interfaces/SemanticKernel/IVectorStoreAdapter.cs`, `IVectorStoreAdapterFactory.cs`, `IVectorStoreService.cs` | `AI/Abstractions/` |
| 12–16 | Portar enums IA | `Enuns/IA/AIChatServiceType.cs`, `AIEmbeddingServiceType.cs`, `InferenceAiAdapterType.cs`, `RoleAiPromptsType.cs`, `VectorStoreType.cs` | `AI/Enums/` |
| 17 | Portar constante | `Constants/IA/ChatCompletionValidatorsConstants.cs` | `AI/Constants/` |
| 18 | Shim `[Obsolete]` nos 17 arquivos | — | — |
| 19 | Build Core.SDK + Domain | — | Verde |

### Critério de aceite
- Todas as interfaces e enums de IA consumíveis via `HotelWise.Core.SDK.AI.*`
- Build verde

---

## Lote D6 — IA: Adapters + DTOs + Configs RAG

**Dependências:** D5 (adapters implementam interfaces IA)  
**Arquivos:** 26

### Tarefas

| # | Ação | Arquivo Origem | Destino Core.SDK |
| :--- | :--- | :--- | :--- |
| 1–5 | Portar adapters | `AI/Adapter/GenericVectorStoreAdapter.cs`, `GroqApiAdapter.cs`, `MistralApiAdapter.cs`, `OllamaAdapter.cs`, `SemanticKernelAdapter.cs` | `AI/Adapters/` |
| 6–7 | Portar DTOs IA | `Dto/IA/SemanticKernel/PromptMessageVO.cs`, `DataVectorBase.cs` | `AI/DTO/` |
| 8 | Portar DTOs IA ² | `Dto/IA/AskAssistantResponse.cs` (2 classes) | `AI/DTO/` |
| 9–26 | Portar 18 configs RAG | `Dto/AppConfig/Rag/*.cs` (AiInferenceConfigBase, ApplicationIAConfig, AzureAdConfig, AzureAISearchConfig, AzureCosmosDBConfig, AzureOpenAIConfig, AzureOpenAIEmbeddingsConfig, GroqApiConfig, MistralApiConfig, MistralApiEmbeddingsConfig, OllamaConfig, OpenAIConfig, OpenAIEmbeddingsConfig, QdrantConfig, RagConfig, RedisConfig, SearchSettings, WeaviateConfig) | `AI/Configuration/` |
| 27 | Shim `[Obsolete]` nos 26 arquivos | — | — |
| 28 | Build Core.SDK + Domain | — | Verde |

> ² `AskAssistantResponse.cs` contém `AskAssistantResponse` + `AskAssistantRequest` — portar ambas.

### Nota sobre `GroqApiAdapter`
Depende de `GroqApiLibrary.GroqApiClient`. O Core.SDK deve ter `ProjectReference` para `GroqApiLibrary` (configurado na Fase 0).

### Critério de aceite
- Adapters compilam com dependências SK/Groq/Ollama/Mistral (TFM `net10.0`/`net8.0`)
- 18 configs RAG no Core
- Build verde

---

## Lote D7 — IA: Validators + Helpers

**Dependências:** D6 (validators referenciam DTOs de IA)  
**Arquivos:** 7

### Tarefas

| # | Ação | Arquivo Origem | Destino Core.SDK |
| :--- | :--- | :--- | :--- |
| 1 | Portar validator | `Validator/AI/AskAssistantRequestValidator.cs` | `AI/Validation/AskAssistantRequestValidator.cs` |
| 2 | Portar validator | `Validator/AI/PromptMessageValidator.cs` | `AI/Validation/PromptMessageValidator.cs` |
| 3 | Portar validator | `Validator/AI/HistoryPromptsValidator.cs` | `AI/Validation/HistoryPromptsValidator.cs` |
| 4 | Portar helper | `Helpers/EmbeddingHelper.cs` | `AI/Helpers/EmbeddingHelper.cs` |
| 5 | Portar helper | `Helpers/AI/ChatSessionHelper.cs` | `AI/Helpers/ChatSessionHelper.cs` |
| 6 | Portar helper | `Helpers/AI/TokenCounterHelper.cs` | `AI/Helpers/TokenCounterHelper.cs` |
| 7 | Shim `[Obsolete]` nos 6 arquivos | — | — |
| 8 | Build Core.SDK + Domain | — | Verde |
| 9 | Testes: `TokenCounterHelperTests`, `ChatSessionHelperTests`, `AIConfigTests` | — | Verde |

### Critério de aceite
- Todos os 92 arquivos do Domain estão portados e com shims
- Build verde: `dotnet build HotelWise.Domain/HotelWise.Domain.csproj`
- Testes de helpers IA passam

---

## Validação Final da Onda 1

```powershell
# Build de todo o Domain + dependências
dotnet build HotelWise.Domain/HotelWise.Domain.csproj -c Release

# Verificar shims
$shimCount = (Select-String -Path "HotelWise.Domain\**\*.cs" -Pattern "HW_CORE_SDK_" -Recurse).Count
Write-Host "Shims encontrados: $shimCount (esperado: 92)"

# Testes do Core
dotnet test HotelWise.Core.SDK.Tests/HotelWise.Core.SDK.Tests.csproj

# Cobertura parcial
dotnet test HotelWise.Core.SDK.Tests/HotelWise.Core.SDK.Tests.csproj `
    --collect:"XPlat Code Coverage"
```

**Gate para Onda 2:** Build verde + 92 shims + testes passando.

# HotelWise.Core.SDK → casca sobre SmartCoreHub.Core.SDK

**Status:** ✅ casca **concluída** + hosts migrados para FQNs SCH + **PR-D6 cutover** (canônicos)  
**Atualizado:** 2026-09-06

Os tipos casca neste pacote estão marcados com `[SdkWrappedSource]`:
- citam o **pacote NuGet** `SmartCoreHub.Core.SDK` e o **FQN** do tipo destino (pós-`20260906.516.0`);
- a maioria **delega/herda** de `SmartCoreHub.Core.SDK` (thin wrapper).

## Informações do Pacote Publicado

| Campo | Valor |
| :--- | :--- |
| **PackageId canônico** | `SmartCoreHub.Core.SDK` |
| **Versão Publicada / CPM** | `20260906.516.0` |
| **Feed NuGet Oficial** | [https://www.nuget.org/packages/SmartCoreHub.Core.SDK/](https://www.nuget.org/packages/SmartCoreHub.Core.SDK/) |
| **Comando de Instalação** | `dotnet add package SmartCoreHub.Core.SDK --version 20260906.516.0` |
| **Atributo de casca** | `SmartCoreHub.Core.SDK.Common.Attributes.SdkWrappedSourceAttribute` |
| **Docs unificação** | `SmartCoreHub/Documentation/CoreFinal/implementacao-hotelwise-core-sdk.md` |

## Checklist da casca

1. [x] Publicar `SmartCoreHub.Core.SDK` no feed (`20260906.516.0`).
2. [x] `PackageReference` + CPM `PackageVersion` no HotelWiseAPI → `20260906.516.0`.
3. [x] Thin wrappers (herança / delegação estática / enum espelho) nos tipos migráveis (~92/110).
4. [x] Alias `SearchCriteria.MaxHotelRetrieve` ↔ SCH `MaxRetrieve`.
5. [x] Destinos **Ported** usados onde ainda necessário (`Security.Ported.*`, `HelperCharSet` Ported).
6. [x] Removido `ProjectReference` → `GroqApiLibrary` (Groq embutido no SCH; evita CS0433).
7. [x] Cluster AI: configs (herança + sealed composition), DTOs, helpers, validation, adapters.
8. [x] Runtime AI: `SemanticKernelProviderConfigure` delega SCH + overlay DI HW; factories delegam SCH.
9. [x] `CoreSdkInfo` delega metadados ao SCH; `IServiceResponse<T>` herda SCH.
10. [x] `HotelWise.Core.SDK.Tests` **79/79** + `HotelWiseAPI.sln` Release.
11. [x] Hosts migrados para FQNs SCH (`Domain`, `Data`, `Service`, `API`, `*.Tests`); `PackageReference` SCH; testes **264/264**.
12. [x] **`[SdkWrappedSource]`** — atributo em `SmartCoreHub.Core.SDK.Common.Attributes` (NuGet `20260906.516.0`); **~108** anotações na casca HW (targets canônicos pós-D6).

## PR-D6 cutover (2026-09-06)

| Item | Status |
| :--- | :--- |
| CPM `SmartCoreHub.Core.SDK` | **`20260906.516.0`** |
| Entidades | → `LongEntityBase` (+ aliases HW `Enable`/`CreatedDate`/`ModifyDate`) |
| Repos | → `GenericRepository<T,TContext>` (casca `GenericRepositoryBase`) |
| Serviços host | → `DtoEntityServiceBase` (façade DTO); casca `GenericEntityServiceBase` mantida p/ testes SDK |
| Abstractions | Descongeladas; `IGenericRepository` → Infra Generic; `IGenericService` ainda façade DTO SCH Abstractions |
| `[SdkWrappedSource]` | Aplicado (~108) com FQNs atuais |
| Restante Ported (até Fase C.1) | `Security.Ported.*` (Token/SecurityHelper*); `HelperCharSet` Ported; hosts ainda tipam `Abstractions.IGenericService` via DTO façade |
| **Não feito** | Fase C.1 (limpeza Ported no SCH) |

## `[SdkWrappedSource]` — aplicado

| Item | Status |
| :--- | :--- |
| Definição canônica | `SmartCoreHub.Core.SDK/Common/Attributes/SdkWrappedSourceAttribute.cs` |
| Versão publicada `20260906.516.0` | **Com** o atributo no pacote |
| Casca `HotelWise.Core.SDK` | **~108** `[SdkWrappedSource]`; script `_tools/apply-sdkwrappedsource.ps1` |
| Exemplos de target | `LongEntityBase`; `EntityFrameworkCore.Repositories.GenericRepository`; `Service.API.Middleware.*`; `Domain.DTOs.Entities.CultureDisplayDto` |

## Convertido nesta passagem (2026-08-31)

| Tipo | Estratégia |
| :--- | :--- |
| `CoreSdkInfo` | `const` delegados ao SCH |
| `IServiceResponse<T>` | Herança vazia `: SCH.IServiceResponse<T>` |
| `SemanticKernelProviderConfigure` | Delega 100% ao SCH |
| `VectorStoreAdapterFactory` | Herda SCH; retorna adapters SCH |
| `AIInferenceAdapterFactory` | Herda SCH factory |
| `GenericVectorStoreAdapter` | Casca HW sobre adapter SCH; assinaturas SCH (`SearchCriteria`, etc.) |
| `AzureADEntraIDConstants`, `AppConfigConstants`, `ValidatorConstants`, `EntityTypeConfigurationConstants` | Forward `const` ao SCH (§4.6 implementacao) |
| `ConfigureServicesAI`, `ServiceCollectionConfigureAppSettings`, `AIInferenceService` | Delegação/herança SCH |
| `IGenericService`, `ServiceResponse`, interfaces AI/Vector | Herança vazia `: SCH.*` |
| `PromptMessageVO`, `DataVectorVO` | Herança vazia `: SCH.*` |
| Hosts | `GlobalUsings.Core.cs` → namespaces SCH; DI bridges → `Service.AI.Configure` / `Service.DependenciesCollection.Extensions` |

## Retenções locais (mínimas — casca HW)

| Retenção | Motivo |
| :--- | :--- |
| `AI/Enums/*` (6) | Espelho `(int)Sch.*` — namespace HW |
| `ApplicationIAConfig`, `RagConfig` (sealed SCH) | Referência direta via `global using` → SCH; sem composição `Inner` |
| `SearchSettings` | Herança SCH (não sealed) |
| Adapters HW (`GroqApiAdapter`, etc.) | Casca fina; `IApplicationIAConfig` pass-through SCH |

**Nota:** interfaces (`IAIInference*`, `IVectorStore*`, `IGenericService`, `IServiceResponse`) herdam SCH; implementações SCH satisfazem o contrato SCH. Tipos HW explícitos na casca existem só onde sealed/composição/`new` enum impedem herança pura.

Follow-up: **Fase C.1** no SCH (remover Ported HW / pares 19–24) após estabilizar consumidores.

## Layout SCH (camadas)

- `Common/` — Attributes, DTOs/constants HW, `Security/`
- `Domain/` — `Abstractions/`, `AI/` (contratos), `Helpers/` (+ `Ported/`)
- `Infrastructure/` — `AI/Adapters`, ThirdParty, `Data/ModelBuilderExtensions`, Middleware, `EntityFrameworkCore/Repositories`
- `Service/` — `AI/` (runtime), DI, `Services/Generic`, `Validation/`, `API/Helpers`, `Security/Ported`

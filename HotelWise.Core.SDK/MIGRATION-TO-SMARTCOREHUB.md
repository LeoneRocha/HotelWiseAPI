# HotelWise.Core.SDK → casca sobre SmartCoreHub.Core.SDK

**Status:** ✅ casca **concluída** + hosts migrados para FQNs SCH  
**Atualizado:** 2026-08-31

Os tipos migrados neste pacote estão marcados com `[Obsolete]`:
- citam a **camada** SCH (`Common` / `Domain` / `Infrastructure` / `Service`);
- citam o **pacote NuGet** `SmartCoreHub.Core.SDK` e o **FQN** do tipo destino;
- a maioria **delega/herda** de `SmartCoreHub.Core.SDK` (thin wrapper).

## Informações do Pacote Publicado

| Campo | Valor |
| :--- | :--- |
| **PackageId canônico** | `SmartCoreHub.Core.SDK` |
| **Versão Publicada** | `20260831.2105.0` |
| **Feed NuGet Oficial** | [https://www.nuget.org/packages/SmartCoreHub.Core.SDK/](https://www.nuget.org/packages/SmartCoreHub.Core.SDK/) |
| **Comando de Instalação** | `dotnet add package SmartCoreHub.Core.SDK --version 20260831.2105.0` |
| **DiagnosticId depreciação** | `HW_MIGRATED` (opcional; omitido em parte por compat `netstandard2.0`) |
| **Docs unificação** | `SmartCoreHub/Documentation/CoreFinal/implementacao-hotelwise-core-sdk.md` |

## Checklist da casca

1. [x] Publicar `SmartCoreHub.Core.SDK` no feed (`20260831.2105.0`).
2. [x] `PackageReference` + CPM `PackageVersion` no HotelWiseAPI.
3. [x] Thin wrappers (herança / delegação estática / enum espelho) nos tipos migráveis (~92/110).
4. [x] Alias `SearchCriteria.MaxHotelRetrieve` ↔ SCH `MaxRetrieve`.
5. [x] Destinos **Ported** usados onde o `[Obsolete]` indica Ported.
6. [x] Removido `ProjectReference` → `GroqApiLibrary` (Groq embutido no SCH; evita CS0433).
7. [x] Cluster AI: configs (herança + sealed composition), DTOs, helpers, validation, adapters.
8. [x] Runtime AI: `SemanticKernelProviderConfigure` delega SCH + overlay DI HW; factories delegam SCH.
9. [x] `CoreSdkInfo` delega metadados ao SCH; `IServiceResponse<T>` herda SCH.
10. [x] `HotelWise.Core.SDK.Tests` **79/79** + `HotelWiseAPI.sln` Release.
11. [x] Hosts migrados para FQNs SCH (`Domain`, `Data`, `Service`, `API`, `*.Tests`); `PackageReference` SCH; testes **264/264**.
12. [ ] **`[SdkWrappedSource]`** — atributo vive em `SmartCoreHub.Core.SDK.Common.Attributes` (NuGet); **não** redefinir na casca HW. Anotações removidas da casca até publicar nova versão do pacote SCH que inclua o atributo; depois aplicar `_tools/replace-obsolete-with-wrapped.ps1`.

## `[SdkWrappedSource]` — pendente nova versão NuGet

| Item | Status |
| :--- | :--- |
| Definição canônica | `SmartCoreHub.Core.SDK/Common/Attributes/SdkWrappedSourceAttribute.cs` (repo SCH) |
| Versão publicada `20260831.2105.0` | **Sem** o atributo no pacote |
| Casca `HotelWise.Core.SDK` | **Sem** cópia local; **sem** anotações `[SdkWrappedSource]` por enquanto |
| Próximo passo | Publicar SCH → bump CPM → rodar `replace-obsolete-with-wrapped.ps1` |

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
| `AI/Enums/*` (6) | Espelho `(int)Sch.*` — namespace HW para `[Obsolete]` |
| `ApplicationIAConfig`, `RagConfig` (sealed SCH) | Referência direta via `global using` → SCH; sem composição `Inner` |
| `SearchSettings` | Herança SCH (não sealed) |
| Adapters HW (`GroqApiAdapter`, etc.) | Casca fina; `IApplicationIAConfig` pass-through SCH |

**Nota:** interfaces (`IAIInference*`, `IVectorStore*`, `IGenericService`, `IServiceResponse`) herdam SCH; implementações SCH satisfazem o contrato SCH. Tipos HW explícitos na casca existem só onde sealed/composição/`new` enum impedem herança pura.

Follow-up opcional: deprecar pacote `HotelWise.Core.SDK` quando não houver consumidores externos.

## Layout SCH (camadas)

- `Common/` — Attributes, DTOs/constants HW, `Security/`
- `Domain/` — `Abstractions/`, `AI/` (contratos), `Helpers/` (+ `Ported/`), `Extensions/`
- `Infrastructure/` — `AI/Adapters`, ThirdParty, `Data/ModelBuilderExtensions`, Middleware
- `Service/` — `AI/` (runtime), DI, `Services/Generic`, `Validation/`, `API/Helpers/Ported`

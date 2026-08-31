# HotelWise.Core.SDK → casca sobre SmartCoreHub.Core.SDK

**Status:** ✅ casca **ativada** (Etapa 3 — AI cluster convertido; retenções de contrato/DI documentadas)  
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
3. [x] Thin wrappers (herança / delegação estática / enum espelho) nos tipos migráveis.
4. [x] Alias `SearchCriteria.MaxHotelRetrieve` ↔ SCH `MaxRetrieve`.
5. [x] Destinos **Ported** usados onde o `[Obsolete]` indica Ported.
6. [x] Removido `ProjectReference` → `GroqApiLibrary` (Groq embutido no SCH; evita CS0433).
7. [x] Cluster AI: configs (herança + sealed composition), DTOs, helpers, validation, adapters.
8. [x] `HotelWise.Core.SDK.Tests` **79/79** + `HotelWiseAPI.sln` Release.
9. [ ] Hosts migrados para FQNs SCH (CS0618 esperado até lá).

## O que já está feito

- Port aditivo no `SmartCoreHub.Core.SDK` (Etapa 1) — **110/110**.
- `[Obsolete]` nos **110** tipos, com NuGet + FQN.
- **Casca ativada:** `SmartCoreHub.Core.SDK` **20260831.2105.0** via CPM.
- Wrappers thin para Abstractions (maioria), Common, Helpers, Extensions, Logging, Security, Domain EntityBase*, Infra, Services genéricos, **cluster AI**.
- Enums: **espelho** (não `TypeForwardedTo`).
- `ApplicationIAConfig` / `RagConfig` (sealed no SCH): **composição** + `Inner` / bridge para adapters SCH.
- Adapters: composição sobre SCH + `ApplicationIAConfigSchBridge`.

## Retenções locais (intencionais)

| Retenção | Motivo |
| :--- | :--- |
| `AI/Enums/*` | Espelho (regra de casca; não herda) |
| `AI/Abstractions` de runtime (`IAIInference*`, `IApplicationIAConfig`, `IRagConfig`, `IVectorStore*`, `IAssistantService`) | Assinaturas com DTOs/enums HW; herdar SCH quebraria hosts |
| `AI/Configure/SemanticKernelProviderConfigure` + `ConfigureServicesAI` | DI shim: registra tipos **HW** (`IApplicationIAConfig` / factories HW) |
| `AI/Services` factories (`AIInferenceAdapterFactory`, `VectorStoreAdapterFactory`, `AIInferenceService`) | Orquestração thin com tipos HW (não SCH factory) |
| `IGenericService` / `ServiceResponse` / `IServiceResponse` | Compat hosts (`ServiceResponse` HW); `ErrorResponse` / `GenericEntityServiceBase` já wrapped |
| `ServiceCollectionConfigureAppSettings` | DI shim HW (`AzureAdConfig` / token HW) |

Follow-up opcional: unseal SCH `ApplicationIAConfig`/`RagConfig` ou migrar hosts para FQNs SCH e apagar casca.

## Layout SCH (camadas)

- `Common/` — Attributes, DTOs/constants HW, `Security/`
- `Domain/` — `Abstractions/`, `AI/` (contratos), `Helpers/` (+ `Ported/`), `Extensions/`
- `Infrastructure/` — `AI/Adapters`, ThirdParty, `Data/ModelBuilderExtensions`, Middleware
- `Service/` — `AI/` (runtime), DI, `Services/Generic`, `Validation/`, `API/Helpers/Ported`

# Análise — SmartDigitalPsico.Domain

**Versão:** 1.0  
**Data:** 2026-08-04  
**Projeto:** `SmartDigitalPsico.Domain`  
**Onda:** 1 ([PlanoExecucao-PorProjeto.md](./PlanoExecucao-PorProjeto.md))  
**Docs pai:** [Levantamento.md](./Levantamento.md) · [PlanoDeAcao.md](./PlanoDeAcao.md) · [Progresso.md](./Progresso.md)

---

## 1. Papel na migração

Domain concentra **contratos, VOs, DTOs base, helpers, crypto, hypermedia framework e API base**. É a **primeira onda** após o scaffolding: sem esses tipos canônicos no Core, Data/Service não portam implementações com segurança.

Estratégia: portar canônico → arquivo Domain fica `[Obsolete]` + comentário (consulta/shim) → consumidores atualizam `using` para Core.

---

## 2. Portar+Obsoletar

| Tipo | Path (relativo Domain) | DiagnosticId | Testes (Domain.Test / outros) |
| ---- | ---------------------- | ------------ | ----------------------------- |
| `IEntityBaseRepository<T>` | `Interfaces/Repository/IEntityBaseRepository.cs` | `SDP_CORE_SDK_REPO` | Mocks Domain/Service |
| `IStorageTableContract<T>` | `Interfaces/TableEntity/IStorageTableContract.cs` | `SDP_CORE_SDK_REPO` | Data.Test table |
| `IStorageTableRepositoryFactory` | `Interfaces/Infrastructure/IStorageTableRepositoryFactory.cs` | `SDP_CORE_SDK_REPO` | Service.Test factories |
| `IStorageQueueContract` | `Interfaces/Infrastructure/IStorageQueueAdapter.cs` | `SDP_CORE_SDK_REPO` | Data.Test |
| `IStorageQueueRepositoryFactory` | `Interfaces/Infrastructure/IStorageQueueRepositoryFactory.cs` | `SDP_CORE_SDK_REPO` | Service.Test |
| `IFileDiskRepository` | `Interfaces/Repository/IFileDiskRepository.cs` | `SDP_CORE_SDK_REPO` | Data.Test file/disk |
| `EStorageAdapterType` | `Enuns/EStorageAdapterType.cs` | `SDP_CORE_SDK_REPO` | — |
| `BaseEntityTable` | `TableEntityNoSQL/BaseEntityTable.cs` | `SDP_CORE_SDK_REPO` | — |
| `ICacheRepository` / `IMemoryCacheRepository` / `IDiskCacheRepository` | `Interfaces/Repository/` | `SDP_CORE_SDK_CACHE` | Cache tests |
| `ICacheService` / `IDataCacheDto<T>` / `ETypeLocationCache` | Interfaces / Enuns | `SDP_CORE_SDK_CACHE` | `CacheServiceTests` |
| `CacheConfigurationDto` / `ServiceResponseCacheVO<T>` | DTO Domains / VO | `SDP_CORE_SDK_CACHE` | — |
| `IStorageBlobAdapter` / `BlobFileDto` / `LocationSaveFileConfigurationDto` | Interfaces / DTO | `SDP_CORE_SDK_AZURE` | Azure tests |
| `ICryptoAdpter` / `ICryptoAdapterFactory` / `ICryptoService` | `Interfaces/Security/` | `SDP_CORE_SDK_CRYPTO` | `CryptoAndTokenTests` |
| `AesCryptoAdpter` / `RsaCryptoAdpter` / `CryptoAdapterFactory` | `Security/` | `SDP_CORE_SDK_CRYPTO` | `CryptoAndTokenTests` |
| `TokenConfigurationDto` / `ITokenConfigurationDto` / `RsaCryptoDto` | DTO/Interfaces Security | `SDP_CORE_SDK_CRYPTO` | — |
| `ExcelGeneratorOpenXmlAdapter` / PDF adapters | `Report/` | `SDP_CORE_SDK_REPORT` | Report adapter tests |
| Helpers §6.1 Levantamento (`DateHelper`, …, `ApiBaseController`, `RequestCultureMiddleware`) | `Helpers/`, `AppException/`, `Validation/`, `API/` | `SDP_CORE_SDK_HELPER` / `SDP_CORE_SDK_API` | GeneralHelpers, File, Security, AppException, ApiBase, RequestCulture, ValidationHelper |
| `ServiceResponse<T>` / `IServiceResponse<T>` / `ErrorResponse` / `PagedSearchVO<T>` | `VO/` / Interfaces | `SDP_CORE_SDK_HELPER` | Amplamente usados |
| `EntityBase*` / `Record*` / `EntityDtoBase*` / `FileBase`/`FileData`/`FileDetailDto` / SMTP DTOs | Contracts / DTO | `SDP_CORE_SDK_HELPER` | — |
| Hypermedia framework (sem enrichers) | `Hypermedia/` (Abstract, Filters, Constants, `ContentResponseEnricher`, links) | `SDP_CORE_SDK_HYPER` | — |
| `ServiceCollectionHelper` | `Helpers/ServiceCollectionHelper.cs` (ns `Service.Helpers`) | `SDP_CORE_SDK_HELPER` | `ServiceCollectionHelperTests` |

---

## 3. Manter

| Área | Motivo |
| ---- | ------ |
| Entidades Patient/Medical/User/SystemDomains | Domínio clínico |
| DTOs Add/Get/Update de produto | Específicos |
| Validators FluentValidation de negócio | Específicos |
| Enrichers Hypermedia `GetPatient*` / Medical / Domains | Específicos |
| `TokenService`, `TokenVO`, configs produto | Auth/produto |
| `LogAppHelper`, `AuditLogHelper`, `ConfigurationAppSettingsHelper`, `SecurityHelperApi`, `ApplicationLanguageHelper`, `LanguageActionFilterAttribute` | Acoplados ao host |
| Medical/* helpers | Produto |
| Table entities `PatientRecordTableEntity`, `UserTokenSessionTableEntity` | Domínio NoSQL |
| `IEntityBaseService` (contrato de serviço de negócio) | Fica com EntityBaseService no Service |
| Schedule helpers / motor | **Backlog** — [Levantamento-ScheduleNotificationCore.md](./Levantamento-ScheduleNotificationCore.md) |

---

## 4. Dependências

| Depende de | Fornece para |
| ---------- | ------------ |
| (após F0) Core.SDK | Data, Service, WebAPI (tipos canônicos) |
| Pacotes: EF abstractions leves, Azure.Data.Tables (BaseEntityTable), JWT/crypto, Newtonsoft, etc. | — |

Domain deve referenciar Core.SDK na Onda 1. Tipos canônicos **não** devem referenciar Data/Service.

---

## 5. Lotes internos (paralelo após D1)

| Lote | Itens | Paralelo |
| ---- | ----- | -------- |
| D1 | EntityBase, ServiceResponse, ErrorResponse, IEntityBaseRepository, Records | Sequencial (base) |
| D2 | Cache contracts + DTOs/enums | Sim com D3–D7 |
| D3 | Helpers + AppException + ValidationErrorCodes | Sim |
| D4 | Crypto Domain | Sim |
| D5 | Hypermedia framework | Sim |
| D6 | Report Domain + ApiBaseController + RequestCultureMiddleware | Sim |
| D7 | Storage/blob contracts + BaseEntityTable + factory ifaces | Sim |

---

## 6. Checklist Obsolete + usings

- [ ] Cada tipo Portar+Obsoletar tem cópia canônica no Core
- [ ] Arquivo Domain com `// Movido para SmartDigitalPsico.Core.SDK` + `[Obsolete(..., DiagnosticId=...)]`
- [ ] Shim fino preferido (herda/delega)
- [ ] Usings internos Domain → Core onde aplicável
- [ ] Domain.Test usings → Core; arquivos de teste **não apagados**
- [ ] `dotnet build` + `dotnet test` Domain.Test verdes

---

## 7. Riscos

| Risco | Mitigação |
| ----- | --------- |
| `ServiceCollectionHelper` namespace `Service.Helpers` | Manter nome/comportamento; só relocação canônica |
| Hypermedia + `PagedSearchVO` | Portar framework + VO juntos (lote D5/D1) |
| `ApiBaseController` em Domain | Portar; controllers WebAPI atualizam using na Onda 4 |
| Circular Domain↔Data | Core não referencia Data; retarget DbContext só no canônico Data/Core repo |

---

## 8. Backlog Schedule

Ver [Levantamento-ScheduleNotificationCore.md](./Levantamento-ScheduleNotificationCore.md) — interfaces/models/helpers Schedule genéricos **fora** desta onda.

---

## 9. Links

- Execução: [PlanoExecucao-PorProjeto.md](./PlanoExecucao-PorProjeto.md)  
- Próxima onda: [Analise-Data.md](./Analise-Data.md)

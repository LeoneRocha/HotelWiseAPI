# Análise — SmartDigitalPsico.Service

**Versão:** 1.0  
**Data:** 2026-08-04  
**Projeto:** `SmartDigitalPsico.Service`  
**Onda:** 3 ([PlanoExecucao-PorProjeto.md](./PlanoExecucao-PorProjeto.md))  
**Docs pai:** [Levantamento.md](./Levantamento.md) · [PlanoDeAcao.md](./PlanoDeAcao.md) · [Progresso.md](./Progresso.md)

---

## 1. Papel na migração

Service concentra **adapters Azure**, **factories** Table/Queue, **CacheService**, **SMTP**, **factories de report**, e DI de infraestrutura. Também hospeda **negócio** que **permanece** (`EntityBaseService`, Medical Schedule, notifications).

Onda 3 após Domain (contratos) e Data (generics/cache repos).

---

## 2. Portar+Obsoletar

| Tipo | Path (relativo Service) | DiagnosticId | Testes (Service.Test) |
| ---- | ----------------------- | ------------ | --------------------- |
| `AzureStorageBlobAdapter` | `Infrastructure/Azure/Storage/AzureStorageBlobAdapter.cs` | `SDP_CORE_SDK_AZURE` | `AzureStorageAdaptersCoverageTests` |
| `AzureStorageTableAdapter<T>` | `Infrastructure/Azure/Storage/AzureStorageTableAdapter.cs` | `SDP_CORE_SDK_AZURE` | idem |
| `AzureStorageQueueAdapter` | `Infrastructure/Azure/Storage/AzureStorageQueueAdapter.cs` | `SDP_CORE_SDK_AZURE` | idem |
| `StorageTableRepositoryFactory` | `Infrastructure/StorageTableRepositoryFactory.cs` | `SDP_CORE_SDK_REPO` | `InfrastructureFactoryTests`, `StorageTableEntityServiceTests` |
| `StorageTableEntityService<T>` | `Infrastructure/StorageTableEntityService.cs` | `SDP_CORE_SDK_REPO` | `StorageTableEntityServiceTests` |
| `StorageQueueRepositoryFactory` | `Infrastructure/StorageQueueRepositoryFactory.cs` | `SDP_CORE_SDK_REPO` | `InfrastructureFactoryTests` |
| `StorageQueueService` | `Infrastructure/StorageQueueService.cs` | `SDP_CORE_SDK_REPO` | `InfrastructureMethodCoverageGapTests` |
| `CacheService` (arquivo inteiro, stubs inclusos) | `Infrastructure/CacheManager/CacheService.cs` | `SDP_CORE_SDK_CACHE` | `CacheServiceTests` |
| `EmailStrategyFactory` / `SmtpEmailStrategy` / `EmailContext` / `ThirdPartyEmailStrategy` | `Infrastructure/Smtp/` | `SDP_CORE_SDK_SMTP` | Smtp tests |
| `ExcelGeneratorFactory` / `PdfReportAdapterFactory` | `Infrastructure/Report/` | `SDP_CORE_SDK_REPORT` | — |

DI: `ServicesDomainNoSql`, `ServicesDomainQueue`, registros de cache — apontar tipos ao Core após port.

---

## 3. Manter

| Área | Motivo |
| ---- | ------ |
| `EntityBaseService` / `ReportBaseService` | Negócio + validators/localization |
| `DataEntity/*` serviços de entidade | Produto |
| `Bussines/Schedule/Implementations/Medical/**` | Medical host |
| `MedicalScheduleNotificationAdapter` | Produto |
| Token session adapters + `TokenSessionPersistenceFactory` | Auth produto |
| `NotificationPlatformServiceFactory` + Email/Sms/WhatsApp de domínio | Canais produto |
| `ExcelGeneratorService` / `PdfReportService` | Orquestração com conteúdo clínico |
| `CryptoService` (se orquestra só factory) | Avaliar: factory já no Core; serviço host pode Manter |

**Backlog:** `Bussines/Schedule/Core/**` — [Levantamento-ScheduleNotificationCore.md](./Levantamento-ScheduleNotificationCore.md) (não nesta onda).

---

## 4. Dependências

| Precisa (antes) | Fornece para |
| --------------- | ------------ |
| Core: contratos storage/cache, generics Data, helpers | WebAPI (DI), WindowsService/WebJob se aplicável |
| Domain shims ou Core direto | — |
| `IApplicationCacheLogRepository` (host) | `CacheService` canônico mantém dep tipada — sem redesenhar |

---

## 5. Lotes internos (paralelo)

| Lote | Itens | Paralelo |
| ---- | ----- | -------- |
| S1 | Azure Blob + Table + Queue adapters | Sim entre os 3 |
| S2 | Factories/services Table + Queue | Após S1 |
| S3 | `CacheService` | Sim com S4/S5 |
| S4 | SMTP stack | Sim |
| S5 | Report factories | Sim |

---

## 6. Checklist Obsolete + usings

- [ ] Canônicos no Core; arquivos Service Obsolete + comentário (não apagar)
- [ ] DI (`Configure/Domain/*`) registra tipos Core
- [ ] Service.Test usings → Core; testes canônicos em Core.SDK.Tests
- [ ] EntityBaseService / Medical **intocados** (exceto usings de tipos já no Core)
- [ ] Build + Service.Test verdes

---

## 7. Riscos

| Risco | Mitigação |
| ----- | --------- |
| `CacheService` ↔ ApplicationCacheLog | Manter interface no host; não criar hook novo |
| Factories AWS/Google throw | Manter comportamento; não implementar providers novos |
| Namespace anomaly `StorageQueueRepositoryFactory` | Não “corrigir” além do necessário ao portar |
| Medical Schedule Core misturado | Isolar backlog; não portar Medical nesta onda |

---

## 8. Backlog Schedule

Portar `Bussines/Schedule/Core` = iniciativa futura. Medical Implementations = sempre Manter.

---

## 9. Links

- Execução: [PlanoExecucao-PorProjeto.md](./PlanoExecucao-PorProjeto.md)  
- Anterior: [Analise-Data.md](./Analise-Data.md) · Próxima: [Analise-WebAPI.md](./Analise-WebAPI.md)

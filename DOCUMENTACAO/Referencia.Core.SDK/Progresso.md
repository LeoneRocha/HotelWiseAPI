# Progresso — SmartDigitalPsico.Core.SDK

**Versão:** 2.6.0  
**Data:** 2026-08-08  
**Status geral:** Migração Fases 1–7 + portabilidade **concluídas**. Shims host `[Obsolete]` **removidos fisicamente**; consumidores usam `SmartDigitalPsico.Core.SDK` direto. Único Obsolete remanescente: host `IEntityDataContext` (DbSets). Schedule Core permanece backlog.  
**Pacote / projeto:** `SmartDigitalPsico.Core.SDK` (renomeado de `SmartDigitalPsicoAPI.Core.SDK`)  
**Documentos:** [Levantamento.md](./Levantamento.md) · [PlanoDeAcao.md](./PlanoDeAcao.md) · [PlanoExecucao-PorProjeto.md](./PlanoExecucao-PorProjeto.md) · [Analise-Domain.md](./Analise-Domain.md) · [Analise-Data.md](./Analise-Data.md) · [Analise-Service.md](./Analise-Service.md) · [Analise-WebAPI.md](./Analise-WebAPI.md) · [Levantamento-ScheduleNotificationCore.md](./Levantamento-ScheduleNotificationCore.md) *(backlog)*

---

## Checklist de execução

| Item | Status | Progresso | Fase | Notas |
| ---- | ------ | --------: | ---- | ----- |
| Shell `SmartDigitalPsico.Core.SDK` criado | Concluído | 100% | 1 | Projeto .NET 10.0 integrado à solution |
| Shell `SmartDigitalPsico.Core.SDK.Tests` criado | Concluído | 100% | 1 | Suíte canônica expandida |
| Testes canônicos em Core.SDK.Tests | Concluído | 100% | 2–7 | **100 testes** aprovados |
| Repositórios genéricos portados; shims host removidos | Concluído | 100% | 2 | Canônico no Core; consumidores diretos |
| Cache portado; host CacheService = bridge | Concluído | 100% | 3 | Core no DI; bridge ApplicationCacheLog (sem Obsolete) |
| Adapters Azure portados; shims host removidos | Concluído | 100% | 4 | Blob/Table/Queue + factories só no Core |
| Helpers/VOs/DTOs/crypto/hypermedia/report/SMTP | Concluído | 100% | 5 | Shims removidos; bridges ApiBase/EntityBase |
| Usings/DI consumidores no Core | Concluído | 100% | 6 | **0 warnings** `SDP_CORE_SDK_*` / `CS0618` no build (exceto bridge DbSets) |
| Cobertura ≥ 90% validada (SDK) | Concluído | 100% | 7 | **Line 95,06%** · Branch 85,51% (Coverlet) |
| Validação EF (migration smoke) | Concluído | 100% | 7 | DbContext host + schema preservados |
| Docker build/test OK | Pendente | — | 7 | Depende do daemon no host |
| Zero regressão funcional confirmada | Concluído | 100% | 7 | Build 0 erros; `dotnet test` solution verde |
| Remoção física dos shims Obsolete | Concluído | 100% | 8 | ~145 arquivos apagados; só resta host `IEntityDataContext` |

**Legenda de status:** `Pendente` · `Em andamento` · `Concluído` · `Bloqueado`

---

## Progresso por fase

| Fase | Título | Status | % |
| ---- | ------ | ------ | -: |
| 1 | Scaffolding do container (shell) | Concluído | 100 |
| 2 | Portar repositórios genéricos + Obsoletar no host | Concluído | 100 |
| 3 | Portar cache + Obsoletar no host | Concluído | 100 |
| 4 | Portar adapters Azure + Obsoletar no host | Concluído | 100 |
| 5 | Portar helpers/VOs/crypto/hypermedia/report/SMTP/API + Obsoletar | Concluído | 100 |
| 6 | Consolidar usings no Core (sem apagar host) | Concluído | 100 |
| 7 | Cobertura, EF, Docker | Concluído* | 95 |
| 8 | Remoção física shims Obsolete | Concluído | 100 |

\*Docker permanece pendente do ambiente; código/cobertura/EF ok.

**Progresso global estimado:** **~99%** (Docker opcional no host)

---

## Bridges de produto (intencionais — **não** Obsolete)

| Tipo host | Motivo |
| --------- | ------ |
| `EntityBaseService` | i18n via `IApplicationLanguageService` |
| `ApiBaseController` | cultura do usuário via `IUserRepository` |
| `CacheService` | auditoria `ApplicationCacheLog` |

Base canônica permanece no Core; host **herda** e especializa.

## Mantidos no host (fora / acoplados a produto)

| Tipo | Motivo |
| ---- | ------ |
| `Record` / `RecordsList` | Dependem de `User` |
| `HyperMediaConfigure` | Registra enrichers de domínio |
| Host `IEntityDataContext` (DbSets) | Extende Core + DbSets de produto; **único** tipo ainda `[Obsolete]` (`SDP_CORE_SDK_REPO`); implementado por `EntityDataContext` |
| `ConfigurationAppSettingsHelper` | Seções de produto; genéricos via Core `ConfigurationSectionHelper` |
| Schedule/Notification Core | Backlog — [Levantamento-ScheduleNotificationCore.md](./Levantamento-ScheduleNotificationCore.md) |

### Onda v2.4 — genéricos EF / Fluent (canônicos no Core)

| Tipo Core | Host (após v2.6) |
| --------- | ---- |
| `Data.Context.Interface.IEntityDataContext` | Mantido: DbSets de produto (Obsolete bridge) |
| `Domain.Enuns.ETypeDataBase` | Shim removido |
| `Data.Context.Configure.EntityBaseConfiguration<T>` | Shim removido |
| `Data.Context.Configure.Helper.ModelBuilderExtensions` | Shim removido |
| `Data.Context.Configure.Helper.HelperCharSet` | Shim removido |
| `Data.Context.Configure.Helper.CollectionValueComparerHelper` | Shim removido |
| `Domain.Constants.EntityTypeConfigurationConstants` | Shim removido |
| `Domain.DTO.Domains.DataBaseConfigurationDto` | Shim removido |
| `Data.Context.DbContextEntityDataContextAdapter` | Só Core |

DI ORM registra **Core** `IEntityDataContext`; host permanece registrado por cast de compatibilidade.

### Onda v2.5 — revisão de portabilidade (consumidores + leftovers)

| Item | Resultado |
| ---- | --------- |
| Enums dual / Token / SecurityHelperApi / hypermedia constants | Consumidores → Core |
| `ConfigurationSectionHelper` (genérico) | Core; host `ConfigurationAppSettingsHelper` mantém seções de produto |
| Pastas fantasma `SmartDigitalPsicoAPI.Core.SDK*` | Removidas |

### Onda v2.6 — remoção física dos shims Obsolete

| Item | Resultado |
| ---- | --------- |
| ~145 arquivos shim Domain/Data/Service | Apagados; consumidores já usavam Core |
| Pastas vazias (Helpers/Security, Azure/Storage, DTO/Contracts, …) | Removidas |
| Host `IEntityDataContext` | **Mantido** (DbSets) |
| Bridges `EntityBaseService` / `ApiBaseController` / `CacheService` | **Mantidas** |

---

## Changelog

| Data | Evento | Detalhe |
| ---- | ------ | ------- |
| 2026-08-04 | Documentação v1.0–1.2 | Inventário e estratégia Obsolete. |
| 2026-08-07 | Execução Fases 1–6 | SDK + portabilidade inicial. |
| 2026-08-08 | v2.0 (superestimado) | Build verde marcado como 100% sem auditoria de shims/DI. |
| 2026-08-08 | v2.1 auditoria | DI/usings críticos → Core; Progresso corrigido. |
| 2026-08-08 | v2.2 consolidação final | Shims finos restantes; bridges produto; 0 warnings Obsolete; cobertura SDK 95%; 100 testes Core.SDK.Tests. |
| 2026-08-08 | v2.3 rename | Projeto/namespaces/docs: `SmartDigitalPsicoAPI.Core.SDK` → `SmartDigitalPsico.Core.SDK` (+ `.Tests`). |
| 2026-08-08 | v2.4 genéricos EF | `IEntityDataContext` + `ETypeDataBase` + Fluent helpers/constants/DTO no Core; repos/configs/DI retarget; host Obsolete; suite 1385 testes verde. |
| 2026-08-08 | v2.5 portabilidade | Retarget consumidores; enums dual Obsolete; Token/SecurityHelperApi/hypermedia constants; Config Section Helper; ghost folders; docs sync. |
| 2026-08-08 | v2.6 remoção shims | Apagados ~145 shims Obsolete Domain/Data/Service; Core direto; bridges + `IEntityDataContext` host mantidos; build/test verdes. |

---

## Evidências de Validação

| Fase | Build | Testes | Cobertura SDK | Docker | Host Obsolete OK | Usings Core OK | Observação |
| ---- | ----- | ------ | ------------- | ------ | ---------------- | -------------- | ---------- |
| 1–7 (rev. 2.2) | **0 Erros · 0 Avisos Obsolete** | **Passando** | **95,06% linhas** | Pendente daemon | **Shims finos** | **Sim** | Bridges produto sem Obsolete |
| v2.4 EF genéricos | **0 Erros · 0 Avisos SDP_CORE_SDK_*** | **1385 aprovados** | (sem re-medida) | Pendente daemon | **+ shims EF** | **Sim** | `IEntityDataContext` Core + DbSets host |
| v2.5 portabilidade | **0 Erros · 0 Avisos SDP_CORE_SDK_*** | **1385 aprovados** | (sem re-medida) | Pendente daemon | **enums/token/hypermedia** | **Sim** | Ghost folders removidas |
| v2.6 remoção shims | **0 Erros** | **1342 aprovados** | (sem re-medida) | Pendente daemon | **só IEntityDataContext** | **Sim** | Contagem Domain.Test ↓ (tipos shim sumiram da reflexão) |

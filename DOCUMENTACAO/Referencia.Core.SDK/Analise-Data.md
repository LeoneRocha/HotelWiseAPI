# Análise — SmartDigitalPsico.Data

**Versão:** 1.0  
**Data:** 2026-08-04  
**Projeto:** `SmartDigitalPsico.Data`  
**Onda:** 2 ([PlanoExecucao-PorProjeto.md](./PlanoExecucao-PorProjeto.md))  
**Docs pai:** [Levantamento.md](./Levantamento.md) · [PlanoDeAcao.md](./PlanoDeAcao.md) · [Progresso.md](./Progresso.md)

---

## 1. Papel na migração

Data concentra **implementações** de repositório genérico EF, cache Memory/Disk, Table/Queue wrappers e FileDisk. Depende dos contratos/helpers já canônicos no Core (Onda 1 / Domain).

**Não portar:** pasta `Context/Configure/Entity/*` (Fluent API do projeto), DbContext tipado, migrations, repos de domínio.

> **Supersedido (Progresso v2.4+):** o contrato **genérico** `IEntityDataContext` (sem DbSets de produto), `EntityBaseConfiguration`, `ModelBuilderExtensions`, `HelperCharSet` e `CollectionValueComparerHelper` foram portados ao Core; host mantém shim Obsolete + DbSets de produto. O trecho “`IEntityDataContext` Manter” abaixo aplica-se apenas ao **shim de produto** (DbSets), não ao contrato genérico.

---

## 2. Portar+Obsoletar

| Tipo | Path (relativo Data) | DiagnosticId | Testes (Data.Test) |
| ---- | -------------------- | ------------ | ------------------ |
| `GenericRepositoryEntityBase<T>` | `Repository/Generic/GenericRepositoryEntityBase.cs` | `SDP_CORE_SDK_REPO` | `ScheduleAndGenericRepositoryCoverageTests`, `RemainingDataCoverageTests` (partes) |
| `GenericTableEntityRepository<T>` | `TableEntityRepository/GenericTableEntityRepository.cs` | `SDP_CORE_SDK_REPO` | `GenericTableEntityRepositoryTests` |
| `GenericStorageQueueRepository` | `Repository/Infrastructure/GenericStorageQueueRepository.cs` | `SDP_CORE_SDK_REPO` | `RemainingDataCoverageTests` |
| `MemoryCacheRepository` | `Repository/CacheManager/MemoryCacheRepository.cs` | `SDP_CORE_SDK_CACHE` | `MemoryCacheRepositoryTests` |
| `DiskCacheRepository` | `Repository/CacheManager/DiskCacheRepository.cs` | `SDP_CORE_SDK_CACHE` | `FileAndDiskCacheRepositoryTests` |
| `FileDiskRepository` | `Repository/FileManager/FileDiskRepository.cs` | `SDP_CORE_SDK_REPO` | `FileAndDiskCacheRepositoryTests`, `FileDiskRepositoryIncompleteReadTests` |

### Ajuste canônico (Core apenas)

Construtor de `GenericRepositoryEntityBase`: aceita `IEntityDataContext` (Core) e também `DbContext` via `DbContextEntityDataContextAdapter`. Host: shim Obsolete apontando ao tipo Core. Host `IEntityDataContext` = Core + DbSets de produto (Obsolete).

---

## 3. Manter

| Área | Motivo |
| ---- | ------ |
| `Context/Configure/Entity/*` (todas `*Configuration.cs`) | EF Fluent do projeto — Levantamento §2.3 |
| `IEntityDataContext` + DbContext concreto + migrations | Específico |
| Repos Principals / SystemDomains / `ScheduleCalendarRepository` | Domínio; herdam base do Core após port |
| `FileManager` / orquestração arquivo+blob+entidades | Produto |
| `ApplicationCacheLogRepository` | Auditoria app |
| Helpers EF (`ModelBuilderExtensions`, charset, etc.) | Específicos Data |
| Seed/mocks de Entity | Projeto |

---

## 4. Dependências

| Precisa (antes) | Fornece para |
| --------------- | ------------ |
| Core: `EntityBase`, `IEntityBaseRepository`, `DateHelper`, contratos cache/file/table/queue | Service (factories usam generics); repos domínio Data |
| ProjectReference Core.SDK | — |

Repos de domínio passam a herdar `GenericRepositoryEntityBase` do Core (using atualizado).

---

## 5. Lotes internos (paralelo após A1)

| Lote | Itens | Paralelo |
| ---- | ----- | -------- |
| A1 | `GenericRepositoryEntityBase` + retarget DbContext | Sequencial primeiro |
| A2 | `MemoryCacheRepository` + `DiskCacheRepository` | Sim entre si |
| A3 | `GenericTableEntityRepository` + `GenericStorageQueueRepository` | Sim |
| A4 | `FileDiskRepository` | Sim com A2/A3 |

---

## 6. Checklist Obsolete + usings

- [ ] Tipos Portar+Obsoletar canônicos no Core
- [ ] Arquivos Data com Obsolete + comentário (não apagar)
- [ ] Repos domínio: `using` da base → Core; build Data
- [ ] Data.Test usings → Core; suíte canônica copiada para Core.SDK.Tests
- [ ] `GenderAndGenericRepositoryTests` / `FileManagerCoverageTests` **permanecem** no Data.Test (domínio)
- [ ] Nenhuma alteração em `Context/Configure/Entity/*` por esta iniciativa

---

## 7. Riscos

| Risco | Mitigação |
| ----- | --------- |
| Quebra repos domínio ao mudar base | Compilar todos `*Repository` após A1; smoke CRUD |
| DiskCache depende FileDisk + DirectoryHelper | Garantir helpers Domain já no Core (Onda 1) |
| Dual lógica se shim não for fino | Preferir herança/delegação ao Core |

---

## 8. Backlog Schedule

`ScheduleCalendarRepository` + `ScheduleCalendarConfiguration` = **Manter**. Motor Schedule Core = backlog doc dedicado.

---

## 9. Links

- Execução: [PlanoExecucao-PorProjeto.md](./PlanoExecucao-PorProjeto.md)  
- Anterior: [Analise-Domain.md](./Analise-Domain.md) · Próxima: [Analise-Service.md](./Analise-Service.md)

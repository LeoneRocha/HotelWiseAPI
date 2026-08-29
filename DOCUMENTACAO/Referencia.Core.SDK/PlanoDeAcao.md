# Plano de Ação — Core canônico + host bridges

**Versão:** 2.1  
**Data:** 2026-08-08  
**Status:** Concluído — shims Obsolete removidos (v2.6); Core é a única fonte dos tipos portados  
**Inventário base:** [Levantamento.md](./Levantamento.md)  
**Fatia futura (Schedule / Notification):** [Levantamento-ScheduleNotificationCore.md](./Levantamento-ScheduleNotificationCore.md) — backlog; **não** altera as Fases 1–7 abaixo  
**Execução por projeto:** [PlanoExecucao-PorProjeto.md](./PlanoExecucao-PorProjeto.md) (ondas Domain→Data→Service→WebAPI)  
**Análises:** [Analise-Domain.md](./Analise-Domain.md) · [Analise-Data.md](./Analise-Data.md) · [Analise-Service.md](./Analise-Service.md) · [Analise-WebAPI.md](./Analise-WebAPI.md)  
**Acompanhamento:** [Progresso.md](./Progresso.md)

---

## Regras não negociáveis

1. **Core = canônico:** tipos portados vivem só em `SmartDigitalPsico.Core.SDK` (sem inventar `Guard`/`Result`/Dapper/UoW/providers Redis novos).
2. **Host = produto + bridges:** **não** recriar shims Obsolete para tipos já no Core. Usar Core direto. Bridges intencionais: `EntityBaseService`, `ApiBaseController`, `CacheService`, host `IEntityDataContext` (DbSets).
3. **Consumidores:** `using` / DI / tipos apontam ao pacote Core.
4. **Único shell:** `SmartDigitalPsico.Core.SDK.csproj` + `SmartDigitalPsico.Core.SDK.Tests.csproj` + entrada na solution.
5. **Um único NuGet:** `PackageId=SmartDigitalPsico.Core.SDK`.
6. **Manter o específico:** DbContext tipado, entidades, migrations, validators de negócio, enrichers de domínio, `EntityBaseService` / `ReportBaseService`.
7. **Zero regressão funcional.**
8. **Testes:** suíte canônica em `Core.SDK.Tests`; testes no host usam usings do Core.
9. **Build após cada fase**; cobertura ≥ 90% no SDK.Tests (tipos canônicos).
10. **Remoção física dos shims Obsolete:** **concluída** (Progresso v2.6). Não reintroduzir wrappers host para tipos Core.

### Histórico — padrão Obsolete (host) — supersedido

Nas Fases 1–7 os originais no host foram marcados `[Obsolete]` + shim fino. Em v2.6 esses arquivos foram **apagados**. O único remanescente é:

```csharp
// Host IEntityDataContext — DbSets de produto (bridge)
[Obsolete(
    "Movido para SmartDigitalPsico.Core.SDK. Use SmartDigitalPsico.Core.SDK.Data.Context.Interface.IEntityDataContext para o contrato genérico. DbSets de produto permanecem neste shim.",
    error: false,
    DiagnosticId = "SDP_CORE_SDK_REPO")]
```

| DiagnosticId | Família (histórico) |
| ------------ | ------- |
| `SDP_CORE_SDK_REPO` | Repositórios / EF / host `IEntityDataContext` |
| `SDP_CORE_SDK_CACHE` | Cache |
| `SDP_CORE_SDK_AZURE` | Adapters Azure |
| `SDP_CORE_SDK_HELPER` | Helpers, VOs, DTOs base |
| `SDP_CORE_SDK_CRYPTO` | Crypto |
| `SDP_CORE_SDK_REPORT` | Report |
| `SDP_CORE_SDK_HYPER` | Hypermedia |
| `SDP_CORE_SDK_SMTP` | SMTP |
| `SDP_CORE_SDK_API` | API / culture middleware |

---

## Arquitetura alvo

```text
SmartDigitalPsicoAPI/
├── SmartDigitalPsico.Core.SDK/          # CANÔNICO (código portado)
├── SmartDigitalPsico.Core.SDK.Tests/    # Suíte canônica
├── SmartDigitalPsico.Domain/            # Específico de produto (+ bridges ApiBase)
├── SmartDigitalPsico.Data/              # Específico + IEntityDataContext (DbSets)
├── SmartDigitalPsico.Service/           # Específico (+ bridges EntityBaseService/CacheService)
└── SmartDigitalPsico.WebAPI/            # Consumidores → Core
```

**TFM:** `net10.0`. Host referencia Core via `ProjectReference`.

---

## Critérios de aceite globais

- [x] `dotnet build SmartDigitalPsicoAPI.sln` verde
- [x] `dotnet test` verde
- [x] Shims Obsolete host **removidos** (exceto `IEntityDataContext` DbSets)
- [x] Consumidores dos tipos portados usam namespaces do Core
- [x] Nenhum tipo inventado fora do inventário
- [x] Atualizar [Progresso.md](./Progresso.md)

### Ritual por tipo (Fases 2–5 — histórico)

1. Portar código canônico para o Core
2. Host: `[Obsolete]` + shim fino *(supersedido — hoje apagar / não recriar)*
3. Atualizar usings/DI dos consumidores para o Core
4. Testes canônicos em `Core.SDK.Tests`
5. Build + test

---

## Fase 1 — Scaffolding do container

### Escopo

- Criar shell `SmartDigitalPsico.Core.SDK.csproj` + `SmartDigitalPsico.Core.SDK.Tests.csproj`
- Incluir na solution; `ProjectReference` onde necessário
- Pastas vazias; **zero** classes de negócio

### Checklist

- [ ] Shells compilam
- [ ] Solution inclui os projetos
- [ ] Host referencia SDK sem quebrar build

### Critérios de aceite

- [ ] Build verde; nenhum tipo de negócio no SDK ainda

---

## Fase 2 — Portar repositórios genéricos + Obsoletar no host

### Escopo (portar → Core; Obsoletar no host)

`IEntityBaseRepository<T>`, `GenericRepositoryEntityBase<T>`, Table/Queue contracts/repos/factories/services, `EStorageAdapterType`, `BaseEntityTable`, `IFileDiskRepository`, `FileDiskRepository`.

**Não portar (produto):** repos Principals/SystemDomains/Schedule, DbContext tipado, migrations.  
**Atualização v2.4:** contrato genérico `IEntityDataContext` **foi** portado ao Core; host shim mantém DbSets de produto.

### Ajuste EF (só no canônico)

No Core, `GenericRepositoryEntityBase` usa `IEntityDataContext` (+ overload `DbContext` via adapter). No host, shim Obsolete aponta ao tipo do Core.

### Checklist

- [ ] Tipos canônicos no Core
- [ ] Originais no host com Obsolete + comentário (não apagados)
- [ ] Usings/DI dos consumidores → Core
- [ ] Testes canônicos em SDK.Tests; host tests com usings atualizados

### Critérios de aceite

- [ ] Build + testes verdes; cobertura dos tipos desta fase ≥ 90% no SDK.Tests
- [ ] Smoke EF / CRUD básico intacto

---

## Fase 3 — Portar cache + Obsoletar no host

### Escopo

Contratos cache, `MemoryCacheRepository`, `DiskCacheRepository`, `CacheConfigurationDto`, `ServiceResponseCacheVO<T>`, `CacheService` (**arquivo inteiro**, stubs inclusos).

**Manter (sem Obsolete desta iniciativa):** `ApplicationCacheLog*`, `IApplicationCacheLogRepository`.

### Checklist

- [ ] Canônico no Core; host Obsolete
- [ ] Usings/DI → Core
- [ ] Testes canônicos + host usings

### Critérios de aceite

- [ ] Build + testes verdes; comportamento Memory/Disk idêntico; cobertura ≥ 90%; nenhum provider Redis/Mongo novo

---

## Fase 4 — Portar adapters Azure + Obsoletar no host

### Escopo

`IStorageBlobAdapter`, `AzureStorageBlobAdapter`, `AzureStorageTableAdapter<T>`, `AzureStorageQueueAdapter`, `BlobFileDto`, `LocationSaveFileConfigurationDto`.

**Manter:** table entities de domínio, token session adapters, `FileManager`.

### Checklist / aceite

- [ ] Core canônico + host Obsolete + usings Core
- [ ] Build/testes verdes; cobertura ≥ 90%; sem adapters AWS/Google/Mongo novos

---

## Fase 5 — Portar helpers, VOs, DTOs, crypto, hypermedia, report, SMTP, API base + Obsoletar

### Escopo — Portar+Obsoletar

Helpers listados no Levantamento §6.1; VOs/DTO bases §7.1; crypto §5.1; report engines; hypermedia framework; SMTP; `ApiBaseController`, `RequestCultureMiddleware`.

### Escopo — Manter

Schedule/Medical/i18n/config host helpers; validators; enrichers; `EntityBaseService`/`ReportBaseService`; controllers WebAPI.

### Checklist / aceite

- [ ] Core + host Obsolete + usings Core
- [ ] `ValidationErrorCodes` no Core como está (mesmo prefixo)
- [ ] Build + Domain/Service/SDK tests verdes; cobertura ≥ 90%; contratos JSON inalterados

---

## Fase 6 — Consolidação de referências (sem apagar host)

### Escopo

- Confirmar usings/DI **100%** nos tipos canônicos do Core para os itens Portar+Obsoletar
- Shims `[Obsolete]` **permanecem** no host como consulta
- Warnings Obsolete: consumidores não devem mais referenciar shims (corrigir usings restantes)
- `NoWarn` global dos `SDP_CORE_SDK_*` **não** deve mascarar uso indevido nos consumidores; shims internos podem usar `#pragma` pontual
- Dockerfiles/restore incluem o `.csproj` do SDK
- **Não** remover fisicamente os arquivos Obsolete nesta fase

### Checklist / aceite

- [ ] Grep de usings antigos nos consumidores dos tipos portados = 0 (exceto shims e docs)
- [ ] Build + suite verde; `dotnet pack` OK; smoke API

---

## Fase 7 — Cobertura, EF e Docker

### Escopo

- Coverlet SDK ≥ 90%
- Smoke EF (migration) sem mudar schema de produção
- Docker build/test conforme pipeline
- Atualizar Progresso.md

### Checklist / aceite

- [ ] Cobertura ≥ 90%; Docker OK; zero regressão; changelog final

---

## Ordem de execução

```mermaid
flowchart TD
  F1[Fase1 ScaffoldingShell] --> F2[Fase2 PortarRepos Obsoletar]
  F2 --> F3[Fase3 PortarCache Obsoletar]
  F2 --> F4[Fase4 PortarAzure Obsoletar]
  F3 --> F5[Fase5 PortarHelpers Obsoletar]
  F4 --> F5
  F5 --> F6[Fase6 ConsolidarUsings]
  F6 --> F7[Fase7 CoberturaDocker]
```

---

## Fora de escopo

| Item | Motivo |
| ---- | ------ |
| Apagar arquivos `[Obsolete]` do host | Consulta mantida; remoção = iniciativa futura |
| Providers Redis/Mongo/Cosmos novos | Stubs ficam no `CacheService` portado |
| Dapper / UoW / Guard / Result | Inexistentes — não criar |
| Interface mínima nova de contexto EF | ~~Proibido~~ **Feito em v2.4** — `Core.SDK.Data.Context.Interface.IEntityDataContext` (genérico); host shim com DbSets |
| Portar `EntityBaseService` | Bridge no host (i18n); base canônica no Core |
| Pacotes NuGet satélite | Proibido |
| `Data/Context/Configure/Entity/*` | EF Fluent do projeto — **Manter** (ver Levantamento §2.3) |
| Schedule Core + NotificationTemplate stack | Fatia futura — [Levantamento-ScheduleNotificationCore.md](./Levantamento-ScheduleNotificationCore.md); fora das Fases 1–7 |

---

## Comandos de verificação

```bash
dotnet build SmartDigitalPsicoAPI.sln
dotnet test SmartDigitalPsicoAPI.sln --collect:"XPlat Code Coverage"
dotnet pack SmartDigitalPsico.Core.SDK/SmartDigitalPsico.Core.SDK.csproj -c Release
```

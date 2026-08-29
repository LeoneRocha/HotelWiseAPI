# Plano de Implementação — SmartCoreHub.Core.SDK

> **Banner:** este plano operacional (2026-06) está **concluído / supersedido**. O estado atual do pacote e da migração está em [MigracaoGenericos.md](./SmartCoreHub.Core.SDK-MigracaoGenericos.md) e [Remocao-Shims.md](./SmartCoreHub.Core.SDK-Remocao-Shims.md). README NuGet: [`backend/Core/SmartCoreHub.Core.SDK/README.md`](../../../../backend/Core/SmartCoreHub.Core.SDK/README.md).
>
> **Complemento (2026-07-15):** extrações pendentes pós-migração executadas — ver [Extracao-Pendencias.md](./SmartCoreHub.Core.SDK-Extracao-Pendencias.md).

**Documento:** Plano de execução operacional (histórico)  
**Baseado em:** [SmartCoreHub.Core.SDK-Especificacao.md](./SmartCoreHub.Core.SDK-Especificacao.md)  
**Data:** 2026-06-07  
**Restrição absoluta (da época):** não mover, alterar ou apagar código existente no backend; apenas criar novos arquivos em `backend/Core/SmartCoreHub.Core.SDK/`.

---

## 1. Objetivo operacional

Entregar o pacote NuGet `SmartCoreHub.Core.SDK` em **PRs pequenos e revisáveis**, validando build e testes a cada fase antes de avançar.

Ordem de execução:

1. Documentação (spec + plano)
2. Scaffold do projeto (`.csproj`, LICENSE, solução)
3. Módulos fundacionais (`Result`, `EntityBase`) + testes
4. Abstrações e utilitários (Tier A)
5. Infraestrutura SDK (Tier B)
6. Domain avançado e helpers (Tier C)
7. CI build + test
8. CI publish NuGet

---

## 2. Inventário e premissas técnicas

### 2.1 Localização do projeto

O Core.SDK fica **dentro de `backend/`**, na pasta **`core`**, separada dos SDKs de feature em `backend/SDKs/`:

```text
SmartCoreHub/
├── backend/
│   ├── SmartCoreHub.sln
│   ├── Directory.Build.props
│   ├── Directory.Packages.props
│   ├── Implementations/
│   │   ├── Implementations/SmartCoreHub.Domain/
│   │   └── Implementations/SmartCoreHub.Service/
│   ├── SDKs/
│   │   ├── SmartCoreHub.Localization.SDK/      # referência de convenções NuGet
│   │   ├── SmartCoreHub.Localization.SDK.Tests/
│   │   ├── SmartCoreHub.ClientSDK/
│   │   └── SmartCoreHub.ClientSDK.Tests/
│   └── Core/
│       ├── SmartCoreHub.Core.SDK/              # novo
│       └── SmartCoreHub.Core.SDK.Tests/        # novo
└── Documentation/Features/
    ├── SmartCoreHub.Core.SDK-Especificacao.md
    └── SmartCoreHub.Core.SDK-PlanoImplementacao.md
```

**Referências na solução:**

```text
Project = "SmartCoreHub.Core.SDK", "Core\SmartCoreHub.Core.SDK\SmartCoreHub.Core.SDK.csproj"
Project = "SmartCoreHub.Core.SDK.Tests", "Core\SmartCoreHub.Core.SDK.Tests\SmartCoreHub.Core.SDK.Tests.csproj"
```

Ambos aninhados na pasta virtual **Core** de `backend/SmartCoreHub.sln` (distinta da pasta virtual **SDKs**).

### 2.2 Referência de empacotamento

Espelhar integralmente:

- [backend/SDKs/SmartCoreHub.Localization.SDK/SmartCoreHub.Localization.SDK.csproj](../../../../backend/SDKs/SmartCoreHub.Localization.SDK/SmartCoreHub.Localization.SDK.csproj)
- [Documentation/Features/FEITOS/SDK/SmartCoreHub.Localization.SDK-Requisitos.md](../SDK/SmartCoreHub.Localization.SDK-Requisitos.md)

O Core.SDK reutiliza `backend/Directory.Build.props` e `backend/Directory.Packages.props` — **não** cria arquivos de build separados fora de `backend/`.

### 2.3 Stack de testes

| Pacote | Uso |
| ------ | --- |
| xUnit | Framework de testes (Core.SDK) |
| FluentAssertions | Asserts legíveis |
| Moq | Isolamento |
| coverlet.collector | Cobertura |

---

## 3. Estratégia de entrega (fases e PRs)

| Fase | PR | Escopo | Critério de aceite | Estimativa |
| ---- | -- | ------ | ----------------- | ---------- |
| **0** | — | Spec + plano (este documento) | Markdown revisado | Concluído |
| **1** | PR-1 | Scaffold: `.csproj`, LICENSE, GlobalUsings, solução | `dotnet build` OK | 0,5 dia |
| **2** | PR-2 | `Result`/`Error` + `EntityBase` + testes | `dotnet test` verde | 1 dia |
| **3** | PR-3 | `IClock`, `IAppLogger`, `Guard`, `ParallelOptionsHelper` | Build + test verde | 1 dia |
| **4** | PR-4 | HTTP/Cache/Auth (Tier B) | `dotnet pack` sem warnings | 2 dias |
| **5** | PR-5 | `ValueObject`, `AuditableEntity`, exceções base | XML docs gerada | 2 dias |
| **6** | PR-6 | `StringHelper`, `DateTimeHelper`, Extensions | ≥ 90% cobertura módulos entregues | 2 dias |
| **7** | PR-7 | Pipeline CI build + test | YAML/checklist documentado | 1 dia |
| **8** | PR-8 | Pipeline publish NuGet | Pacote no feed interno | 1 dia |

**Total estimado:** 10–11 dias úteis (1 dev).

---

## 4. Fase 1 — Scaffold (PR-1)

### 4.1 Tarefas

- [ ] Criar `backend/Core/SmartCoreHub.Core.SDK/SmartCoreHub.Core.SDK.csproj` (multi-TFM, metadados NuGet)
- [ ] Criar `backend/Core/SmartCoreHub.Core.SDK.Tests/SmartCoreHub.Core.SDK.Tests.csproj`
- [ ] Copiar `LICENSE` de `SmartCoreHub.Localization.SDK`
- [ ] Criar `GlobalUsings.cs` e `README.md`
- [ ] Adicionar projetos à `backend/SmartCoreHub.sln` (pasta virtual **Core**)
- [ ] Adicionar pacotes xUnit em `backend/Directory.Packages.props` (se ainda não existirem)

### 4.2 Comandos de validação

```powershell
cd c:\git\repos\SmartCoreHub\backend
dotnet build Core\SmartCoreHub.Core.SDK\SmartCoreHub.Core.SDK.csproj -c Release
```

### 4.3 Critério de aceite

- Build Release sem erros em todos os TFMs
- Projetos visíveis na solução, agrupados em **core**

---

## 5. Fase 2 — Fundação (PR-2)

### 5.1 Arquivos

| Arquivo | Descrição |
| ------- | --------- |
| `backend/Core/SmartCoreHub.Core.SDK/Common/Error.cs` | Record com Code, Message, Metadata |
| `backend/Core/SmartCoreHub.Core.SDK/Common/Result.cs` | Result + Result&lt;T&gt; |
| `backend/Core/SmartCoreHub.Core.SDK/Domain/EntityBase.cs` | Adaptado de BaseEntity (Guid Id) |
| `backend/Core/SmartCoreHub.Core.SDK.Tests/Common/ResultTests.cs` | Sucesso, falha, múltiplos erros, acesso a Value |
| `backend/Core/SmartCoreHub.Core.SDK.Tests/Domain/EntityBaseTests.cs` | Igualdade, Activate/Deactivate |

### 5.2 Comandos

```powershell
cd c:\git\repos\SmartCoreHub\backend
dotnet test Core\SmartCoreHub.Core.SDK.Tests\SmartCoreHub.Core.SDK.Tests.csproj -c Release
```

### 5.3 Critério de aceite

- Todos os testes passam
- Cobertura dos dois módulos implementados

---

## 6. Fase 3 — Abstrações Tier A (PR-3)

Copiar/adaptar (originais intactos em `SmartCoreHub.Domain`):

| Origem | Destino |
| ------ | ------- |
| `Implementations/SmartCoreHub.Domain/Interfaces/Common/IClock.cs` | `Core/SmartCoreHub.Core.SDK/Abstractions/IClock.cs` |
| `Implementations/SmartCoreHub.Domain/Interfaces/Common/IAppLogger.cs` | `Core/SmartCoreHub.Core.SDK/Logging/IAppLogger.cs` |
| Rascunho Guard | `Core/SmartCoreHub.Core.SDK/Validation/Guard.cs` |
| `Implementations/SmartCoreHub.Domain/Helpers/ParallelOptionsHelper.cs` | `Core/SmartCoreHub.Core.SDK/Helpers/ParallelOptionsHelper.cs` |

Testes unitários para cada módulo.

---

## 7. Fase 4 — Infraestrutura SDK Tier B (PR-4)

Copiar/adaptar de Localization.SDK e ClientSDK:

- `Core/SmartCoreHub.Core.SDK/Http/HttpHeaderNamesHelper.cs`
- `Core/SmartCoreHub.Core.SDK/Abstractions/IAuthHeaderProvider.cs`
- `Core/SmartCoreHub.Core.SDK/Authentication/ApiKeyAuthHeaderProvider.cs`
- `Core/SmartCoreHub.Core.SDK/Abstractions/IApiErrorMapper.cs`
- `Core/SmartCoreHub.Core.SDK/Caching/MemoryCacheProvider.cs`
- `Core/SmartCoreHub.Core.SDK/Http/HttpRequestExecutorBase.cs` (extrair de BaseHttpService)
- `Core/SmartCoreHub.Core.SDK/Polyfills/IsExternalInit.cs`

Validação:

```powershell
cd c:\git\repos\SmartCoreHub\backend
dotnet pack Core\SmartCoreHub.Core.SDK\SmartCoreHub.Core.SDK.csproj -c Release --no-build
```

---

## 8. Fase 5 — Domain avançado (PR-5)

- `Core/SmartCoreHub.Core.SDK/Domain/ValueObject.cs`
- `Core/SmartCoreHub.Core.SDK/Domain/AuditableEntity.cs` (string CreatedBy/UpdatedBy, sem entidade User)
- `Core/SmartCoreHub.Core.SDK/Exceptions/SmartCoreHubSdkException.cs` + hierarquia base

---

## 9. Fase 6 — Helpers e Extensions (PR-6)

Implementar conforme spec:

- `StringHelper` / `StringExtensions` (Normalize, Slugify, MaskDocument, etc.)
- `DateTimeHelper` / `DateTimeExtensions` (UTC, StartOfDay, CalculateAge, etc.)

Meta: ≥ 90% cobertura com `[Theory]` / `[InlineData]`.

---

## 10. Fase 7 — CI build + test (PR-7)

Reutilizar padrão de Localization.SDK:

- Restore → Build (multi-TFM) → Test → (sem pack/publish inicialmente)
- Documentar steps em checklist Azure DevOps / GitHub Actions
- Referência: [Documentation/Features/FEITOS/AZURE-PIPELINE-LOCALIZATION-CHECKLIST.md](../AZURE-PIPELINE-LOCALIZATION-CHECKLIST.md)

### Checklist CI inicial

- [ ] Job `build-core-sdk` na pipeline existente ou pipeline dedicada
- [ ] `dotnet restore` na solução `backend/SmartCoreHub.sln`
- [ ] `dotnet build Core/SmartCoreHub.Core.SDK/SmartCoreHub.Core.SDK.csproj -c Release` multi-TFM
- [ ] `dotnet test Core/SmartCoreHub.Core.SDK.Tests/SmartCoreHub.Core.SDK.Tests.csproj -c Release --no-build`
- [ ] Artefato de cobertura (opcional)

---

## 11. Fase 8 — CI publish NuGet (PR-8)

- `dotnet pack` após build completo
- Push para feed interno (mesmos secrets do Localization.SDK)
- Referência: [Documentation/Features/FEITOS/NUGET-PUBLISH-POWERSHELL-ONLY.md](../NUGET-PUBLISH-POWERSHELL-ONLY.md)

---

## 12. Migração gradual (pós-estabilização)

Ordem sugerida de adoção pelos consumidores:

1. `SmartCoreHub.Localization.SDK` — referenciar Core.SDK; remover duplicatas (HttpHeaderNamesHelper, ICacheProvider, etc.)
2. `SmartCoreHub.CloudClientSDK` — referenciar Core.SDK; extrair BaseHttpService
3. `SmartCoreHub.Domain` — avaliar substituição de BaseEntity/IClock por tipos Core (breaking change interno)
4. Demais camadas — fase a fase conforme aprovação

**Regra:** cada migração em PR separado; nunca alterar originais e consumidor no mesmo PR.

---

## 13. Riscos e mitigações

| Risco | Mitigação |
| ----- | --------- |
| Divergência `Guid` vs `long` Id | Documentar; `LongEntityBase` futuro |
| Duplicação temporária | Esperado até migração; catalogar em spec |
| Multi-TFM netstandard | Polyfills (`IsExternalInit`); testes em net10.0 |
| Breaking changes acidentais | SemVer rigoroso; changelog no pacote |

---

## 14. Comandos úteis (referência rápida)

```powershell
cd c:\git\repos\SmartCoreHub\backend

# Build
dotnet build Core\SmartCoreHub.Core.SDK\SmartCoreHub.Core.SDK.csproj -c Release

# Test
dotnet test Core\SmartCoreHub.Core.SDK.Tests\SmartCoreHub.Core.SDK.Tests.csproj -c Release

# Pack
dotnet build Core\SmartCoreHub.Core.SDK\SmartCoreHub.Core.SDK.csproj -c Release
dotnet pack Core\SmartCoreHub.Core.SDK\SmartCoreHub.Core.SDK.csproj -c Release --no-build
```

---

## 15. Status atual

| Item | Status |
| ---- | ------ |
| Documentação spec + plano | Concluído |
| Localização definida (`backend/Core/`) | Concluído |
| Scaffold projeto | Pendente |
| Fase 2 (Result + EntityBase) | Pendente |
| Fases 3–8 | Pendente |

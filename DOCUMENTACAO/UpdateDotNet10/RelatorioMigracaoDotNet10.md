# Relatório de Migração — HotelWiseAPI para .NET 10

**Status:** CONCLUÍDO  
**Data da execução:** 2026-07-31  
**Branch:** `chore/update-packages-hotelwiseapi-dotnet10`  
**Solução:** `HotelWiseAPI/HotelWiseAPI.sln`  
**SDK usado:** `.NET SDK 10.0.301` (`global.json` com `rollForward: latestFeature`)  
**Documentos de origem:**

- `DOCUMENTACAO/UpdateDotNet10/RascunhoPlanoUpdateDotNet10.md`
- `DOCUMENTACAO/UpdateDotNet10/PlanoAcaoMigracaoDotNet10.md`
- `DOCUMENTACAO/API/2026-07-LevantamentoConjuntoHomologado-HotelWiseAPI.md`
- `DOCUMENTACAO/API/PlanoImplementacaoMigracaoDotNet10-HotelWiseAPI.md`

---

## 1. Objetivo

Registrar a execução da migração HotelWiseAPI de .NET 8 para .NET 10:

- TFM `net10.0` nos projetos internos/API/Console
- Multi-target `net8.0;net10.0` no `GroqApiLibrary`
- CPM via `Directory.Packages.props` (Conjunto Homologado v1)
- Validação de build, pack e testes

---

## 2. Escopo executado

| Projeto | TFM final |
| ------- | --------- |
| HotelWise.API | net10.0 |
| HotelWise.Service | net10.0 |
| HotelWise.Data | net10.0 |
| HotelWise.Domain | net10.0 |
| GroqApiLibrary | net8.0;net10.0 |
| HotelWise.ConsolePOC | net10.0 |

Fora do ciclo: `GroqToolLibrary` (órfão), `HotelWiseUI`.

---

## 3. Gerenciamento de Pacotes NuGet

Arquivo criado: `HotelWiseAPI/Directory.Packages.props`

| Bloco | Versões aplicadas |
| ----- | ----------------- |
| A — Plataforma | AspNetCore / Extensions / System.Text.Json **10.0.10** |
| B — Persistência | EF **9.0.18** + Pomelo **9.0.0** + SqlServer **9.0.18** |
| C — OpenAPI / logs / identity | Swashbuckle **10.2.3**, Serilog.AspNetCore **10.0.0**, Identity.Web **3.15.1** |
| D — Azure / utils | AutoMapper **16.2.0**, Azure.* do levantamento, Graph **5.105.0** |
| AI | SK **1.78.0** (+ alphas/previews alinhados) |

**Pins transitivos adicionados (mitigação NU1903):**

- `System.Security.Cryptography.Xml` **10.0.10**
- família `Microsoft.Kiota.*` **1.22.2**
- `CentralPackageTransitivePinningEnabled=true`

**Desvio documentado (Groq multi-target):** `Microsoft.AspNetCore.Components` usa `VersionOverride="8.0.22"` no TFM `net8.0` (pacote 10.x não é compatível com net8).

---

## 4. Ajustes técnicos realizados

| Área | Ajuste |
| ---- | ------ |
| Swashbuckle 10 / OpenAPI 2.x | `using Microsoft.OpenApi` (namespace `Models` removido) |
| VectorData May 2025 | Atributos `VectorStoreKey/Data/Vector`; `VectorStore` / `VectorStoreCollection`; `SearchAsync`; `EnsureCollectionExistsAsync` |
| Qdrant DI | `builder.Services.AddQdrantCollection` / `AddQdrantVectorStore` (IServiceCollection) |
| Agents SK 1.78 | `AgentResponseItem.Message.Content` |
| OllamaSharp 5.4 | `EmbedAsync` no lugar de `GenerateEmbeddingAsync` |
| EF + Pomelo | Mantido Bloco B v1 (sem EF 10 — Pomelo 10 oficial ainda inexistente) |
| Seeds determinísticos | `HotelsMockData` / `UserMockData` alinhados ao snapshot (sem `UtcNow` / hash aleatório) |
| Seed Room | `RoomsMockData` + `HasData` + migration `SeedRoomPosDotNet10` (Room Id=100; recria Hotel Id=1 se ausente) |

---

## 5. Validações

### SDK

```text
8.0.416 / 9.0.314 / 10.0.300 / 10.0.301
```

### Build Release

| Item | Resultado |
| ---- | --------- |
| Erros | **0** |
| NU1903 AutoMapper 15 / NU1904 SK 1.41 | **Ausentes** |
| NU1903 Cryptography.Xml / Kiota (pós-pin) | **Ausentes** |

### Testes

| Item | Resultado |
| ---- | --------- |
| Projetos de teste | **0** |
| Status | **N/A — gap** (`dotnet test` sem projetos de teste) |

### Pack GroqApiLibrary

| Item | Resultado |
| ---- | --------- |
| `lib/net8.0/` | **Sim** |
| `lib/net10.0/` | **Sim** |

### Migrations / Smoke API

| Item | Resultado |
| ---- | --------- |
| Migration temporária `ValidacaoPosUpdateDotNet10` | **Up/Down vazios** → removida com `migrations remove --force` |
| `database update` (pré-seed) | **OK** |
| Migration `SeedRoomPosDotNet10` | **Aplicada** (`20260731234148_SeedRoomPosDotNet10`) |
| Registros no MySQL Development | User `admin` (Id=1); Hotel `Hotel Example` (HotelId=1); Room `Quarto Example` (Id=100) |
| Smoke API / auth / AI | Pendente de runtime local |

---

## 6. Resultados pós-migração

```text
Data da execução: 2026-07-31
Branch: chore/update-packages-hotelwiseapi-dotnet10
SDK usado: 10.0.301
Build Release: OK (0 erros)
Pack GroqApiLibrary: OK (net8.0 + net10.0)
Migrations: OK — ValidacaoPosUpdateDotNet10 vazia/removida; SeedRoomPosDotNet10 aplicada
Seeds: User admin + Hotel Example + Room Id=100
Smoke API: pendente (ambiente)
Desvios do Conjunto v1:
  - Groq: VersionOverride Components 8.0.22 em net8.0
  - Pins transitivos Cryptography.Xml 10.0.10 e Kiota 1.22.2
  - SeedRoom Up inclui INSERT Hotel Id=1 se ausente (DB de prod/dev já sem o seed original)
```

### Quantitativo

```text
Projetos .NET atualizados: 6 / 6
Pacotes NuGet alinhados ao Conjunto v1: sim (com pins transitivos)
Testes automatizados: 0/0 (gap)
Vulnerabilidades NU1903/NU1904 (AutoMapper 15 / SK 1.41): resolvidas
Falhas de build encontradas/corrigidas: VectorData API, Qdrant DI, OpenAPI namespace, Agents, Ollama Embed
Migrations validadas: sim
```

---

## 7. Infraestrutura e CI

| Item | Status |
| ---- | ------ |
| `global.json` | SDK 10.0.301 + rollForward |
| README (.NET SDK) | Atualizado para SDK / .NET 10 |
| Dockerfile API | N/A |
| QdrantDockerFile | Sem mudança |
| Azure DevOps UseDotNet | **Pendente (fora do tree)** — alinhar task para `10.x` |

---

## 8. Riscos residuais

| Risco | Status |
| ----- | ------ |
| Pomelo 9 → EF 9 (sem EF 10) | Aceito no v1 |
| SK InMemory/Qdrant 1.74-preview vs core 1.78 | Monitorar no smoke |
| Sem suíte de testes | Aceito; smoke manual |
| Identity.Web 3.x / Graph 5.x | Conjunto v2 |
| GroqToolLibrary órfão | Fora do ciclo |

---

## 9. Conclusão

Migração de TFM/pacotes **concluída com build Release OK**, pack multi-target do `GroqApiLibrary`, migration temporária vazia (removida) e seed de Room aplicado no MySQL Development. Restam smoke runtime (API/auth/AI) e alinhamento do pipeline Azure DevOps externo para SDK 10.

---

## 10. Referências

- `DOCUMENTACAO/GuiaGenericoAtualizacaoPacotes.md`
- `DOCUMENTACAO/UpdateDotNet10/PlanoAcaoMigracaoDotNet10.md`
- `DOCUMENTACAO/UpdateDotNet10/RascunhoPlanoUpdateDotNet10.md`
- `DOCUMENTACAO/API/2026-07-LevantamentoConjuntoHomologado-HotelWiseAPI.md`
- `DOCUMENTACAO/API/PlanoImplementacaoMigracaoDotNet10-HotelWiseAPI.md`
- Vector Store May 2025: https://learn.microsoft.com/en-us/semantic-kernel/support/migration/vectorstore-may-2025
- Swashbuckle v10: https://github.com/domaindrivendev/Swashbuckle.AspNetCore/blob/master/docs/migrating-to-v10.md

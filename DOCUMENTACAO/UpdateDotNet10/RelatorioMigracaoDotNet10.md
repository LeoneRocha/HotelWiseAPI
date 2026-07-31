# Relatório de Migração — HotelWiseAPI para .NET 10

**Status:** PENDENTE DE EXECUÇÃO (baseline pré-migração)  
**Data do baseline / inventário:** 2026-07-31  
**Solução:** `HotelWiseAPI/HotelWiseAPI.sln`  
**SDK disponível no ambiente de inventário:** `.NET SDK 10.0.301` (também 8.0.416, 9.0.314, 10.0.300)  
**Documentos de origem:**

- `DOCUMENTACAO/UpdateDotNet10/RascunhoPlanoUpdateDotNet10.md`
- `DOCUMENTACAO/UpdateDotNet10/PlanoAcaoMigracaoDotNet10.md`
- `DOCUMENTACAO/API/2026-07-LevantamentoConjuntoHomologado-HotelWiseAPI.md`
- `DOCUMENTACAO/API/PlanoImplementacaoMigracaoDotNet10-HotelWiseAPI.md`

> Após concluir a migração, atualizar este arquivo: mudar o status para **CONCLUÍDO**, preencher seções 5–9 com comandos/resultados reais e listar desvios do Conjunto Homologado v1.

---

## 1. Objetivo

Registrar o **estado atual** (pré-migração) e o template de evidências da migração HotelWiseAPI de .NET 8 para .NET 10, incluindo:

- TFM `net10.0` nos projetos internos/API/Console
- Multi-target `net8.0;net10.0` no `GroqApiLibrary`
- CPM via `Directory.Packages.props`
- Conjunto Homologado v1 (AspNet **10.0.10**, EF/Pomelo **9.0.18/9.0.0**, SK **1.78**, etc.)
- Validação de build, pack, migrations e smoke da API

---

## 2. Escopo previsto

### Projetos (alvo)

| Projeto | TFM atual (baseline) | TFM alvo |
| ------- | -------------------- | -------- |
| HotelWise.API | net8.0 | net10.0 |
| HotelWise.Service | net8.0 | net10.0 |
| HotelWise.Data | net8.0 | net10.0 |
| HotelWise.Domain | net8.0 | net10.0 |
| GroqApiLibrary | net8.0 | net8.0;net10.0 |
| HotelWise.ConsolePOC | net8.0 | net10.0 |

Fora do ciclo: `GroqToolLibrary` (órfão, fora do .sln), `HotelWiseUI`.

### Ainda não migrado (baseline 2026-07-31)

Confirmado que **todos** os projetos do `.sln` ainda declaram:

```xml
<TargetFramework>net8.0</TargetFramework>
```

Não existe `Directory.Packages.props` na raiz de `HotelWiseAPI/`.

---

## 3. Gerenciamento de Pacotes NuGet (planejado)

### Estado atual

- Versões **inline** em cada `.csproj`
- Drift conhecido: `Microsoft.EntityFrameworkCore` **8.0.19** vs Design/Relational/SqlServer/Tools **8.0.16**
- Extensions / `System.Text.Json` em **9.0.8** com TFM 8
- Avisos: `NU1903` AutoMapper 15.0.1; `NU1904` SemanticKernel.Core 1.41.0

### Estado alvo

Arquivo a criar:

```text
HotelWiseAPI/Directory.Packages.props
```

Conjunto Homologado v1 (resumo):

| Bloco | Versões |
| ----- | ------- |
| A — Plataforma | AspNetCore / Extensions / System.Text.Json **10.0.10** |
| B — Persistência | EF **9.0.18** + Pomelo **9.0.0** + SqlServer **9.0.18** |
| C — OpenAPI / logs / identity | Swashbuckle **10.2.3**, Serilog.AspNetCore **10.0.0**, Identity.Web **3.15.1** |
| D — Azure / utils | AutoMapper **16.2.0**, Azure.* latest estável do levantamento, Graph **5.105.0** |
| AI | SK **1.78.0** (+ alphas/previews alinhados) |

### Por que CPM?

Antes:

```xml
<PackageReference Include="Newtonsoft.Json" Version="13.0.3" />
```

Depois:

```xml
<PackageReference Include="Newtonsoft.Json" />
```

com versão em `Directory.Packages.props` — evita drift entre API/Data/Domain/Service.

---

## 4. Ajustes técnicos esperados (a confirmar na execução)

| Área | Ajuste esperado | Arquivos candidatos |
| ---- | --------------- | ------------------- |
| Swashbuckle 10 / OpenAPI 2.x | Namespaces e security scheme | Config Swagger em API/Domain |
| EF 9 + Pomelo 9 | Alinhar providers; validar `UseMySql` | `HotelWise.Data`, `ServiceCollectionAddAllDependencies.cs` |
| LINQ / `Contains` | Preferir `List<T>` se avaliação falhar | Repositórios em Data |
| Semantic Kernel 1.78 | APIs renomeadas / plugins | Domain / Service AI |
| AutoMapper 16 | Profiles / DI | Service / Domain |
| Identity.Web 3.15 | Smoke auth (sem major 4 no v1) | API auth config |

---

## 5. Validações — baseline (pré-migração)

### SDK

```powershell
dotnet --list-sdks
```

Resultado no inventário:

```text
8.0.416
9.0.314
10.0.300
10.0.301
```

### Build baseline

```powershell
cd HotelWiseAPI
dotnet restore HotelWiseAPI.sln
dotnet build HotelWiseAPI.sln -c Release
```

| Item | Resultado (preencher na execução) |
| ---- | --------------------------------- |
| Erros | _a preencher_ |
| Avisos NU1903/NU1904 | Presentes no inventário (AutoMapper 15 / SK 1.41) |
| Data/hora | _a preencher_ |

### Testes

```powershell
dotnet test HotelWiseAPI.sln -c Release
```

| Item | Resultado |
| ---- | --------- |
| Projetos de teste | **0** |
| Status | **N/A — gap** |

### Pack GroqApiLibrary (alvo pós-migração)

```powershell
dotnet pack GroqApiLibrary/GroqApiLibrary.csproj -c Release -o ./artifacts/nupkg
```

| Item | Alvo | Resultado |
| ---- | ---- | --------- |
| `lib/net8.0/` | Sim | _pendente_ |
| `lib/net10.0/` | Sim | _pendente_ |

### Migrations

```powershell
dotnet ef migrations list --project HotelWise.Data --startup-project HotelWise.API
dotnet ef migrations add ValidacaoPosUpdateDotNet10 --project HotelWise.Data --startup-project HotelWise.API
# se Up/Down vazios:
dotnet ef migrations remove --force --project HotelWise.Data --startup-project HotelWise.API
```

| Item | Resultado |
| ---- | --------- |
| List | _pendente_ |
| Migration temporária | _pendente_ (esperado: vazia) |
| database update (teste) | _pendente_ |

### Execução da API

```powershell
dotnet run --project HotelWise.API
```

| Check | Resultado |
| ----- | --------- |
| Startup / DI | _pendente_ |
| Swagger | _pendente_ |
| Auth smoke | _pendente_ |
| AI smoke | _pendente_ |
| Serilog sem segredos | _pendente_ |

---

## 6. Resultados pós-migração (preencher)

```text
Data da execução: ____-__-__
Branch: chore/update-packages-hotelwiseapi-dotnet10
SDK usado: 10.0.x
Build Release: OK / FAIL (erros: _, avisos: _)
Pack GroqApiLibrary: OK / FAIL
Migrations: OK (vazia) / INVESTIGADA
Smoke API: OK / FAIL
Desvios do Conjunto v1: (lista ou nenhum)
```

### Quantitativo

```text
Projetos .NET atualizados: 6 / 6
Pacotes NuGet alinhados ao Conjunto v1: N
Testes automatizados: 0/0 (gap)
Vulnerabilidades NU1903/NU1904 resolvidas: sim/não
Falhas encontradas/corrigidas: N/N
```

---

## 7. Infraestrutura e CI

| Item | Baseline | Alvo | Status |
| ---- | -------- | ---- | ------ |
| `global.json` | Ausente | SDK 10.x + rollForward | Pendente |
| README (.NET SDK) | SDK 8 | SDK 10 | Pendente |
| Dockerfile API | Inexistente | N/A neste ciclo | N/A |
| QdrantDockerFile | Presente | Sem mudança | OK |
| Azure DevOps UseDotNet | Externo | 10.x | Pendente (fora do tree) |

---

## 8. Riscos residuais (válidos até o Conjunto v2)

| Risco | Status |
| ----- | ------ |
| Pomelo 9 → EF 9 (sem EF 10) | Aceito no v1 |
| SK InMemory/Qdrant 1.74-preview vs core 1.78 | Monitorar no smoke |
| Sem suíte de testes | Aceito; smoke manual |
| Identity.Web 3.x / Graph 5.x (majors adiadas) | Conjunto v2 |
| GroqToolLibrary órfão | Fora do ciclo |

---

## 9. Conclusão

**Neste momento a migração ainda não foi executada.** O baseline e o Conjunto Homologado v1 estão documentados. A execução deve seguir o `PlanoAcaoMigracaoDotNet10.md` / `PlanoImplementacaoMigracaoDotNet10-HotelWiseAPI.md` e atualizar as seções 5–8 deste relatório com evidências reais.

### Próximos passos

1. Executar fases 0–7 do plano de ação.  
2. Preencher este relatório (status → CONCLUÍDO).  
3. Abrir PR com `Directory.Packages.props`, `.csproj`, `global.json`, README e este relatório atualizado.

---

## 10. Referências

- `DOCUMENTACAO/GuiaGenericoAtualizacaoPacotes.md`
- `DOCUMENTACAO/UpdateDotNet10/PlanoAcaoMigracaoDotNet10.md`
- `DOCUMENTACAO/UpdateDotNet10/RascunhoPlanoUpdateDotNet10.md`
- `DOCUMENTACAO/API/2026-07-LevantamentoConjuntoHomologado-HotelWiseAPI.md`
- `DOCUMENTACAO/API/PlanoImplementacaoMigracaoDotNet10-HotelWiseAPI.md`

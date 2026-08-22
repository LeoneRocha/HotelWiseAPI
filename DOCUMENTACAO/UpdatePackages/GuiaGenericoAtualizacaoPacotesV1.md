# Guia Genérico — Atualização de Pacotes (.NET NuGet e Central Package Management) — V1

**Documento:** Guia operacional reutilizável (Versão 1 - HotelWiseAPI)  
**Baseado em:** [RascunhoPlanoUpdateDotNet10.md](file:///c:/git/HotelWise/HotelWiseAPI/DOCUMENTACAO/UpdateDotNet10/RascunhoPlanoUpdateDotNet10.md), [PlanoAcaoMigracaoDotNet10.md](file:///c:/git/HotelWise/HotelWiseAPI/DOCUMENTACAO/UpdateDotNet10/PlanoAcaoMigracaoDotNet10.md) e [RelatorioMigracaoDotNet10.md](file:///c:/git/HotelWise/HotelWiseAPI/DOCUMENTACAO/UpdateDotNet10/RelatorioMigracaoDotNet10.md)  
**Data:** 2026-08-22  
**Aplicabilidade:** Qualquer ciclo de atualização de dependências deste repositório (rotina periódica, upgrade de major, migração de runtime/framework).  

**Ciclo corrente (exemplo concreto de referência):** migração **HotelWiseAPI** .NET 8 → .NET 10 — ver [2026-07-LevantamentoConjuntoHomologado-HotelWiseAPI.md](file:///c:/git/HotelWise/HotelWiseAPI/DOCUMENTACAO/API/2026-07-LevantamentoConjuntoHomologado-HotelWiseAPI.md) e [PlanoImplementacaoMigracaoDotNet10-HotelWiseAPI.md](file:///c:/git/HotelWise/HotelWiseAPI/DOCUMENTACAO/API/PlanoImplementacaoMigracaoDotNet10-HotelWiseAPI.md).

---

## 1. Objetivo

Padronizar como atualizar dependências de pacotes em toda a solução backend **HotelWiseAPI**, preservando:

- Compatibilidade do artefato distribuído externamente (pacote NuGet publicável [GroqApiLibrary](file:///c:/git/HotelWise/HotelWiseAPI/GroqApiLibrary/GroqApiLibrary.csproj) com multi-targeting `net8.0;net10.0`)
- Integridade de migrations EF Core, seeds, constraints e schemas de banco de dados (MySQL via Pomelo / SQL Server)
- Funcionamento e estabilidade da API REST ([HotelWise.API](file:///c:/git/HotelWise/HotelWiseAPI/HotelWise.API/HotelWise.API.csproj)), POCs de console ([HotelWise.ConsolePOC](file:///c:/git/HotelWise/HotelWiseAPI/HotelWise.ConsolePOC/HotelWise.ConsolePOC.csproj)), Injeção de Dependências (DI), logging (Serilog), telemetria (Application Insights) e middlewares
- Ecossistema de Inteligência Artificial e RAG: **Semantic Kernel**, conectores de IA (Mistral AI, Ollama, Groq), Vector Stores (**Qdrant**, InMemory) e manipulação de documentos
- Build local, containers Docker ([QdrantDockerFile](file:///c:/git/HotelWise/HotelWiseAPI/QdrantDockerFile/docker-compose.yml)), stacks locais em `IA_Local/`, scripts de auditoria ([check_packages.ps1](file:///c:/git/HotelWise/HotelWiseAPI/check_packages.ps1)) e pipelines CI/CD
- Zero alteração de regra de negócio ou contrato público durante o ciclo de atualização de pacotes

Este guia é genérico: as versões concretas de cada ciclo devem ser registradas em um documento filho por execução (o "Conjunto Homologado" daquele ciclo — ver Seção 5), nunca hardcoded aqui.

---

## 2. Escopo e não escopo

### 2.1 Escopo

| Categoria | Ação |
| --------- | ---- |
| Projetos .NET da solução (`HotelWise.API`, `HotelWise.Service`, `HotelWise.Data`, `HotelWise.Domain`, `HotelWise.ConsolePOC`) | Atualizar pacotes NuGet via Central Package Management ([Directory.Packages.props](file:///c:/git/HotelWise/HotelWiseAPI/Directory.Packages.props)); atualizar TFM apenas em ciclos de migração de runtime |
| Pacote NuGet publicável ([GroqApiLibrary](file:///c:/git/HotelWise/HotelWiseAPI/GroqApiLibrary/GroqApiLibrary.csproj)) | Atualizar dependências preservando multi-targeting (`net8.0;net10.0`) e validação via `dotnet pack` |
| Persistência e Migrations ([HotelWise.Data](file:///c:/git/HotelWise/HotelWiseAPI/HotelWise.Data/HotelWise.Data.csproj)) | Validar contexto `HotelWiseDbContextMysql`, migrations em `Migrations/MySql/` e estabilidade de schema DDL |
| Docker e Infraestrutura ([QdrantDockerFile](file:///c:/git/HotelWise/HotelWiseAPI/QdrantDockerFile/docker-compose.yml), `IA_Local/`) | Atualizar imagens base e configurações quando o ciclo envolver mudanças de infraestrutura ou runtime |
| Scripts de automação ([check_packages.ps1](file:///c:/git/HotelWise/HotelWiseAPI/check_packages.ps1)) | Atualizar referências e comandos caso necessário |
| Pipelines CI/CD e SDK ([global.json](file:///c:/git/HotelWise/HotelWiseAPI/global.json), Azure DevOps) | Alinhar versão de SDK .NET (`10.0.301` / `UseDotNet@2`) |

### 2.2 Não escopo

- Frontend [HotelWiseUI](https://github.com/LeoneRocha/HotelWiseUI) (reside em repositório externo separado; atualizações de npm/React/Vite são tratadas em ciclo próprio)
- Alteração de regras de negócio, contratos REST, payloads JSON ou schemas de banco sem necessidade técnica
- Refatoração de domínio ou preferências arquiteturais não relacionadas à atualização
- Projetos órfãos não incluídos na solução (ex.: [GroqToolLibrary.csproj](file:///c:/git/HotelWise/HotelWiseAPI/GroqApiLibrary/GroqToolLibrary.csproj))
- Troca de bibliotecas por equivalentes (ex.: troca de ORM ou provedor Pomelo — decisão arquitetural separada, com RFC própria)

Qualquer mudança fora do escopo deve ser registrada e tratada em PR separado.

---

## 3. Princípios obrigatórios

1. **Inventário antes de alterar** — nunca atualizar sem primeiro gerar a lista do que está desatualizado e vulnerável (Seção 4).
2. **Conjunto Homologado por ciclo** — cada ciclo de atualização produz uma tabela "pacote / versão atual / versão a aplicar / latest disponível / justificativa quando não for a latest". Só entram versões estáveis (sem `preview`, `rc`, `beta`, `next`, `canary`) em produção, exceto pacotes de IA que exigem sincronismo em previews coordenadas (ex.: SK connectors).
3. **Atualizar por blocos coesos, nunca pacote a pacote isolado** — pacotes do mesmo ecossistema sobem juntos (ex.: todos `Microsoft.AspNetCore.*` e `Microsoft.Extensions.*` no mesmo patch; família `Microsoft.EntityFrameworkCore.*` alinhada; ferramentas `Microsoft.SemanticKernel.*` coordenadas).
4. **Respeitar dependências rígidas do grafo** — quando um pacote trava a major de outro (ex.: provider de banco `Pomelo.EntityFrameworkCore.MySql` 9.x travando a major do `Microsoft.EntityFrameworkCore` em 9.x), documentar a trava e NÃO forçar a latest. Registrar a condição de destrave para o próximo ciclo ("Conjunto v2 futuro").
5. **Abordagem incremental por fases** — validar build e smoke ao final de cada fase; nunca alterar tudo de uma vez.
6. **Centralização de versões via CPM** — a solução usa Central Package Management ([Directory.Packages.props](file:///c:/git/HotelWise/HotelWiseAPI/Directory.Packages.props) como fonte única; `.csproj` sem atributo `Version`).
7. **Não quebrar pacotes distribuídos** — [GroqApiLibrary](file:///c:/git/HotelWise/HotelWiseAPI/GroqApiLibrary/GroqApiLibrary.csproj) mantém multi-targeting (`TargetFrameworks: net8.0;net10.0`) e validação via `dotnet pack`.
8. **Branch dedicada com commits por fase** — ex.: `chore/update-packages-YYYY-MM` ou `chore/update-packages-hotelwiseapi-dotnet10`.
9. **Major bump exige atenção individual** — ler changelog/breaking changes antes de aplicar; um major de terceiro nunca sobe "de carona" no lote.
10. **Nenhuma migration/schema novo por causa de atualização** — se a atualização gerar migration não-vazia ou diff de schema, investigar antes de commitar.

---

## 4. Fase de inventário (sempre a primeira)

### 4.1 .NET / NuGet

Comandos para inventário da solução:

```powershell
dotnet --list-sdks
dotnet list HotelWiseAPI.sln package --outdated
dotnet list HotelWiseAPI.sln package --vulnerable --include-transitive
dotnet list HotelWiseAPI.sln package
```

Ou através do script utilitário:

```powershell
.\check_packages.ps1
```

Gerar tabelas:

| Projeto | Caminho | Tipo | TFM atual | Publicável? | No .sln? |
| ------- | ------- | ---- | --------- | ----------- | -------- |
| [HotelWise.API](file:///c:/git/HotelWise/HotelWiseAPI/HotelWise.API/HotelWise.API.csproj) | `HotelWise.API/` | ASP.NET Core Web API (Host) | net10.0 | Não | Sim |
| [HotelWise.Service](file:///c:/git/HotelWise/HotelWiseAPI/HotelWise.Service/HotelWise.Service.csproj) | `HotelWise.Service/` | Class Library (Services / AI wiring) | net10.0 | Não | Sim |
| [HotelWise.Data](file:///c:/git/HotelWise/HotelWiseAPI/HotelWise.Data/HotelWise.Data.csproj) | `HotelWise.Data/` | Class Library (EF Core / MySQL / SqlServer) | net10.0 | Não | Sim |
| [HotelWise.Domain](file:///c:/git/HotelWise/HotelWiseAPI/HotelWise.Domain/HotelWise.Domain.csproj) | `HotelWise.Domain/` | Class Library (Domain / Entities / SK) | net10.0 | Não | Sim |
| [GroqApiLibrary](file:///c:/git/HotelWise/HotelWiseAPI/GroqApiLibrary/GroqApiLibrary.csproj) | `GroqApiLibrary/` | Class Library (Cliente Groq) | net8.0;net10.0 | **Sim (NuGet)** | Sim |
| [HotelWise.ConsolePOC](file:///c:/git/HotelWise/HotelWiseAPI/HotelWise.ConsolePOC/HotelWise.ConsolePOC.csproj) | `HotelWise.ConsolePOC/` | Console Application (POC Ollama) | net10.0 | Não | Sim |
| [GroqToolLibrary](file:///c:/git/HotelWise/HotelWiseAPI/GroqApiLibrary/GroqToolLibrary.csproj) | `GroqApiLibrary/` | Class Library (Órfão) | net8.0 | Não | Não |

| Pacote | Versão atual | Latest stable | Versão a aplicar | Justificativa se diferente da latest |
| ------ | ------------ | ------------- | ---------------- | ------------------------------------ |

Inventário de referência do ciclo .NET 10: [2026-07-LevantamentoConjuntoHomologado-HotelWiseAPI.md](file:///c:/git/HotelWise/HotelWiseAPI/DOCUMENTACAO/API/2026-07-LevantamentoConjuntoHomologado-HotelWiseAPI.md).

### 4.2 Frontend companion (HotelWiseUI)

> No ciclo HotelWiseAPI backend, o frontend reside no repositório companion [HotelWiseUI](https://github.com/LeoneRocha/HotelWiseUI). Quando sincronizado, seguir comandos npm padrão (`npm outdated`, `npm audit`, `npm test`, `npm run build`).

---

## 5. Conjunto Homologado — regras de montagem

### 5.1 Blocos .NET (modelo)

Organizar o conjunto em blocos, na ordem de dependência:

- **Bloco A — Plataforma** (`Microsoft.AspNetCore.Authentication.JwtBearer`, `Microsoft.Extensions.*`, `System.Text.Json`, `Microsoft.Kiota.*`): todos no MESMO patch do ciclo do runtime alvo (.NET 10).
- **Bloco B — Persistência** (`Microsoft.EntityFrameworkCore.*` + providers `Pomelo.EntityFrameworkCore.MySql`, `Microsoft.EntityFrameworkCore.SqlServer`): todos na MESMA major, limitada pela major suportada pelo provider mais restritivo. Documentar a trava (ex.: "Pomelo 9.x exige EF Core <= 9.x") e a condição de destrave.
- **Bloco C — OpenAPI, logging, tokens e segurança** (`Swashbuckle.AspNetCore`, `Swashbuckle.AspNetCore.Filters`, `Serilog.*`, `Microsoft.IdentityModel.JsonWebTokens`, `Microsoft.Identity.Web`, `Microsoft.ApplicationInsights.AspNetCore`): Swashbuckle segue compatibilidade com ASP.NET Core e Serilog alinhado.
- **Bloco D — Domínio, utilitários, nuvem e documentos** (`AutoMapper`, `FluentValidation`, `Newtonsoft.Json`, `Polly`, `Bogus`, `HtmlAgilityPack`, `Markdig`, `DocumentFormat.OpenXml`, `PDFsharp`, `QuestPDF`, `Azure.*`, `Microsoft.Graph`): latest estável, com atenção a licenças em majors novos (ex.: AutoMapper 15+).
- **Bloco AI — Inteligência Artificial e Vector Store** (`Microsoft.SemanticKernel.*`, `Microsoft.Extensions.AI`, `Microsoft.Extensions.VectorData.Abstractions`, `CommunityToolkit.VectorData.*`, `OllamaSharp`, `Mistral.SDK`): família Semantic Kernel alinhada na mesma versão/preview coordenada.

Dependências rígidas típicas (validar a cada ciclo):

| Se usar | Então obrigatoriamente |
| ------- | ---------------------- |
| Provider `Pomelo.EntityFrameworkCore.MySql` na major N (ex.: 9.0.0) | Todos `Microsoft.EntityFrameworkCore.*` na major N (ex.: 9.0.18) |
| Runtime `net10.0` em Web API / Hosts | Todos `Microsoft.AspNetCore.*` / `Microsoft.Extensions.*` no patch do ciclo (.NET 10) |
| Swashbuckle v10+ | Namespaces atualizados (`Microsoft.OpenApi` em vez de `Microsoft.OpenApi.Models`) |
| VectorData Abstractions (May 2025+) | Atributos `[VectorStoreRecordKey]`, `[VectorStoreRecordData]`, `[VectorStoreRecordVector]` |
| `GroqApiLibrary` multi-target `net8.0;net10.0` | `dotnet pack` deve gerar pastas `lib/net8.0/` e `lib/net10.0/` válidas |

Aplicação: todas as versões entram/atualizam em [Directory.Packages.props](file:///c:/git/HotelWise/HotelWiseAPI/Directory.Packages.props); os arquivos `.csproj` permanecem sem o atributo `Version=`.

---

## 6. Plano de execução por fases

```mermaid
flowchart TD
    F0[Fase 0 - Preparação e baseline] --> F1[Fase 1 - CPM e GroqApiLibrary multi-target]
    F1 --> F2[Fase 2 - Camadas internas Domain / Data / Service]
    F2 --> F3[Fase 3 - Hosts executáveis HotelWise.API e ConsolePOC]
    F3 --> F4[Fase 4 - Persistência e EF Core Migrations]
    F4 --> F5[Fase 5 - Docker e Scripts de auditoria]
    F5 --> F6[Fase 6 - CI/CD e Evidências]
```

- **Fase 0 — Preparação**: branch dedicada; inventário (Seção 4); montar Conjunto Homologado (Seção 5); commit baseline com build verde no estado atual.
- **Fase 1 — CPM e Packable**: atualizar [Directory.Packages.props](file:///c:/git/HotelWise/HotelWiseAPI/Directory.Packages.props) e validar o build/pack de [GroqApiLibrary](file:///c:/git/HotelWise/HotelWiseAPI/GroqApiLibrary/GroqApiLibrary.csproj) (`net8.0;net10.0`).
- **Fase 2 — Camadas internas**: aplicar blocos A, B, D e AI em [HotelWise.Domain](file:///c:/git/HotelWise/HotelWiseAPI/HotelWise.Domain/HotelWise.Domain.csproj), [HotelWise.Data](file:///c:/git/HotelWise/HotelWiseAPI/HotelWise.Data/HotelWise.Data.csproj) e [HotelWise.Service](file:///c:/git/HotelWise/HotelWiseAPI/HotelWise.Service/HotelWise.Service.csproj).
- **Fase 3 — Hosts executáveis**: aplicar blocos A, C e AI em [HotelWise.API](file:///c:/git/HotelWise/HotelWiseAPI/HotelWise.API/HotelWise.API.csproj) e [HotelWise.ConsolePOC](file:///c:/git/HotelWise/HotelWiseAPI/HotelWise.ConsolePOC/HotelWise.ConsolePOC.csproj).
- **Fase 4 — Persistência e Migrations**: validar migrations EF Core com `HotelWiseDbContextMysql`, gerar migration temporária para validar integridade DDL e aplicar seeds.
- **Fase 5 — Containers e scripts**: verificar [QdrantDockerFile/docker-compose.yml](file:///c:/git/HotelWise/HotelWiseAPI/QdrantDockerFile/docker-compose.yml), stacks `IA_Local/` e script [check_packages.ps1](file:///c:/git/HotelWise/HotelWiseAPI/check_packages.ps1).
- **Fase 6 — CI/CD e evidências**: alinhar [global.json](file:///c:/git/HotelWise/HotelWiseAPI/global.json) e pipelines do Azure DevOps; gerar relatório final do ciclo.

Exemplo detalhado fase a fase do ciclo .NET 10: [PlanoImplementacaoMigracaoDotNet10-HotelWiseAPI.md](file:///c:/git/HotelWise/HotelWiseAPI/DOCUMENTACAO/API/PlanoImplementacaoMigracaoDotNet10-HotelWiseAPI.md).

---

## 7. Checklist de validação

### 7.1 .NET — build e restore

```powershell
dotnet restore HotelWiseAPI.sln
dotnet build HotelWiseAPI.sln -c Release
```

- [ ] Restore sem `NU1107` (conflito de versão) e `NU1202` (TFM incompatível)
- [ ] Build Release com 0 erros; warnings de obsolescência corrigidos ou justificados
- [ ] Warnings `NU1903` / `NU1904` (vulnerabilidades) mitigados com pins transitivos quando necessário

### 7.2 .NET — EF Core / migrations

```powershell
$env:ASPNETCORE_ENVIRONMENT = "Development"

dotnet ef migrations list `
  --project HotelWise.Data/HotelWise.Data.csproj `
  --startup-project HotelWise.API/HotelWise.API.csproj `
  --context HotelWiseDbContextMysql

dotnet ef database update `
  --project HotelWise.Data/HotelWise.Data.csproj `
  --startup-project HotelWise.API/HotelWise.API.csproj `
  --context HotelWiseDbContextMysql
```

- [ ] `migrations list` e `database update` em banco local sem erro
- [ ] Técnica da migration temporária: gerar migration `ValidacaoPosUpdate`; se vier com `Up`/`Down` vazios (ou apenas updates estáveis de seed), a atualização não alterou schema estrutural DDL (esperado) — remover com `dotnet ef migrations remove --force`
- [ ] Seeds verificados no banco: User `admin`, Hotel `Hotel Example`, Room `Quarto Example`

### 7.3 .NET — pack do SDK publicável (GroqApiLibrary)

```powershell
dotnet pack GroqApiLibrary/GroqApiLibrary.csproj -c Release -o ./artifacts/nupkg
```

- [ ] Pacote `.nupkg` gerado com sucesso em `./artifacts/nupkg`
- [ ] Pacote contém pastas `lib/net8.0/` e `lib/net10.0/`

### 7.4 .NET — execução da API (smoke)

```powershell
dotnet run --project HotelWise.API/HotelWise.API.csproj
```

- [ ] Startup sem `InvalidOperationException` de DI
- [ ] Endpoint `GET /health` retornando 200
- [ ] Swagger acessível (`/swagger` ou `/swagger/index.html`)
- [ ] Headers de segurança e `X-Correlation-ID` operando nos logs
- [ ] Endpoints de IA e Semantic Kernel respondendo normalmente
- [ ] Logs do Serilog estruturados e sem credenciais expostas

---

## 8. Docker, DevContainer, scripts e CI/CD (quando o runtime mudar)

| Item | O que verificar |
| ---- | --------------- |
| Docker / Vector Store ([QdrantDockerFile](file:///c:/git/HotelWise/HotelWiseAPI/QdrantDockerFile/docker-compose.yml)) | `docker compose build --no-cache && docker compose up -d`; container Qdrant acessível nas portas `6333` e `6334` |
| Stacks locais (`IA_Local/`) | Configurações do Ollama / modelos locais compatíveis com as versões das bibliotecas |
| SDK .NET ([global.json](file:///c:/git/HotelWise/HotelWiseAPI/global.json)) | SDK pin `10.0.301` com `rollForward: latestFeature` |
| Pipelines Azure DevOps | Task `UseDotNet@2` configurada para `10.x` |
| Scripts de auditoria ([check_packages.ps1](file:///c:/git/HotelWise/HotelWiseAPI/check_packages.ps1)) | Execução sem erros de parsing ou codificação |

---

## 9. Evidências obrigatórias da entrega

1. **Conjunto Homologado do ciclo** — documento em `DOCUMENTACAO/API/<AAAA-MM>-LevantamentoConjuntoHomologado-HotelWiseAPI.md` com tabelas aplicadas e justificativas.
2. **Lista de arquivos alterados** — [Directory.Packages.props](file:///c:/git/HotelWise/HotelWiseAPI/Directory.Packages.props), `.csproj`, Dockerfiles, pipelines, scripts, [global.json](file:///c:/git/HotelWise/HotelWiseAPI/global.json).
3. **Relatório quantitativo:**

```text
Projetos .NET atualizados: 6 / 6
Pacotes NuGet atualizados: N
Testes automatizados: 0 (gap documentado; validação via smoke/build/pack)
Vulnerabilidades resolvidas: N
Falhas encontradas/corrigidas: N/N
Migrations validadas: Sim
```

4. **Riscos residuais** — majors adiados (ex.: Pomelo 10 / EF Core 10), travas de grafo, warnings pendentes.

---

## 10. Plano de rollback

```powershell
git checkout <branch-do-ciclo>
git reset --hard <commit-baseline>

dotnet restore HotelWiseAPI.sln
dotnet build HotelWiseAPI.sln -c Release
dotnet pack GroqApiLibrary/GroqApiLibrary.csproj -c Release
```

Restaurar em conjunto: [Directory.Packages.props](file:///c:/git/HotelWise/HotelWiseAPI/Directory.Packages.props), `.csproj`, [global.json](file:///c:/git/HotelWise/HotelWiseAPI/global.json), scripts e arquivos de configuração.

---

## 11. Riscos e mitigações recorrentes

| Risco | Impacto | Mitigação |
| ----- | ------- | --------- |
| Provider trava major do ORM (`Pomelo` x `EF Core`) | Não é possível usar a latest do EF Core | Segurar o bloco inteiro na major 9.x; documentar destrave para ciclo futuro ("Conjunto v2") |
| Mistura de patches `Microsoft.*` | `NU1107` / restore instável | CPM ([Directory.Packages.props](file:///c:/git/HotelWise/HotelWiseAPI/Directory.Packages.props)) + Bloco A no mesmo patch (10.0.10) |
| Major bump silencioso de terceiro (ex.: AutoMapper 15+) | Breaking changes ou mudanças de licença | Major nunca sobe no lote; avaliar changelog e impacto |
| Mudanças na API de Vector Data do Semantic Kernel | Erros de compilação em atributos e buscas | Alinhar modelo de atributos (`[VectorStoreRecordKey]`, etc.) conforme convenção da versão homologada |
| Artefato publicável perde compatibilidade | Consumidores externos quebram | Multi-targeting `net8.0;net10.0` no `GroqApiLibrary` + validação de `dotnet pack` |
| Migration não-vazia pós-update | Alteração de schema não intencional | Técnica da migration temporária (7.2); investigar antes de commitar |
| Pipeline com SDK desalinhado | CI vermelho ou build divergente do local | Fase 6 obrigatória; alinhar `UseDotNet@2` para `10.x` |

---

## 12. Modo de execução sugerido (para IA/agente)

1. Ler este guia e gerar o inventário completo (Seção 4) sem alterar o código.
2. Propor o Conjunto Homologado do ciclo em documento filho sob `DOCUMENTACAO/API/` (ex.: `DOCUMENTACAO/API/<AAAA-MM>-LevantamentoConjuntoHomologado-HotelWiseAPI.md`) e aguardar aprovação.
3. Executar por fases (Seção 6), commitando por fase e preenchendo os checklists (Seção 7).
4. Validar persistência, EF Core migrations e o pack do `GroqApiLibrary`.
5. Atualizar infraestrutura e CI conforme necessário (Seção 8).
6. Entregar relatório final com evidências (Seção 9) e abrir PR.

---

## Referências

- [Directory.Packages.props](file:///c:/git/HotelWise/HotelWiseAPI/Directory.Packages.props) — fonte única de versões NuGet (CPM)
- [global.json](file:///c:/git/HotelWise/HotelWiseAPI/global.json) — especificação do SDK .NET
- [README.md](file:///c:/git/HotelWise/HotelWiseAPI/README.md) — visão geral da API e arquitetura
- [RascunhoPlanoUpdateDotNet10.md](file:///c:/git/HotelWise/HotelWiseAPI/DOCUMENTACAO/UpdateDotNet10/RascunhoPlanoUpdateDotNet10.md) — RFC original do plano de atualização
- [PlanoAcaoMigracaoDotNet10.md](file:///c:/git/HotelWise/HotelWiseAPI/DOCUMENTACAO/UpdateDotNet10/PlanoAcaoMigracaoDotNet10.md) — plano de ação operacional
- [RelatorioMigracaoDotNet10.md](file:///c:/git/HotelWise/HotelWiseAPI/DOCUMENTACAO/UpdateDotNet10/RelatorioMigracaoDotNet10.md) — relatório de evidências de migração
- [2026-07-LevantamentoConjuntoHomologado-HotelWiseAPI.md](file:///c:/git/HotelWise/HotelWiseAPI/DOCUMENTACAO/API/2026-07-LevantamentoConjuntoHomologado-HotelWiseAPI.md) — inventário e Conjunto Homologado v1
- [PlanoImplementacaoMigracaoDotNet10-HotelWiseAPI.md](file:///c:/git/HotelWise/HotelWiseAPI/DOCUMENTACAO/API/PlanoImplementacaoMigracaoDotNet10-HotelWiseAPI.md) — checklist fase a fase

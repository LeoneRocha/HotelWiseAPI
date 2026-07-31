# Rascunho / RFC — Migração HotelWiseAPI .NET 8 → .NET 10

**Documento:** RFC + prompt operacional para IA/agente  
**Solução:** `HotelWiseAPI/HotelWiseAPI.sln`  
**Data:** 2026-07-31  
**Status:** Planejado (não executado)  
**Planos oficiais:**  
- `DOCUMENTACAO/UpdateDotNet10/PlanoAcaoMigracaoDotNet10.md`  
- `DOCUMENTACAO/API/2026-07-LevantamentoConjuntoHomologado-HotelWiseAPI.md`  
- `DOCUMENTACAO/API/PlanoImplementacaoMigracaoDotNet10-HotelWiseAPI.md`

---

## Prompt para Cursor IA

**Objetivo:**  
Atualizar a solução **HotelWiseAPI** de **.NET 8** para **.NET 10**, aplicando o **Conjunto Homologado v1**, garantindo build, migrations EF, pack do `GroqApiLibrary`, startup da API/Console e smoke de DI/auth/AI — **sem** alterar regras de negócio nem contratos públicos.

### Tarefas principais

- Migrar projetos do `.sln` para `net10.0` (exceto `GroqApiLibrary` → `net8.0;net10.0`).
- Introduzir CPM (`HotelWiseAPI/Directory.Packages.props`) e remover `Version=` dos `.csproj`.
- Aplicar **somente** versões do Conjunto Homologado v1 (não improvisar).
- Validar migrations MySQL/Pomelo (técnica da migration temporária).
- Build Release da solução; `dotnet pack` do GroqApiLibrary.
- Executar API e ConsolePOC; validar DI, Swagger, auth, fluxo AI básico.
- Atualizar README / `global.json` para SDK 10; anotar pipeline Azure DevOps externo.
- Preencher `RelatorioMigracaoDotNet10.md` com evidências.

### Pontos de atenção

- **Pomelo oficial = 9.0.0** → EF Core permanece em **9.0.18** no runtime `net10.0` (não forçar EF 10).
- **Não usar** forks Pomelo (Microting / omarbaruzzo) sem RFC.
- Persistência dual: MySQL (Pomelo) **e** SqlServer na **mesma major EF 9**.
- Stack AI: SK **1.78.0** + alphas; InMemory/Qdrant **1.74.0-preview** (risco residual).
- **Não há projetos de teste** — smoke manual é obrigatório.
- Não existe Dockerfile .NET da API (só `QdrantDockerFile/`).
- Frontend `HotelWiseUI` está **fora** deste RFC.

### Critérios de aceite

- [ ] Projetos internos/API/Console em `net10.0`; GroqApiLibrary em `net8.0;net10.0`.
- [ ] CPM ativo; versões = Conjunto v1.
- [ ] Build Release 0 erros; pack com `lib/net8.0` e `lib/net10.0`.
- [ ] Migrations validadas; migration temporária vazia (ou desvio justificado).
- [ ] API sobe; DI OK; Swagger OK; smoke auth + AI.
- [ ] README + `global.json` SDK 10; CI externo anotado.
- [ ] Relatório de evidências preenchido.

### Diretrizes para a IA

1. Ler o Conjunto Homologado e o Plano de Implementação **antes** de editar código.
2. Abordagem incremental por fases (CPM → Groq → Domain/Data/Service → API → EF → código → infra).
3. Validar `dotnet build -c Release` ao fim de cada fase.
4. Em `GroqApiLibrary`, usar `#if NET8_0` / `#if NET10_0` só se houver breaking de API entre TFMs.
5. Não criar testes novos neste ciclo (salvo se indispensável para destravar compile).
6. Não alterar `HotelWiseUI`.
7. Não commitar/PR sem pedido explícito do responsável.

### Modo de execução sugerido

1. Apresentar plano de alteração dos `.csproj` + amostra do `Directory.Packages.props` alinhada ao Conjunto v1.  
2. Aguardar validação (se o usuário pedir).  
3. Executar fases 0–7.  
4. Preencher o relatório.

---

# RFC-001 — Migração HotelWiseAPI para .NET 10

## 1. Objetivo

Migrar a plataforma backend HotelWise de `net8.0` para `net10.0`, centralizando NuGet via CPM, preservando o pacote `GroqApiLibrary` multi-target e a compatibilidade Pomelo/EF sem regressões funcionais.

## 2. Escopo

| Inclui | Não inclui |
| ------ | ---------- |
| 6 projetos do `HotelWiseAPI.sln` | `HotelWiseUI` |
| CPM + Conjunto Homologado v1 | Fork Pomelo / EF 10 |
| Migrations EF (MySQL) | Nova suíte de testes |
| README / global.json / nota CI | Docker aspnet da API (inexistente) |
| Stack AI alinhada (SK/KM) | Mudança de contratos REST |

## 3. Arquitetura atual

```text
HotelWise.API (Web)
  └── HotelWise.Service
        ├── HotelWise.Data     (EF + Pomelo MySQL + SqlServer)
        └── HotelWise.Domain   (Semantic Kernel, Kernel Memory, Serilog, Swagger)
              └── GroqApiLibrary (NuGet packable)

HotelWise.ConsolePOC
```

- Auth: `Microsoft.Identity.Web` + JWT  
- Logging: Serilog  
- AI: Semantic Kernel, Kernel Memory, OllamaSharp, Mistral, Qdrant (infra Docker à parte)

## 4. Estratégia de migração

1. **Inventário e Conjunto Homologado** (já feito em `DOCUMENTACAO/API/`).
2. **CPM primeiro** como fonte única de versões.
3. **Packable primeiro** (`GroqApiLibrary` multi-target).
4. **Bibliotecas** Domain → Data → Service.
5. **Executáveis** API + ConsolePOC.
6. **EF / migrations** com prova de schema inalterado.
7. **Ajustes de código** (Swagger 10, LINQ, SK, AutoMapper).
8. **Infra documental** (SDK 10) + relatório.

## 5. Análise de impacto

| Área | Impacto | Severidade |
| ---- | ------- | ---------- |
| TFM net10 | Recompile + peers AspNet 10 | Alta |
| EF 8→9 + Pomelo 8→9 | Migrations / provider | Alta |
| Swashbuckle 9→10 | OpenAPI 2.x | Média |
| SK 1.41→1.78 | APIs AI / CVE fix | Alta |
| AutoMapper 15→16 | Mapping + licença | Média |
| Identity.Web 3.15 (sem major 4) | Baixo no v1 | Baixa |
| Ausência de testes | Regressão silenciosa | Alta (mitigar com smoke) |

## 6. Riscos e mitigações

| Risco | Mitigação |
| ----- | --------- |
| `NU1107` EF10+Pomelo9 | Conjunto v1 = EF 9.0.18 |
| SK preview desencontrado | Smoke Qdrant; fallback de versão |
| CI Node/.NET desalinhado | `global.json` + UseDotNet 10.x |
| Sem testes | Checklist smoke no plano |

## 7. Plano de rollback

`git reset --hard <baseline>` + `dotnet restore/build`. Restaurar `Directory.Packages.props` + `.csproj` + `global.json` juntos.

## 8. Qualidade e evidências

- Build Release 0 erros  
- Pack Groq com dois TFMs  
- Migration temporária vazia  
- Smoke API / auth / AI  
- Relatório preenchido em `RelatorioMigracaoDotNet10.md`

## 9. Pipeline CI/CD

- Pipeline atual: Azure DevOps externo (`lionscorp.visualstudio.com/...`) — fora do tree.  
- Ação: alinhar SDK para **10.x** na task correspondente.  
- Local: `global.json` com rollForward `latestFeature`.

## 10. Observabilidade

- Manter Serilog; validar sinks Console/File após upgrade.  
- Não introduzir Application Insights neste ciclo (não é dependência atual obrigatória).

## 11. Checklist resumido

- [ ] Conjunto v1 aplicado via CPM  
- [ ] TFMs corretos  
- [ ] Build + pack OK  
- [ ] EF migrations OK  
- [ ] Smoke API OK  
- [ ] Docs + relatório OK  

## 12. Referências

- `DOCUMENTACAO/GuiaGenericoAtualizacaoPacotes.md`  
- `DOCUMENTACAO/UpdateDotNet10/PlanoAcaoMigracaoDotNet10.md`  
- `DOCUMENTACAO/API/2026-07-LevantamentoConjuntoHomologado-HotelWiseAPI.md`  
- `DOCUMENTACAO/API/PlanoImplementacaoMigracaoDotNet10-HotelWiseAPI.md`  
- Pomelo EF10 tracking: https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql/issues/2007

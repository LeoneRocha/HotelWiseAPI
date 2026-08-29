# Análise — SmartDigitalPsico.WebAPI

**Versão:** 1.0  
**Data:** 2026-08-04  
**Projeto:** `SmartDigitalPsico.WebAPI`  
**Onda:** 4 ([PlanoExecucao-PorProjeto.md](./PlanoExecucao-PorProjeto.md))  
**Docs pai:** [Levantamento.md](./Levantamento.md) · [PlanoDeAcao.md](./PlanoDeAcao.md) · [Progresso.md](./Progresso.md)

---

## 1. Papel na migração

WebAPI é **quase só consumidor**. Não concentra implementações genéricas a portar (exceto indiretamente via Domain: `ApiBaseController` já portado na Onda 1).

Trabalho principal: `ProjectReference` ao Core, atualizar `using`s em controllers/filters/Program, smoke de API. Controllers e middlewares de produto = **Manter**.

---

## 2. Portar+Obsoletar

| Tipo | Situação no WebAPI |
| ---- | ------------------ |
| *(nenhum tipo de produção exclusivo WebAPI inventariado para Core)* | — |
| Uso de `ApiBaseController` | Controller base herda tipo **canônico no Core** (Obsolete no Domain); atualizar `using` |

Se algum helper local exclusivo existir no WebAPI no futuro, avaliar à parte — hoje o inventário aponta genéricos em Domain/Data/Service.

---

## 3. Manter

| Área | Motivo |
| ---- | ------ |
| Todos os Controllers `Controllers/v1/**` | API de produto |
| Auth/JWT setup, Swagger, pipeline | Host ASP.NET |
| Enrichers registrados na API | Domínio (tipos Manter no Domain) |
| `NotificationTemplateController` e demais SystemDomains/Principals | Produto |
| Medical Schedule endpoints | Produto |

---

## 4. Dependências

| Precisa (antes) | Ação |
| --------------- | ---- |
| Ondas 1–3 concluídas (tipos canônicos no Core) | Atualizar usings |
| ProjectReference `SmartDigitalPsico.Core.SDK` | Adicionar no `.csproj` |
| Domain/Data/Service já referenciando Core | Evitar misturar Obsolete em controllers |

---

## 5. Lotes internos

| Lote | Conteúdo | Paralelo? |
| ---- | -------- | --------- |
| W1 | ProjectReference + usings globais / `_Imports` / Program | Sequencial |
| W2 | Controllers por área (SystemDomains, Principals, Schedule) — só usings | Pode paralelizar PRs por pasta **após** W1 |
| W3 | Smoke: build, WebAPI.Test, health/endpoints críticos | Sequencial ao final |

---

## 6. Checklist Obsolete + usings

- [ ] WebAPI referencia Core.SDK
- [ ] Controllers usam namespaces Core para tipos portados (`ServiceResponse`, `ApiBaseController`, etc.)
- [ ] Zero warning Obsolete novo nos controllers (corrigir using)
- [ ] Nenhum arquivo de controller apagado ou “movido” para Core
- [ ] `dotnet build` WebAPI + `WebAPI.Test` verdes
- [ ] Smoke manual/automatizado de endpoints principais

---

## 7. Riscos

| Risco | Mitigação |
| ----- | --------- |
| Usings parciais (parte Obsolete, parte Core) | Grep por namespaces antigos dos tipos Portar |
| Hypermedia filters | Framework no Core; enrichers continuam Domain — registrar como hoje |
| Breaking contrato JSON | Não alterar DTOs de produto; só origem do tipo base |

---

## 8. Backlog Schedule

Endpoints Medical/Schedule **Manter**. Quando Schedule Core for portado (doc dedicado), WebAPI só atualiza usings de novo — sem mover controllers.

---

## 9. Links

- Execução: [PlanoExecucao-PorProjeto.md](./PlanoExecucao-PorProjeto.md)  
- Anterior: [Analise-Service.md](./Analise-Service.md)  
- Consolidação: [PlanoDeAcao.md](./PlanoDeAcao.md) Fases 6–7 · [Progresso.md](./Progresso.md)
